using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LobbyManager : MonoBehaviourPun
{
    public GameObject _startButton;


    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _startButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
