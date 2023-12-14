using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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

    [Header("ETC")]
    [SerializeField] private Text StatusText;
    [SerializeField] private PhotonView _pv;

    #region ��������
    void Awake()
    {
        //=> Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
    //=> PhotonNetwork.JoinLobby();
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }, null);
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
    public void Send()
    {
        //_pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + _chatInput.text);
        //_chatInput.text = "";

        string message = _chatInput.text;
        if (!string.IsNullOrEmpty(message) && PhotonNetwork.InRoom)
        {
            _pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + message);
            _chatInput.text = "";
        }
        else
        {
            Debug.LogError("Cannot send empty message or not in a room.");
        }
    }

    // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    [PunRPC]
    void ChatRPC(string msg)
    {
        bool _isInput = false;
        for (int i = 0; i < _chatText.Length; i++)
        {
            if (_chatText[i].text == "")
            {
                _isInput = true;
                _chatText[i].text = msg;
                break;
            }
        }
        if (!_isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < _chatText.Length; i++)
            {
                _chatText[i - 1].text = _chatText[i].text;
            }
            _chatText[_chatText.Length - 1].text = msg;
        }
    }
    #endregion
}
