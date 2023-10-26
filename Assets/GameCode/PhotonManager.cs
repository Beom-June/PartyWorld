using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string _version = "1.0f";                                      //  ���� �Է�
    [SerializeField] private string _userId = "GenJiBoy";                           //  ����� ���̵� �Է�

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;                                //  ���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.GameVersion = _version;                                       //  ���� ������ �������� ���� ���
        PhotonNetwork.NickName = _userId;                                           //  ���� ���̵� �Ҵ�
        Debug.Log(PhotonNetwork.SendRate);                                          //  ��� Ƚ�� ����
        PhotonNetwork.ConnectUsingSettings();                                       //  ���� ����
    }

    //  ���� ������ ���� �� ȣ��Ǵ� CallBack Method
    public override void OnConnectedToMaster()
    {
        //PhotonNetwork.JoinOrCreateRoom("Joint Or CreateRoom", new RoomOptions { IsOpen = true }, TypedLobby.Default);
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();                              //  �κ� ����
    }

    //  �κ� ���� �� ȣ�� �Ǵ� Callback Method
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();                         //  ���� ��ġ ����ŷ ��� ����
    }

    //  ������ �� ������ �������� ��� ȣ��Ǵ� Callback Method
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���н� ���� �����
        Debug.Log($"JoinRandom Filed => {returnCode} : {message}");

        // ���� �Ӽ� ����
        RoomOptions _roomOps = new RoomOptions();
        _roomOps.MaxPlayers = 10;                       //  ����� �ִ� 20
        _roomOps.IsOpen = true;                         //  ���� ���� ����
        _roomOps.IsVisible = true;                      //  �κ񿡼� �� ��Ͽ� ���� ��ų�� ����

        // �� ����
        PhotonNetwork.CreateRoom("My Room", _roomOps);
    }

    // �� ������ �Ϸ�� �� ȣ��Ǵ� Callback Method
    public override void OnCreatedRoom()
    {
        Debug.Log("*** Created Room! ***");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }
    // �뿡 ������ �� ȣ��Ǵ� Callback Method
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount}");

        //  �뿡 ������ ����� ���� Ȯ��
        foreach (var _player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{_player.Value.NickName}, {_player.Value.ActorNumber}");
        }
    }

    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.Instantiate("GenJiBoy", Vector3.zero, Quaternion.identity, 0);

    //}
}
