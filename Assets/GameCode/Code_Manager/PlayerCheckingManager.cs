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
    [SerializeField] private List<int> _sceneIdx;  // 씬 인덱스를 담는 리스트
    [SerializeField] private string _sceneName;        // 현재 씬 이름

    [SerializeField] private List<bool> _checkingPlayers;
    [SerializeField] private int _playerCount;                           // 방에 접속한 플레이어 수
    [SerializeField] private int _readyCount;                            // 준비 완료한 플레이어 수
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
            _checkingPlayers = new List<bool>(new bool[_playerCount]);          // 플레이어 수만큼 리스트 초기화
        }
        //  해당 씬으로 플레이어가 들어왔는지 체크
        if (PhotonNetwork.IsMasterClient && _readyCount == _playerCount)
        {
            //PlayStart(); // 모든 플레이어가 준비되면 게임 시작
        }
    }
    // 플레이어 준비 상태 갱신
    [PunRPC]
    private void SetPlayerReady()
    {
        int _playerIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Photon ActorNumber는 1부터 시작
        photonView.RPC("UpdatePlayerReady", RpcTarget.All, _playerIdx, true);
    }

    //  로비에 있던 플레이어가 다 체크되면 다음 씬으로 넘어감
    private void PlayStart()
    {
        Debug.Log("All players are ready. Loading next scene...");
        System.Console.WriteLine("checking : " + FirebaseAuthManager.Instance._userEmail);     //  테스트용 디버그

        if (PhotonNetwork.IsMasterClient)
        {
            // 원하는 맵 씬 로드 (예: 첫 번째 맵)
            PhotonNetwork.LoadLevel("Scene_Map01");
        }
    }

    //  랜덤 씬으로 보내는 함수
    public void GoToRandomScene()
    {
        //  Scene Null 이면 Return
        if (_sceneIdx.Count == 0)
        {
            Debug.LogError(" *** Scene Null !! *** ");
            return;
        }

        // 랜덤으로 씬 인덱스를 선택
        int _randIdx = Random.Range(0, _sceneIdx.Count);
        int _selectedSceneIdx = _sceneIdx[_randIdx];

        // 선택된 씬의 이름을 가져옴
        _sceneName = SceneUtility.GetScenePathByBuildIndex(_selectedSceneIdx);

        // 선택된 씬 로드
        SceneManager.LoadScene(_selectedSceneIdx);

        // 씬 이름을 콘솔에 출력
        Debug.Log(" Scene Name : " + _sceneName);
    }
    #region Photon 동기화 코드
    // 새로운 플레이어가 방에 들어왔을 때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log($"{newPlayer.NickName} has joined. Total players: {_playerCount}");
        Debug.Log($"{FirebaseAuthManager.Instance._userEmail} has joined. Total players: {FirebaseAuthManager.Instance._userEmail.Length}");

        if (PhotonNetwork.IsMasterClient)
        {
            _checkingPlayers.Add(false); // 새 플레이어 추가 (준비 상태는 false)
        }
    }

    // 플레이어가 방에서 나갔을 때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log($"{otherPlayer.NickName} has left. Total players: {_playerCount}");

        if (PhotonNetwork.IsMasterClient)
        {
            _checkingPlayers.RemoveAt(otherPlayer.ActorNumber - 1); // 떠난 플레이어 제거
        }
    }
    #endregion
}
