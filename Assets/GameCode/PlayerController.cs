using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Funtion
    /// </summary>
    float _horizentalAxis;
    float _verticalAxis;

    bool _playerWalk;                            // �÷��̾� �ȱ� bool ��
    bool _playerJump;                            // �÷��̾� ���� bool ��
    bool _playerDash;                            // �÷��̾� ȸ�� bool ��

    bool _isJump;                                // �÷��̾� ���� ���� bool ��
    bool _isDash;                                // �÷��̾� ȸ�� ���� bool ��
    bool _isAttackReady;                         // �÷��̾� ���� �غ� bool ��

    /// <summary>
    /// Component
    /// </summary>
    Vector3 _moveVec;
    Vector3 _dashVec;                                  // ȸ�ǽ� ������ ��ȯ���� �ʵ��� ����
    Rigidbody _playerRigidbody;
    Animator _animator;
    GameObject nearObj;
    private JoyStick _joyStick;

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
    }
    void Update()
    {
        PlayerInput();

        PlayerMove();
        PlayerTurn();
        PlayerJump();
        PlayerDash();
    }

    // Ű �Է� �Լ�
    void PlayerInput()
    {
        _horizentalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        // JoyStick Build�� ���
        _horizentalAxis = _joyStick.inputHorizontal();
        _verticalAxis = _joyStick.inputVertical();

        _playerWalk = Input.GetKeyDown(KeyCode.LeftControl);
        _playerJump = Input.GetKeyDown(KeyCode.Space);
        _playerDash = Input.GetKeyDown(KeyCode.LeftShift);

    }


    #region Player �̵� ����
    // �÷��̾� �̵� �Լ�
    public void PlayerMove()
    {
        _moveVec = new Vector3(_horizentalAxis, 0, _verticalAxis).normalized;

        if (_isDash)
        {
            // ȸ�� �����̶� ���� �����̶� ����
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

    // �÷��̾� ���� �Լ�
    void PlayerTurn()
    {
        // �̵� ���� ī�޶� ����
        transform.LookAt(transform.position + _moveVec);
    }

    // �÷��̾� ���� �Լ�
    void PlayerJump()
    {
        if (_playerJump && _isJump == false && _moveVec == Vector3.zero && _isDash == false)
        {
            _playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            _animator.SetTrigger("doJump");
            _isJump = true;
        }
    }
    // �÷��̾� ȸ�� �Լ�
    void PlayerDash()
    {
        //if (playerJump && isJump == false && moveVec != Vector3.zero && isDash == false)
        if (_playerDash && _isJump == false && _moveVec != Vector3.zero && _isDash == false)
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
            _isDash = false;
        }
    }
}