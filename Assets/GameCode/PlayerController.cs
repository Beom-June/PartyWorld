using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IPunObservable
{
    /// <summary>
    /// Funtion
    /// </summary>
    private float _horizentalAxis;
    private float _verticalAxis;

    [SerializeField] private bool _playerJump;           // 플레이어 점프 bool 값
    [SerializeField] private bool _playerDash;                            // 플레이어 회피 bool 값
    [SerializeField] private bool _isJump;               // 플레이어  점프 제어 bool 값
    [SerializeField] private bool _isDash;                                // 플레이어 회피 제어 bool 값
    [SerializeField] private float _jumpPower = 20.0f;

    /// <summary>
    /// Component
    /// </summary>
    private Vector3 _moveVec;
    private Vector3 _dashVec;                                  // 회피시 방향이 전환되지 않도록 제한
    private Rigidbody _playerRigidbody;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private JoyStick _joyStick;

    /// <summary>
    /// Photon Settings
    /// </summary>
    public PhotonView _pv;

    [Header("PlayerState")]
    [SerializeField] private float _speed = 10f;
    //[SerializeField] VirtualJoyStick virtualJoyStick;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void Start()
    {
        _joyStick = GameObject.Find("BackGround_JoyStick").GetComponent<JoyStick>();

        // GameManager 상태 변화 이벤트 구독
        GameManager._gameManager._onGameStateChange += OnGameStateChange;
    }
    void Update()
    {
        //if (_pv.IsMine)
        if (_pv.IsMine && GameManager._gameManager._currentGameState == GameState.Playing)
        {
            PlayerInput();

            PlayerMove();
            PlayerTurn();
            PlayerJump();
            PlayerDash();

            // NavMeshAgent 상태 체크
            //CheckIfOnGround();
        }
    }
    void OnDestroy()
    {
        // GameManager의 상태 변경 이벤트 구독 해제
        GameManager._gameManager._onGameStateChange -= OnGameStateChange;
        Debug.Log(" *** GameManager 구독 해제 *** ");
    }


    // 키 입력 함수
    void PlayerInput()
    {
        _horizentalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        // JoyStick Build시 사용
        _horizentalAxis = _joyStick.inputHorizontal();
        _verticalAxis = _joyStick.inputVertical();

        _playerJump = Input.GetKeyDown(KeyCode.Space);
        _playerDash = Input.GetKeyDown(KeyCode.LeftShift);
    }


    #region Player 이동 관련
    // 플레이어 이동 함수
    [PunRPC]
    public void PlayerMove()
    {
        _moveVec = new Vector3(_horizentalAxis, 0, _verticalAxis).normalized;
        if (_isDash)
        {
            // 회피 방향이랑 가는 방향이랑 같게
            _moveVec = _dashVec;
        }

        // 목표 속도 설정
        Vector3 _targetVelocity = _moveVec * _speed;
        _targetVelocity.y = _playerRigidbody.velocity.y; // y축 속도는 점프 등으로 인한 변화를 유지

        // 리지드바디의 속도를 직접 설정
        _playerRigidbody.velocity = _targetVelocity;

        _animator.SetBool("isMove", _moveVec != Vector3.zero);
    }

    // 플레이어 시점 함수
    [PunRPC]
    void PlayerTurn()
    {
        // 이동 방향 카메라 시점
        transform.LookAt(transform.position + _moveVec);
    }

    // 플레이어 점프 함수
    [PunRPC]
    void PlayerJump()
    {
        if (_playerJump && !_isJump && !_isDash)
        {
            _navMeshAgent.enabled = false; // NavMeshAgent 비활성화

            // 현재 속도를 가져와서 y축 속도를 점프 파워로 설정
            Vector3 _newVelocity = _playerRigidbody.velocity;
            _newVelocity.y = _jumpPower;
            _playerRigidbody.velocity = _newVelocity;

            _animator.SetTrigger("doJump");
            _isJump = true;
        }
    }

    // 플레이어 회피 함수
    [PunRPC]
    void PlayerDash()
    {
        //if (playerJump && isJump == false && moveVec != Vector3.zero && isDash == false)
        if (_playerDash && !_isJump && _moveVec != Vector3.zero && !_isDash)
        {
            _dashVec = _moveVec;
            _speed *= 2;
            _animator.SetTrigger("doDash");
            _isDash = true;

            // 회피 빠져나오는 속도
            Invoke("PlayerDashEnd", 0.2f);

            // 딜레이를 넣어야함
        }
    }

    // PlayerDash에서 사용 중
    [PunRPC]
    void PlayerDashEnd()
    {
        // 원래 속도로 돌아오게함
        _speed *= 0.5f;
        _isDash = false;
    }
    #endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isJump = false; // 지면에 닿으면 점프 상태 해제
            _isDash = false;
            _navMeshAgent.enabled = true;
            Debug.Log("Player landed");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 트랜스폼 정보를 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 수신 측에서 트랜스폼 정보를 받아 업데이트
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
    // 게임 상태 변경을 처리하는 콜백
    void OnGameStateChange(GameState newGameState)
    {
        Debug.Log("PlayerController: GameState가 " + newGameState + "로 변경되었습니다.");

        if (newGameState == GameState.GameOver)
        {
            // 게임 오버 상태 처리, 예: 플레이어 컨트롤 비활성화
            _speed = 0;
            Debug.Log("PlayerController: 게임 오버 - 플레이어 이동 멈춤");
        }
        else if (newGameState == GameState.Playing)
        {
            // 플레이어 컨트롤 다시 활성화
            _speed = 10f;
            Debug.Log("PlayerController: 게임 시작 - 플레이어 이동 가능");
        }
    }
}
