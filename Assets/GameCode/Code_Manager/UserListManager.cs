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
        CheckingUser();
    }


    void CheckingUser()
    {
        // ���� ���� �̸��� �߰� (Firebase Auth���� �����´� ����)
        _userEmails.Add(FirebaseAuthManager.Instance._userEmail); // ���� ���� �̸��� �߰�

        // ����: �ٸ� ���� �̸��� �߰� (�����)
        _userEmails.Add("example1@gmail.com");
        _userEmails.Add("example2@gmail.com");

        // ���� ����Ʈ�� �ؽ�Ʈ �迭�� ���
        for (int i = 0; i < _userName.Length; i++)
        {
            if (i < _userEmails.Count)
            {
                _userName[i].text = _userEmails[i]; // �̸��� ����Ʈ ���
            }
            else
            {
                _userName[i].text = ""; // ����ִ� ĭ �ʱ�ȭ
            }
        }
    }
    public void AddUser(string email)
    {
        if (!_userEmails.Contains(email)) // �ߺ� üũ
        {
            _userEmails.Add(email);
            CheckingUser(); // UI ����
        }
    }

    // Photon �÷��̾� ���� �̺�Ʈ ó��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        string _newPlayerEmail = newPlayer.NickName; // ����: �г��� �Ǵ� �̸���
        AddUser(_newPlayerEmail); // ���� �߰� �� UI ����
    }
}
