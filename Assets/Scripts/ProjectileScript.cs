using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileScript : NetworkBehaviour, IProjectile
{
    public GameObject ProjectileOwner {get; set;} = null;
    public float Damage {get; set;} = 0;
    public Vector2 MoveVector {get; set;} = new Vector2(0, 0);
    public float FlightDuration {get; set;} = 0;
    private float spawnTime;


    public override void OnNetworkSpawn() {
        var localScale = transform.localScale;
        transform.SetParent(null);
        transform.localScale = localScale;
        spawnTime = Time.time;
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (IsServer)
        {
            MoveVector = velocity;
            SetVelocityClientRpc(velocity);
        }
    }

    [ClientRpc]
    void SetVelocityClientRpc(Vector2 velocity)
    {
        if (!IsHost)
        {
            MoveVector = velocity;
        }
    } 

    void FixedUpdate() {
        transform.Translate(MoveVector, Space.World);
        if (NetworkManager.Singleton.IsServer && NetworkObject.IsSpawned)
            if (Time.time - spawnTime > FlightDuration) NetworkObject.Despawn();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (!NetworkManager.Singleton.IsServer || !NetworkObject.IsSpawned) return;

        if (other.tag == "Player") {
            other.GetComponent<PlayerHealth>().TakeDamage(Damage);
            NetworkObject.Despawn();
        }
        if (other.tag == "Obstacle") NetworkObject.Despawn();
    }
}
