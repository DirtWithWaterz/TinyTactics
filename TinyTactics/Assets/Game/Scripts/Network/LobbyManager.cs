using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LobbyManager : MonoBehaviourPun
{
    public GameObject _startButton;

    public void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient)
        {
            _startButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
