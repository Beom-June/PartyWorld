using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;

//  SingleTon으로 사용
public class FirebaseAuthManager
{
    private static FirebaseAuthManager _instance = null;

    public static FirebaseAuthManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FirebaseAuthManager();
            }

            return _instance;
        }
    }

    private FirebaseAuth _auth;     //  로그인, 회원가입 등에 사용
    private FirebaseUser _user;     //  인증이 완료된 유저 정보

    public string _userId => _user.UserId;

    public Action<bool> _loginState;
    //[SerializeField] private InputField _email;
    //[SerializeField] private InputField _password;
    public void Init()
    {
        _auth = FirebaseAuth.DefaultInstance;

        //  임시처리
        if (_auth.CurrentUser != null)
        {
            LogOut();
            Debug.Log(" *** 로그아웃 (매니저) *** ");
        }

        //  변경 확인시 변경
        _auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (_auth.CurrentUser != _user)
        {
            bool _signed = (_auth.CurrentUser != _user && _auth.CurrentUser != null);

            if (!_signed && _user != null)
            {
                Debug.Log(" *** 로그아웃 *** ");
                _loginState?.Invoke(false);
            }

            _user = _auth.CurrentUser;
            if (_signed)
            {
                Debug.Log(" *** 로그인 *** ");
                _loginState?.Invoke(true);
            }
        }
    }

    public void Create(string _email, string _password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError(" *** 회원가입 취소 *** ");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError(" *** 회원가입 실패 *** ");
                    return;
                }

                AuthResult _authResult = task.Result;
                FirebaseUser _newUser = _authResult.User;
                Debug.LogError(" *** 회원가입 완료 *** ");
            });
    }

    public void LogIn(string _email, string _password)
    {
        _auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError(" *** 로그인 취소 *** ");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError(" *** 로그인 실패 *** ");
                return;
            }

            AuthResult _authResult = task.Result;
            FirebaseUser _newUser = _authResult.User;
            Debug.LogError(" *** 로그인 완료 *** ");
        });
    }

    public void LogOut()
    {
        _auth.SignOut();
        Debug.Log(" *** 로그아웃 *** ");
    }
}