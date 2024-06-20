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
    public static GameManager gameManager;
    public GameState currentGameState = GameState.Menu;

    public static int Stage;                        // static���� �������� �� �� �ְ�, ���� ��������
    public event Action<GameState> OnGameStateChange;   //  ������ ���� ��� event 

    private void Awake()
    {
        gameManager = this;

        Stage = 1;
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

    void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.Menu)
        {

        }
        else if (newGameState == GameState.Loading)
        {

        }
        else if (newGameState == GameState.Playing)
        {
             
        }
        else if (newGameState == GameState.GameOver)
        {

        }
        currentGameState = newGameState;
        OnGameStateChange?.Invoke(newGameState);
    }
}