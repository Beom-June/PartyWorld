using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;

//  SingleTon���� ���
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

    private FirebaseAuth _auth;     //  �α���, ȸ������ � ���
    private FirebaseUser _user;     //  ������ �Ϸ�� ���� ����

    public string _userId => _user.UserId;

    public Action<bool> _loginState;
    //[SerializeField] private InputField _email;
    //[SerializeField] private InputField _password;
    public void Init()
    {
        _auth = FirebaseAuth.DefaultInstance;

        //  �ӽ�ó��
        if (_auth.CurrentUser != null)
        {
            LogOut();
            Debug.Log(" *** �α׾ƿ� (�Ŵ���) *** ");
        }

        //  ���� Ȯ�ν� ����
        _auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (_auth.CurrentUser != _user)
        {
            bool _signed = (_auth.CurrentUser != _user && _auth.CurrentUser != null);

            if (!_signed && _user != null)
            {
                Debug.Log(" *** �α׾ƿ� *** ");
                _loginState?.Invoke(false);
            }

            _user = _auth.CurrentUser;
            if (_signed)
            {
                Debug.Log(" *** �α��� *** ");
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
                    Debug.LogError(" *** ȸ������ ��� *** ");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError(" *** ȸ������ ���� *** ");
                    return;
                }

                AuthResult _authResult = task.Result;
                FirebaseUser _newUser = _authResult.User;
                Debug.LogError(" *** ȸ������ �Ϸ� *** ");
            });
    }

    public void LogIn(string _email, string _password)
    {
        _auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError(" *** �α��� ��� *** ");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError(" *** �α��� ���� *** ");
                return;
            }

            AuthResult _authResult = task.Result;
            FirebaseUser _newUser = _authResult.User;
            Debug.LogError(" *** �α��� �Ϸ� *** ");
        });
    }

    public void LogOut()
    {
        _auth.SignOut();
        Debug.Log(" *** �α׾ƿ� *** ");
    }
}