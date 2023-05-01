using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using mudz;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();

    bool random;
    bool roundRobin;

    enum SpawnOrder{Random, RoundRobin}
    [SerializeField] SpawnOrder spawnOrder = SpawnOrder.Random;

    void Start(){
        for(int i = 0; i <= PhotonNetwork.PlayerList.Length; i++){
            Vector2 vec = new Vector2(0, 0);

            if(spawnOrder == SpawnOrder.Random){
                vec = spawnPoints[Random.Range(0, spawnPoints.Count + 1)].transform.position;
            }
            else if(spawnOrder == SpawnOrder.RoundRobin){
                vec = spawnPoints[i].transform.position;
            }
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, vec, Quaternion.identity);
            PhotonView view = player.GetComponent<PhotonView>();
        }
    }
    
}
