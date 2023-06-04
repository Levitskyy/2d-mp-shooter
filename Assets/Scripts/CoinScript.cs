using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinScript : NetworkBehaviour
{

    public void Update() {
        CoinAnimation();
    }

    void CoinAnimation() {
        transform.localScale = new Vector3(
            Mathf.Sin(Time.time * 5) / 4 + 0.25f,
            0.5f,
            1
        );
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (!NetworkManager.Singleton.IsServer || !NetworkObject.IsSpawned) return;

        if (other.tag == "Player") {
            other.GetComponent<PlayerScore>().Score.Value++;
            CoinSpawner.Singleton.coinsSpawned--;
            NetworkObject.Despawn();
        }
    }
}
