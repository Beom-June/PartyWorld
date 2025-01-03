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
    [SerializeField] private PhotonView _pv;
    [SerializeField] private GameObject _textHide;


    #region 서버연결
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
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";

        // 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0)) // 0은 왼쪽 버튼
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
    //  SEND 버튼에 넣기
    public void Send()
    {
        //_chatInput.text = "";

        string _message = _chatInput.text;

        if (!string.IsNullOrEmpty(_message)) // 입력 필드가 비어있는지 확인
        {
            if (PhotonNetwork.InRoom) // 현재 방에 있는지 확인
            {
                string _email = FirebaseAuthManager.Instance._userEmail;
                _pv.RPC("ChatRPC", RpcTarget.All, _email + " : " + _message);
                //_pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + _chatInput.text);
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

    // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    [PunRPC]
    //void ChatRPC(string msg)
    //{
    //    Debug.Log($"Received message: {msg}"); // 디버그로 메시지 확인
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
    //    if (!_isInput) // 꽉차면 한칸씩 위로 올림
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
        Debug.Log($"Received message: {msg}"); // 디버그로 메시지 확인
        for (int i = 0; i < _chatText.Length; i++)
        {
            if (string.IsNullOrEmpty(_chatText[i].text))
            {
                _chatText[i].text = msg;
                return;
            }
        }

        // 메시지 밀기
        for (int i = 1; i < _chatText.Length; i++)
        {
            _chatText[i - 1].text = _chatText[i].text;
        }
        _chatText[_chatText.Length - 1].text = msg;
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