using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] playerSpawnPoints;
    private NetworkObject netObj;
    public int playersSpawned {get; set;} = 0;
    
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
    // Создание персонажа и присвоение его игроку
    public void SpawnPlayerServerRpc(ServerRpcParams serverRpcParams = default) {
        GameManager.Singleton.PlayersCount.Value++;
        GameObject newPlayer;
        newPlayer=(GameObject)Instantiate(playerPrefab);
        // Позиция появления персонажа задаётся в инспекторе юнити
        newPlayer.transform.position = playerSpawnPoints[playersSpawned++].position;  
        netObj=newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        newPlayer.name = newPlayer.name + playersSpawned.ToString();
        var clientId = serverRpcParams.Receive.SenderClientId;
        netObj.SpawnAsPlayerObject(clientId,true);
        newPlayer.GetComponent<PlayerController>().playerColor.Value = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}