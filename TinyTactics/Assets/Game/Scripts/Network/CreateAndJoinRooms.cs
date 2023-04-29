using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField Input;

    [SerializeField] byte maxPlayers;

    public void CreateRoom(){
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(Input.text, options, TypedLobby.Default);
    }
    public void JoinRoom(){
        PhotonNetwork.JoinRoom(Input.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
