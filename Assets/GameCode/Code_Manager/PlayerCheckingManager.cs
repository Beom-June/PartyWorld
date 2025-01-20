using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PlayerCheckingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Scene> _mapScnene;
    [SerializeField] private List<int> _sceneIdx;                           // �� �ε����� ��� ����Ʈ
    [SerializeField] private string _sceneName;                             // ���� �� �̸�

    [SerializeField] private List<bool> _checkingPlayers;
    [SerializeField] private int _playerCount;                              // �濡 ������ �÷��̾� ��
    [SerializeField] private int _readyCount;                               // �غ� �Ϸ��� �÷��̾� ��

    [Header("Setting Loading")]
    [SerializeField] private bool _isLoadingStarted = false;
    [SerializeField] private float _loading;
    [SerializeField] private float _loadingSpeed = 0.001f;                     // �ε� �ӵ�
    [SerializeField] private float _loadingFinish = 100.0f;                    //  �ε� ��
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
            //_checkingPlayers = new List<bool>(new bool[_playerCount]);          // �÷��̾� ����ŭ ����Ʈ �ʱ�ȭ
            CheckingPlayersSize();

            if (!_isLoadingStarted)
            {
                _isLoadingStarted = true; // �ߺ� ���� ����
                StartCoroutine(PlayerLoading());
            }
        }
        //  �ش� ������ �÷��̾ ���Դ��� üũ
        if (PhotonNetwork.IsMasterClient && _readyCount == _playerCount)
        {
            //PlayStart(); // ��� �÷��̾ �غ�Ǹ� ���� ����
            GoToRandomScene();
        }
    }
    // �÷��̾� �غ� ���� ����
    [PunRPC]
    private void SetPlayerReady()
    {
        int _playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Photon ActorNumber�� 1���� ����
        photonView.RPC("UpdatePlayerReady", RpcTarget.All, _playerIdx, true);
    }

    [PunRPC]
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

    [PunRPC]
    // �ε�  �ڷ�ƾ
    private IEnumerator PlayerLoading()
    {
        while (_loading < _loadingFinish)
        {
            _loading += _loadingSpeed; // �ε� �ӵ���ŭ ����
            //Debug.Log($"���� �ε� ����: {_loading}%");

            // �����Ӹ��� ���
            yield return null; // ���� �����ӱ��� ���
        }

        //Debug.Log("�ε� �Ϸ�!");

        // �ڽ��� �غ� ���¸� true�� ����
        if (_loading >= _loadingFinish)
        {
            SetMyPlayerReady();

        }
    }
    // _checkingPlayers ũ�� ���� �Լ�
    private void CheckingPlayersSize()
    {
        if (_checkingPlayers == null)
        {
            _checkingPlayers = new List<bool>();
        }

        // �÷��̾� ���� ����Ʈ ũ�⺸�� ũ�� false �߰�
        while (_checkingPlayers.Count < _playerCount)
        {
            _checkingPlayers.Add(false);
        }

        // �÷��̾� ���� ����Ʈ ũ�⺸�� ������ �ڿ������� ����
        while (_checkingPlayers.Count > _playerCount)
        {
            _checkingPlayers.RemoveAt(_checkingPlayers.Count - 1);
        }

        Debug.Log($"_checkingPlayers ����: {string.Join(", ", _checkingPlayers)}");
    }
    [PunRPC]
    // �ڽ��� �غ� ���¸� �����ϴ� �޼ҵ�
    private void SetMyPlayerReady()
    {
        if (_checkingPlayers == null || _checkingPlayers.Count == 0)
        {
            Debug.LogError("CheckingPlayers ����Ʈ�� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // �ڽ��� �ε��� ���
        Debug.Log("&&&&&&" + playerIndex);
        if (playerIndex >= 0 && playerIndex < _checkingPlayers.Count)
        {
            _checkingPlayers[playerIndex] = true; // �ڽ��� ���¸� true�� ����
            Debug.Log($"���� CheckingPlayers ����: {playerIndex} {string.Join(", ", _checkingPlayers)}");
        }
        else
        {
            Debug.LogError("��ȿ���� ���� �÷��̾� �ε����Դϴ�!");
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
