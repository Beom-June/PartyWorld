using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IPunObservable
{
    /// <summary>
    /// Funtion
    /// </summary>
    float _horizentalAxis;
    float _verticalAxis;

    bool _playerWalk;                            // 플레이어 걷기 bool 값
    bool _playerJump;                            // 플레이어 점프 bool 값
    bool _playerDash;                            // 플레이어 회피 bool 값

    bool _isJump;                                // 플레이어  점프 제어 bool 값
    bool _isDash;                                // 플레이어 회피 제어 bool 값
    bool _isAttackReady;                         // 플레이어 공격 준비 bool 값

    /// <summary>
    /// Component
    /// </summary>
    Vector3 _moveVec;
    Vector3 _dashVec;                                  // 회피시 방향이 전환되지 않도록 제한
    Rigidbody _playerRigidbody;
    Animator _animator;
    GameObject nearObj;
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
        }
    }
    void OnDestroy()
    {
        // GameManager의 상태 변경 이벤트 구독 해제
        GameManager._gameManager._onGameStateChange -= OnGameStateChange;
    }


    // 키 입력 함수
    void PlayerInput()
    {
        _horizentalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        // JoyStick Build시 사용
        _horizentalAxis = _joyStick.inputHorizontal();
        _verticalAxis = _joyStick.inputVertical();

        _playerWalk = Input.GetKeyDown(KeyCode.LeftControl);
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
        if (_playerWalk)
        {
            //transform.position += _moveVec * _speed * 0.3f * Time.deltaTime;
            _playerRigidbody.velocity = _moveVec * _speed * 0.3f;
        }
        else
        {
            //transform.position += _moveVec * _speed * Time.deltaTime;
            _playerRigidbody.velocity = _moveVec * _speed;
        }
        //transform.position += moveVec * Speed * (WalkDown ? 0.3f : 1f) * Time.deltaTime;
        _animator.SetBool("isMove", _moveVec != Vector3.zero);
        _animator.SetBool("isWalk", _playerWalk);

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
        if (_playerJump && _isJump == false && _moveVec == Vector3.zero && _isDash == false)
        {
            _playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            _animator.SetTrigger("doJump");
            _isJump = true;
        }
    }
    // 플레이어 회피 함수
    [PunRPC]
    void PlayerDash()
    {
        //if (playerJump && isJump == false && moveVec != Vector3.zero && isDash == false)
        if (_playerDash && _isJump == false && _moveVec != Vector3.zero && _isDash == false)
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
            _isDash = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
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
