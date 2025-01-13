using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

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

    [SerializeField] private Button _sendBtn;

    [Header("ETC")]
    [SerializeField] private Text StatusText;
    [SerializeField] private GameObject _textHide;
    private PhotonView _pv;

    #region 서버연결

    private void Start()
    {
        // 로그인한 이메일을 PhotonNetwork.NickName에 설정
        PhotonNetwork.NickName = FirebaseAuthManager.Instance._userEmail;

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.IsMessageQueueRunning = true;

        _sendBtn.onClick.AddListener(Send);

        _pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";

        if (Input.GetMouseButtonDown(0))
        {
            // UI가 아닌 다른 영역 클릭 시 활성화
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
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        //myList.Clear();
        //NotifyPlayerCount();
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
        //ChatRPC("<color=green>" + PhotonNetwork.NickName + "님이 참가하셨습니다</color>");
        //string _newMessage = $"<color=green>{FirebaseAuthManager.Instance._userEmail}님이 참가하셨습니다</color>";
        //photonView.RPC("ChatRPC", RpcTarget.All, _newMessage);

        //SyncChatHistoryToNewPlayer(player);

        string _msg = string.Format("<color=green>[{0}]님이 입장하셨습니다.</color>", newPlayer.NickName);
        ConnectMessage(_msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //ChatRPC("<color=red>" + PhotonNetwork.NickName + "님이 퇴장하셨습니다</color>");
        //string _newMessage = $"<color=red>{FirebaseAuthManager.Instance._userEmail}님이 퇴장하셨습니다</color>";
        //photonView.RPC("ChatRPC", RpcTarget.All, _newMessage);

        string _msg = string.Format("<color=red>[{0}]님이 퇴장하셨습니다.</color>", otherPlayer.NickName);
        ConnectMessage(_msg);
    }

    //  New Player에게 기존 채팅 내역 동기화
    private void SyncChatHistoryToNewPlayer(Player Player)
    {
        foreach (var message in _chatText)
        {
            if (!string.IsNullOrEmpty(message.text))
            {
                photonView.RPC("ChatRPC", Player, message.text); // 새 플레이어에게 메시지 전송
            }
        }
    }

    private void NotifyPlayerCount()
    {
        if (PhotonNetwork.InRoom) // 방에 있는지 확인
        {
            int _currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            string _message = $"현재 방에 {_currentPlayers}명이 접속 중입니다.";
            _pv.RPC("ChatRPC", RpcTarget.All, $"<color=green>{_message}</color>");
        }
        else
        {
            Debug.LogWarning("방에 접속 중이 아닙니다. 플레이어 수를 알릴 수 없습니다.");
        }
    }

    //  접속 알림
    [PunRPC]
    public void ConnectMessage(string msg)
    {
        // 메시지가 밀리는 로직
        for (int i = 1; i < _chatText.Length; i++)
        {
            _chatText[i - 1].text = _chatText[i].text; // 이전 칸으로 이동
            _chatText[i].text = msg;
        }
    }

    #endregion

    #region 채팅
    //  Send 버튼에 넣기
    public void Send()
    {
        string _message = _chatInput.text;

        if (!string.IsNullOrEmpty(_message)) // 입력 필드가 비어있는지 확인
        {
            if (PhotonNetwork.InRoom) // 현재 방에 있는지 확인
            {
                string _email = FirebaseAuthManager.Instance._userEmail;
                string _formattedMessage = $"{_email}: {_message}";
                _pv.RPC("ChatRPC", RpcTarget.All, _formattedMessage);
                _chatInput.text = ""; // 입력 필드 초기화
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
    //  Send에서 선언중
    void ChatRPC(string msg)
    {
        Debug.Log($"Received message: {msg}");              // 디버그로 메시지 확인

        // 메시지 밀기
        for (int i = 1; i < _chatText.Length; i++)
        {
            _chatText[i - 1].text = _chatText[i].text;
            _chatText[i].text = msg;
        }
    }
    #endregion

    // GameManager에서 호출하는 이벤트 핸들러
    public void OnGameStateChange(GameState newGameState)
    {
        // 예시: 게임 상태에 따른 채팅창에 메시지 출력
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

    // 텍스트 오브젝트를 비활성화
    public void OnTextClick()
    {
        SetTextActive(false);
    }

    // 활성화 상태 설정 메서드
    private void SetTextActive(bool flag)
    {
        if (_textHide != null)
        {
            _textHide.SetActive(flag);
        }
    }
}