using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField nickInput;
    [SerializeField] TMP_InputField codeInput;

    [SerializeField] byte maxPlayers;


    public void CreateRoom()
    {
        PhotonNetwork.LocalPlayer.NickName =
            nickInput.text == "" ? "0" :
            nickInput.text == null ? "Undefined : Case 1" :
            nickInput.text;
        if (PhotonNetwork.LocalPlayer.NickName.Length < 3)
        {
            nickInput.text = "Nick Length >3";
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(codeInput.text, options, TypedLobby.Default);
    }
    public void JoinRoom()
    {
        PhotonNetwork.LocalPlayer.NickName =
            nickInput.text == "" ? "0" :
            nickInput.text == null ? "Undefined : Case 1" :
            nickInput.text;
        if (PhotonNetwork.LocalPlayer.NickName.Length < 3)
        {
            nickInput.text = "Nick Length >3";
            return;
        }

        PhotonNetwork.JoinRoom(codeInput.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}