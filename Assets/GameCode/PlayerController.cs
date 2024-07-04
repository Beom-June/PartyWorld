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

    [SerializeField] private bool _playerJump;           // �÷��̾� ���� bool ��
    [SerializeField] private bool _playerDash;                            // �÷��̾� ȸ�� bool ��
    [SerializeField] private bool _isJump;               // �÷��̾�  ���� ���� bool ��
    [SerializeField] private bool _isDash;                                // �÷��̾� ȸ�� ���� bool ��
    [SerializeField] private float _jumpPower = 20.0f;

    /// <summary>
    /// Component
    /// </summary>
    private Vector3 _moveVec;
    private Vector3 _dashVec;                                  // ȸ�ǽ� ������ ��ȯ���� �ʵ��� ����
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

        // GameManager ���� ��ȭ �̺�Ʈ ����
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

            // NavMeshAgent ���� üũ
            //CheckIfOnGround();
        }
    }
    void OnDestroy()
    {
        // GameManager�� ���� ���� �̺�Ʈ ���� ����
        GameManager._gameManager._onGameStateChange -= OnGameStateChange;
        Debug.Log(" *** GameManager ���� ���� *** ");
    }


    // Ű �Է� �Լ�
    void PlayerInput()
    {
        _horizentalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        // JoyStick Build�� ���
        _horizentalAxis = _joyStick.inputHorizontal();
        _verticalAxis = _joyStick.inputVertical();

        _playerJump = Input.GetKeyDown(KeyCode.Space);
        _playerDash = Input.GetKeyDown(KeyCode.LeftShift);
    }


    #region Player �̵� ����
    // �÷��̾� �̵� �Լ�
    [PunRPC]
    public void PlayerMove()
    {
        _moveVec = new Vector3(_horizentalAxis, 0, _verticalAxis).normalized;
        if (_isDash)
        {
            // ȸ�� �����̶� ���� �����̶� ����
            _moveVec = _dashVec;
        }

        // ��ǥ �ӵ� ����
        Vector3 _targetVelocity = _moveVec * _speed;
        _targetVelocity.y = _playerRigidbody.velocity.y; // y�� �ӵ��� ���� ������ ���� ��ȭ�� ����

        // ������ٵ��� �ӵ��� ���� ����
        _playerRigidbody.velocity = _targetVelocity;

        _animator.SetBool("isMove", _moveVec != Vector3.zero);
    }

    // �÷��̾� ���� �Լ�
    [PunRPC]
    void PlayerTurn()
    {
        // �̵� ���� ī�޶� ����
        transform.LookAt(transform.position + _moveVec);
    }

    // �÷��̾� ���� �Լ�
    [PunRPC]
    void PlayerJump()
    {
        if (_playerJump && !_isJump && !_isDash)
        {
            _navMeshAgent.enabled = false; // NavMeshAgent ��Ȱ��ȭ

            // ���� �ӵ��� �����ͼ� y�� �ӵ��� ���� �Ŀ��� ����
            Vector3 _newVelocity = _playerRigidbody.velocity;
            _newVelocity.y = _jumpPower;
            _playerRigidbody.velocity = _newVelocity;

            _animator.SetTrigger("doJump");
            _isJump = true;
        }
    }

    // �÷��̾� ȸ�� �Լ�
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

            // ȸ�� ���������� �ӵ�
            Invoke("PlayerDashEnd", 0.2f);

            // �����̸� �־����
        }
    }

    // PlayerDash���� ��� ��
    [PunRPC]
    void PlayerDashEnd()
    {
        // ���� �ӵ��� ���ƿ�����
        _speed *= 0.5f;
        _isDash = false;
    }
    #endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isJump = false; // ���鿡 ������ ���� ���� ����
            _isDash = false;
            _navMeshAgent.enabled = true;
            Debug.Log("Player landed");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Ʈ������ ������ ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // ���� ������ Ʈ������ ������ �޾� ������Ʈ
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
    // ���� ���� ������ ó���ϴ� �ݹ�
    void OnGameStateChange(GameState newGameState)
    {
        Debug.Log("PlayerController: GameState�� " + newGameState + "�� ����Ǿ����ϴ�.");

        if (newGameState == GameState.GameOver)
        {
            // ���� ���� ���� ó��, ��: �÷��̾� ��Ʈ�� ��Ȱ��ȭ
            _speed = 0;
            Debug.Log("PlayerController: ���� ���� - �÷��̾� �̵� ����");
        }
        else if (newGameState == GameState.Playing)
        {
            // �÷��̾� ��Ʈ�� �ٽ� Ȱ��ȭ
            _speed = 10f;
            Debug.Log("PlayerController: ���� ���� - �÷��̾� �̵� ����");
        }
    }
}
