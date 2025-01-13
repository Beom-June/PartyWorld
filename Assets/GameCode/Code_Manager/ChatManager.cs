using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviourPunCallbacks
{
    // ä��â ����
    //[SerializeField] private GameObject LobbyPanel;
    //[SerializeField] private Text LobbyInfoText;
    //[SerializeField] private GameObject RoomPanel;
    //[SerializeField] private InputField NickNameInput;
    //[SerializeField] private Text WelcomeText;
    //List<RoomInfo> myList = new List<RoomInfo>();

    //[SerializeField] private Text ListText;
    //[SerializeField] private Text RoomInfoText;
    [SerializeField] private Text[] _chatText;
    [SerializeField] private InputField _chatInput;

    [SerializeField] private Button _sendBtn;

    [Header("ETC")]
    [SerializeField] private Text StatusText;
    [SerializeField] private GameObject _textHide;
    private PhotonView _pv;

    #region ��������

    private void Start()
    {
        // �α����� �̸����� PhotonNetwork.NickName�� ����
        PhotonNetwork.NickName = FirebaseAuthManager.Instance._userEmail;

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.IsMessageQueueRunning = true;

        _sendBtn.onClick.AddListener(Send);

        _pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";

        if (Input.GetMouseButtonDown(0))
        {
            // UI�� �ƴ� �ٸ� ���� Ŭ�� �� Ȱ��ȭ
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                SetTextActive(true);
            }
        }
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        //=> PhotonNetwork.JoinLobby();
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }, null);
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        //LobbyPanel.SetActive(true);
        //RoomPanel.SetActive(false);
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
        //myList.Clear();
        //NotifyPlayerCount();
    }

    // ���� �����
    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        //LobbyPanel.SetActive(false);
        //RoomPanel.SetActive(false);
    }
    #endregion

    #region ����׿� �� ���� Ȯ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //ChatRPC("<color=green>" + PhotonNetwork.NickName + "���� �����ϼ̽��ϴ�</color>");
        //string _newMessage = $"<color=green>{FirebaseAuthManager.Instance._userEmail}���� �����ϼ̽��ϴ�</color>";
        //photonView.RPC("ChatRPC", RpcTarget.All, _newMessage);

        //SyncChatHistoryToNewPlayer(player);

        string _msg = string.Format("<color=green>[{0}]���� �����ϼ̽��ϴ�.</color>", newPlayer.NickName);
        ConnectMessage(_msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //ChatRPC("<color=red>" + PhotonNetwork.NickName + "���� �����ϼ̽��ϴ�</color>");
        //string _newMessage = $"<color=red>{FirebaseAuthManager.Instance._userEmail}���� �����ϼ̽��ϴ�</color>";
        //photonView.RPC("ChatRPC", RpcTarget.All, _newMessage);

        string _msg = string.Format("<color=red>[{0}]���� �����ϼ̽��ϴ�.</color>", otherPlayer.NickName);
        ConnectMessage(_msg);
    }

    //  New Player���� ���� ä�� ���� ����ȭ
    private void SyncChatHistoryToNewPlayer(Player Player)
    {
        foreach (var message in _chatText)
        {
            if (!string.IsNullOrEmpty(message.text))
            {
                photonView.RPC("ChatRPC", Player, message.text); // �� �÷��̾�� �޽��� ����
            }
        }
    }

    private void NotifyPlayerCount()
    {
        if (PhotonNetwork.InRoom) // �濡 �ִ��� Ȯ��
        {
            int _currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            string _message = $"���� �濡 {_currentPlayers}���� ���� ���Դϴ�.";
            _pv.RPC("ChatRPC", RpcTarget.All, $"<color=green>{_message}</color>");
        }
        else
        {
            Debug.LogWarning("�濡 ���� ���� �ƴմϴ�. �÷��̾� ���� �˸� �� �����ϴ�.");
        }
    }

    //  ���� �˸�
    [PunRPC]
    public void ConnectMessage(string msg)
    {
        // �޽����� �и��� ����
        for (int i = 1; i < _chatText.Length; i++)
        {
            _chatText[i - 1].text = _chatText[i].text; // ���� ĭ���� �̵�
            _chatText[i].text = msg;
        }
    }

    #endregion

    #region ä��
    //  Send ��ư�� �ֱ�
    public void Send()
    {
        string _message = _chatInput.text;

        if (!string.IsNullOrEmpty(_message)) // �Է� �ʵ尡 ����ִ��� Ȯ��
        {
            if (PhotonNetwork.InRoom) // ���� �濡 �ִ��� Ȯ��
            {
                string _email = FirebaseAuthManager.Instance._userEmail;
                string _formattedMessage = $"{_email}: {_message}";
                _pv.RPC("ChatRPC", RpcTarget.All, _formattedMessage);
                _chatInput.text = ""; // �Է� �ʵ� �ʱ�ȭ
            }
            else
            {
                Debug.LogError("Not in a room. Cannot send message.");
            }
        }
        else
        {
            Debug.LogError("Message is empty. Cannot send.");
        }
    }

    [PunRPC]
    //  Send���� ������
    void ChatRPC(string msg)
    {
        Debug.Log($"Received message: {msg}");              // ����׷� �޽��� Ȯ��

        // �޽��� �б�
        for (int i = 1; i < _chatText.Length; i++)
        {
            _chatText[i - 1].text = _chatText[i].text;
            _chatText[i].text = msg;
        }
    }
    #endregion

    // GameManager���� ȣ���ϴ� �̺�Ʈ �ڵ鷯
    public void OnGameStateChange(GameState newGameState)
    {
        // ����: ���� ���¿� ���� ä��â�� �޽��� ���
        if (newGameState == GameState.Playing)
        {
            SendSystemMessage(" *** Game Start *** ");
        }
        else if (newGameState == GameState.GameOver)
        {
            SendSystemMessage(" *** Game Over *** ");
        }
    }

    private void SendSystemMessage(string message)
    {
        _pv.RPC("ChatRPC", RpcTarget.All, "<color=red>" + message + "</color>");

    }

    // �ؽ�Ʈ ������Ʈ�� ��Ȱ��ȭ
    public void OnTextClick()
    {
        SetTextActive(false);
    }

    // Ȱ��ȭ ���� ���� �޼���
    private void SetTextActive(bool flag)
    {
        if (_textHide != null)
        {
            _textHide.SetActive(flag);
        }
    }
}