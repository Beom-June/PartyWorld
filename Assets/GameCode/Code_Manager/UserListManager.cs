using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UserListManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text[] _userName;
    [SerializeField] private List<string> _userEmails = new List<string>(); // 접속한 유저 이메일 리스트
    void Start()
    {
        CheckingUser();
    }


    void CheckingUser()
    {
        // 예제 유저 이메일 추가 (Firebase Auth에서 가져온다 가정)
        _userEmails.Add(FirebaseAuthManager.Instance._userEmail); // 현재 유저 이메일 추가

        // 예제: 다른 유저 이메일 추가 (데모용)
        _userEmails.Add("example1@gmail.com");
        _userEmails.Add("example2@gmail.com");

        // 유저 리스트를 텍스트 배열에 출력
        for (int i = 0; i < _userName.Length; i++)
        {
            if (i < _userEmails.Count)
            {
                _userName[i].text = _userEmails[i]; // 이메일 리스트 출력
            }
            else
            {
                _userName[i].text = ""; // 비어있는 칸 초기화
            }
        }
    }
    public void AddUser(string email)
    {
        if (!_userEmails.Contains(email)) // 중복 체크
        {
            _userEmails.Add(email);
            CheckingUser(); // UI 갱신
        }
    }

    // Photon 플레이어 입장 이벤트 처리
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        string _newPlayerEmail = newPlayer.NickName; // 예제: 닉네임 또는 이메일
        AddUser(_newPlayerEmail); // 유저 추가 및 UI 갱신
    }
}
