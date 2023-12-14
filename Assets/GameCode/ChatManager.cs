using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChatManager : MonoBehaviourPunCallbacks
{
    // 채팅창 관련
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

    #region 서버연결
    void Awake()
    {
        //=> Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
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
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        //myList.Clear();
    }

    // 포톤 끊기면
    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        //LobbyPanel.SetActive(false);
        //RoomPanel.SetActive(false);
    }
    #endregion

    #region 디버그용 방 접속 확인
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }
    //void RoomRenewal()
    //{
    //    ListText.text = "";
    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //        ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
    //    RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    //}
    #endregion

    #region 채팅
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

    // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
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
        if (!_isInput) // 꽉차면 한칸씩 위로 올림
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
