using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { IsOpen = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.identity, 0);

    }
}
