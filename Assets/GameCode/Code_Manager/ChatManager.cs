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
    [SerializeField] private PhotonView _pv;
    [SerializeField] private GameObject _textHide;


    #region ��������
    void Awake()
    {
        //=> Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        _sendBtn.onClick.AddListener(Send);

        _pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";

        // ���콺 Ŭ�� ����
        if (Input.GetMouseButtonDown(0)) // 0�� ���� ��ư
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
        //RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }
    //void RoomRenewal()
    //{
    //    ListText.text = "";
    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //        ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
    //    RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";
    //}
    #endregion

    #region ä��
    //  SEND ��ư�� �ֱ�
    public void Send()
    {
        //_chatInput.text = "";

        string _message = _chatInput.text;

        if (!string.IsNullOrEmpty(_message)) // �Է� �ʵ尡 ����ִ��� Ȯ��
        {
            if (PhotonNetwork.InRoom) // ���� �濡 �ִ��� Ȯ��
            {
                string _email = FirebaseAuthManager.Instance._userEmail;
                _pv.RPC("ChatRPC", RpcTarget.All, _email + " : " + _message);
                //_pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + _chatInput.text);
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

    // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    [PunRPC]
    //void ChatRPC(string msg)
    //{
    //    Debug.Log($"Received message: {msg}"); // ����׷� �޽��� Ȯ��
    //    bool _isInput = false;
    //    for (int i = 0; i < _chatText.Length; i++)
    //    {
    //        if (_chatText[i].text == "")
    //        {
    //            _isInput = true;
    //            _chatText[i].text = msg;
    //            break;
    //        }
    //    }
    //    if (!_isInput) // ������ ��ĭ�� ���� �ø�
    //    {
    //        for (int i = 1; i < _chatText.Length; i++)
    //        {
    //            _chatText[i - 1].text = _chatText[i].text;
    //        }
    //        _chatText[_chatText.Length - 1].text = msg;
    //    }
    //}
    void ChatRPC(string msg)
    {
        Debug.Log($"Received message: {msg}"); // ����׷� �޽��� Ȯ��
        for (int i = 0; i < _chatText.Length; i++)
        {
            if (string.IsNullOrEmpty(_chatText[i].text))
            {
                _chatText[i].text = msg;
                return;
            }
        }

        // �޽��� �б�
        for (int i = 1; i < _chatText.Length; i++)
        {
            _chatText[i - 1].text = _chatText[i].text;
        }
        _chatText[_chatText.Length - 1].text = msg;
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