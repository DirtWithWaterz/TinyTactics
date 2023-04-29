using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenu : MonoBehaviourPun
{
    [SerializeField] GameObject settingsPage;
    [SerializeField] GameObject creditsPage;
    [SerializeField] GameObject loadingPage;

    bool settingsToggle;
    bool creditsToggle;


    public void StartGame(){
        loadingPage.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene("LoadingLobby");
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void ToggleSettings(){
        settingsToggle = !settingsToggle;
        settingsPage.SetActive(settingsToggle);
    }
    public void ToggleCredits(){
        creditsToggle = !creditsToggle;
        creditsPage.SetActive(creditsToggle);
    }
}
