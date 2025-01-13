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
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    #region Photon 콜백 처리
    public override void OnJoinedRoom()
    {
        // 첫 번째 플레이어 포함, 현재 방에 있는 모든 플레이어 정보 초기화
        Debug.Log("Joined the room. Updating player list...");
        ListUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ListUpdate();
    }
    // 플레이어가 나갔을 때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListUpdate();
    }
    #endregion

    void ListUpdate()
    {
        // 중복 방지를 위해 기존 리스트를 초기화
        _userEmails.Clear();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.NickName != PhotonNetwork.NickName) // 자기 자신의 닉네임과 다를 경우만 추가
                _userEmails.Add(p.NickName); // 이메일 대신 닉네임 추가
        }
        // UI 업데이트
        for (int i = 0; i < _userName.Length; i++)
        {
            if (i < _userEmails.Count)
            {
                _userName[i].text = _userEmails[i]; // 유저 이메일/닉네임 출력
            }
            else
            {
                _userName[i].text = ""; // 남는 칸은 빈 문자열로 초기화
            }
        }
    }
}