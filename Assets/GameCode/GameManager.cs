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

    public static int _stage;                        // static으로 전역에서 할 수 있게, 게임 스테이지
    public event Action<GameState> _onGameStateChange;   //  옵저버 패턴 사용 event 

    private PhotonManager _photonMgr;
    private ChatManager _chatMgr;
    [SerializeField] private bool _ischatMgrCheck = false;

    private void Awake()
    {
        _gameManager = this;

        _stage = 1;

        // PhotonManager 참조
        _photonMgr = FindObjectOfType<PhotonManager>();
        if (_photonMgr == null)
        {
            Debug.LogError("PhotonManager를 찾을 수 없습니다!");
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
        // 게임이 시작되면 플레이 상태로
        SetGameState(GameState.Playing);
    }
    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }
    public void InitializeChatManager()
    {
        // ChatManager 참조 및 이벤트 등록
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
        //  Menu 상태
        if (newGameState == GameState.Menu)
        {

        }
        //  Loading 상태
        else if (newGameState == GameState.Loading)
        {

        }
        //  Play 상태
        else if (newGameState == GameState.Playing)
        {
            _photonMgr?.StartGame(); // PhotonManager에서 게임 시작 처리
        }
        //  GameOver 상태
        else if (newGameState == GameState.GameOver)
        {
            _photonMgr?.EndGame(); // PhotonManager에서 게임 오버 처리
        }
        _currentGameState = newGameState;
        _onGameStateChange?.Invoke(newGameState);
    }
}