using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;


public class LogInSystem : MonoBehaviour
{
    [SerializeField] private InputField _email;
    [SerializeField] private InputField _password;

    [SerializeField] private Text _outputTxt;

    void Start()
    {
        FirebaseAuthManager.Instance._loginState += OnChangedState;
        FirebaseAuthManager.Instance.Init();
    }

    private void OnChangedState(bool sign)
    {
        _outputTxt.text = sign ? " *** 로그인 *** " : " *** 로그아웃 *** ";
        _outputTxt.text += FirebaseAuthManager.Instance._userId;

        if (sign)
        {
            // 로그인 상태일 경우 원하는 씬으로 이동
            SceneManager.LoadScene("Scene_Lobby");
        }
    }

    public void Create()
    {
        string _e = _email.text;
        string _p = _password.text;

        FirebaseAuthManager.Instance.Create(_e, _p);

        //  Create 실패시 띄우는 UI   -> 추후 제작
    }
    public void LogIn()
    {
        FirebaseAuthManager.Instance.LogIn(_email.text, _password.text);
    }
    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }
}