using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] playerSpawnPoints;
    private NetworkObject netObj;
    private int playersSpawned = 0;
    
    public static PlayerSpawner Singleton;

    void Start() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership=false)]
    public void SpawnPlayerServerRpc(ServerRpcParams serverRpcParams = default) {
        GameManager.Singleton.PlayersCount++;
        GameObject newPlayer;
        newPlayer=(GameObject)Instantiate(playerPrefab);
        newPlayer.transform.position = playerSpawnPoints[playersSpawned++].position;
        newPlayer.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        netObj=newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        newPlayer.name = newPlayer.name + playersSpawned.ToString();
        var clientId = serverRpcParams.Receive.SenderClientId;
        netObj.SpawnAsPlayerObject(clientId,true);
    }
}