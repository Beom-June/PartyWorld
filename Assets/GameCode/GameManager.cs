using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    Menu,
    Loading,
    Playing,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager _gameManager;
    public GameState _currentGameState = GameState.Menu;

    public static int _stage;                        // static���� �������� �� �� �ְ�, ���� ��������
    public event Action<GameState> _onGameStateChange;   //  ������ ���� ��� event 

    private PhotonManager _photonMgr;
    private ChatManager _chatMgr;
    [SerializeField] private bool _ischatMgrCheck = false;

    private void Awake()
    {
        _gameManager = this;

        _stage = 1;

        // PhotonManager ����
        _photonMgr = FindObjectOfType<PhotonManager>();
        if (_photonMgr == null)
        {
            Debug.LogError("PhotonManager�� ã�� �� �����ϴ�!");
        }

        InitializeChatManager();
    }
    void Start()
    {
        StartGame();
    }

    void Update()
    {

    }
    public void StartGame()
    {
        // ������ ���۵Ǹ� �÷��� ���·�
        SetGameState(GameState.Playing);
    }
    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }
    public void InitializeChatManager()
    {
        // ChatManager ���� �� �̺�Ʈ ���
        _chatMgr = FindObjectOfType<ChatManager>();
        _ischatMgrCheck = _chatMgr != null;

        if (_ischatMgrCheck)
        {
            _onGameStateChange += _chatMgr.OnGameStateChange;
        }
        else
        {
            Debug.Log("<color=red>" + " *** ChatManager is Null *** " + "</color>");
        }
    }
    void SetGameState(GameState newGameState)
    {
        //  Menu ����
        if (newGameState == GameState.Menu)
        {

        }
        //  Loading ����
        else if (newGameState == GameState.Loading)
        {

        }
        //  Play ����
        else if (newGameState == GameState.Playing)
        {
            _photonMgr?.StartGame(); // PhotonManager���� ���� ���� ó��
        }
        //  GameOver ����
        else if (newGameState == GameState.GameOver)
        {
            _photonMgr?.EndGame(); // PhotonManager���� ���� ���� ó��
        }
        _currentGameState = newGameState;
        _onGameStateChange?.Invoke(newGameState);
    }
}