using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UserListManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text[] _userName;
    [SerializeField] private List<string> _userEmails = new List<string>(); // ������ ���� �̸��� ����Ʈ
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    #region Photon �ݹ� ó��
    public override void OnJoinedRoom()
    {
        // ù ��° �÷��̾� ����, ���� �濡 �ִ� ��� �÷��̾� ���� �ʱ�ȭ
        Debug.Log("Joined the room. Updating player list...");
        ListUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ListUpdate();
    }
    // �÷��̾ ������ ��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListUpdate();
    }
    #endregion

    void ListUpdate()
    {
        // �ߺ� ������ ���� ���� ����Ʈ�� �ʱ�ȭ
        _userEmails.Clear();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.NickName != PhotonNetwork.NickName) // �ڱ� �ڽ��� �г��Ӱ� �ٸ� ��츸 �߰�
                _userEmails.Add(p.NickName); // �̸��� ��� �г��� �߰�
        }
        // UI ������Ʈ
        for (int i = 0; i < _userName.Length; i++)
        {
            if (i < _userEmails.Count)
            {
                _userName[i].text = _userEmails[i]; // ���� �̸���/�г��� ���
            }
            else
            {
                _userName[i].text = ""; // ���� ĭ�� �� ���ڿ��� �ʱ�ȭ
            }
        }
    }
}