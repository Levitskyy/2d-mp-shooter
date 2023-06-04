using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private Vector2 topRightCorner;
    private NetworkObject netObj;
    public int coinsSpawned {get; set;} = 0;
    private float lastSpawnTime = 0f;
    private float coinSpawnCoolDown;
    
    public static CoinSpawner Singleton;

    void Start() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (!NetworkManager.Singleton.IsServer) return;

        if (Time.time - lastSpawnTime > coinSpawnCoolDown && coinsSpawned < 5) {
            SpawnCoinServerRpc();
            coinSpawnCoolDown = Random.Range(3f, 10f);
            lastSpawnTime = Time.time;
        }
    }

    [ServerRpc(RequireOwnership=false)]
    public void SpawnCoinServerRpc() {
        coinsSpawned++;
        GameObject newCoin;
        newCoin =(GameObject)Instantiate(coinPrefab);
        newCoin.transform.position = new Vector3(
            Random.Range(bottomLeftCorner.x, topRightCorner.x),
            Random.Range(bottomLeftCorner.y, topRightCorner.y),
            0
        );
        netObj=newCoin.GetComponent<NetworkObject>();
        newCoin.SetActive(true);
        newCoin.name = newCoin.name + coinsSpawned.ToString();
        netObj.Spawn();
    }
}
