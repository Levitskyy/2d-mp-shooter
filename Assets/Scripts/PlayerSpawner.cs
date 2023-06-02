using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefab; //add prefab in inspector
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
        GameObject newPlayer;
        newPlayer=(GameObject)Instantiate(playerPrefab);
        newPlayer.transform.position = playerSpawnPoints[playersSpawned++].position;
        netObj=newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        var clientId = serverRpcParams.Receive.SenderClientId;
        netObj.SpawnAsPlayerObject(clientId,true);
    }
}