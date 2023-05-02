using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using mudz;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;

    bool otherPlayerSpawning = false;

    bool random;
    bool roundRobin;

    int spawnPoint1Amount;
    int spawnPoint2Amount;

    enum SpawnOrder{Random, RoundRobin}
    [SerializeField] SpawnOrder spawnOrder = SpawnOrder.Random;

    void Start(){
        if(spawnOrder == SpawnOrder.Random){
            Debug.Log("Calling SpawnRandom");
            SpawnRandom();
        }
        else{
            Debug.Log("Calling SpawnRound");
            StartCoroutine(SpawnRound());
        }
    }
    public void SpawnRandom()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];
        PhotonNetwork.Instantiate(playerPrefab.name, selectedSpawnPoint.position, Quaternion.identity);
    }
    public IEnumerator SpawnRound()
    {
        Debug.Log("1");
        yield return new WaitUntil(() => !otherPlayerSpawning);
        Debug.Log("2");
        photonView.RPC(nameof(UpdateWaitStatus), RpcTarget.AllBufferedViaServer, true);
        int i = 3;
        if(Mathf.Min(
            spawnPoint1Amount, 
            spawnPoint2Amount) == 
            spawnPoint1Amount
            )
        {
            i = 0;
        } else{
            i = 1;
        }
        Debug.Log("3");
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[i].position, Quaternion.identity);
        photonView.RPC(nameof(UpdateSpawnPointPosition), RpcTarget.AllBufferedViaServer, i);
        photonView.RPC(nameof(UpdateWaitStatus), RpcTarget.AllBufferedViaServer, false);
        Debug.Log("4");
    }
    [PunRPC]
    public void UpdateSpawnPointPosition(int i){
        if(i == 0){
            spawnPoint1Amount++;

        } else{
            spawnPoint2Amount++;
        }
    }
    [PunRPC]
    public void UpdateWaitStatus(bool i){
        otherPlayerSpawning = i;
    }
}
