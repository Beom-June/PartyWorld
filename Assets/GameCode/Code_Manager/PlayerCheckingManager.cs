using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class PlayerCheckingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Scene> _mapScnene;
    [SerializeField] private List<int> _sceneIdx;  // �� �ε����� ��� ����Ʈ
    [SerializeField] private string _sceneName;        // ���� �� �̸�

    [SerializeField] private List<bool> _checkingPlayers;
    [SerializeField] private int _playerCount;                           // �濡 ������ �÷��̾� ��
    [SerializeField] private int _readyCount;                            // �غ� �Ϸ��� �÷��̾� ��
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    void Start()
    {
        //SetPlayerReady();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            _checkingPlayers = new List<bool>(new bool[_playerCount]);          // �÷��̾� ����ŭ ����Ʈ �ʱ�ȭ
        }
        //  �ش� ������ �÷��̾ ���Դ��� üũ
        if (PhotonNetwork.IsMasterClient && _readyCount == _playerCount)
        {
            //PlayStart(); // ��� �÷��̾ �غ�Ǹ� ���� ����
        }
    }
    // �÷��̾� �غ� ���� ����
    [PunRPC]
    private void SetPlayerReady()
    {
        int _playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Photon ActorNumber�� 1���� ����
        photonView.RPC("UpdatePlayerReady", RpcTarget.All, _playerIdx, true);
    }

    //  �κ� �ִ� �÷��̾ �� üũ�Ǹ� ���� ������ �Ѿ
    private void PlayStart()
    {
        Debug.Log("All players are ready. Loading next scene...");
        System.Console.WriteLine("checking : " + FirebaseAuthManager.Instance._userEmail);     //  �׽�Ʈ�� �����

        if (PhotonNetwork.IsMasterClient)
        {
            // ���ϴ� �� �� �ε� (��: ù ��° ��)
            PhotonNetwork.LoadLevel("Scene_Map01");
        }
    }

    //  ���� ������ ������ �Լ�
    public void GoToRandomScene()
    {
        //  Scene Null �̸� Return
        if (_sceneIdx.Count == 0)
        {
            Debug.LogError(" *** Scene Null !! *** ");
            return;
        }

        // �������� �� �ε����� ����
        int _randIdx = Random.Range(0, _sceneIdx.Count);
        int _selectedSceneIdx = _sceneIdx[_randIdx];

        // ���õ� ���� �̸��� ������
        _sceneName = SceneUtility.GetScenePathByBuildIndex(_selectedSceneIdx);

        // ���õ� �� �ε�
        SceneManager.LoadScene(_selectedSceneIdx);

        // �� �̸��� �ֿܼ� ���
        Debug.Log(" Scene Name : " + _sceneName);
    }
    #region Photon ����ȭ �ڵ�
    // ���ο� �÷��̾ �濡 ������ ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log($"{newPlayer.NickName} has joined. Total players: {_playerCount}");
        Debug.Log($"{FirebaseAuthManager.Instance._userEmail} has joined. Total players: {FirebaseAuthManager.Instance._userEmail.Length}");

        if (PhotonNetwork.IsMasterClient)
        {
            _checkingPlayers.Add(false); // �� �÷��̾� �߰� (�غ� ���´� false)
        }
    }

    // �÷��̾ �濡�� ������ ��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log($"{otherPlayer.NickName} has left. Total players: {_playerCount}");

        if (PhotonNetwork.IsMasterClient)
        {
            _checkingPlayers.RemoveAt(otherPlayer.ActorNumber - 1); // ���� �÷��̾� ����
        }
    }
    #endregion
}
