using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileScript : MonoBehaviour, IProjectile
{
    public GameObject ProjectileOwner {get; set;} = null;
    public float Damage {get; set;} = 0;
    public float MoveSpeed {get; set;} = 0;
    public Vector2 MoveVector {get; set;} = new Vector2(0, 0);
    public float FlightDuration {get; set;} = 0;
    private float spawnTime;


    public void Start() {
        var localScale = transform.localScale;
        transform.SetParent(null);
        transform.localScale = localScale;
        spawnTime = Time.time;
    }

    void FixedUpdate() {
        transform.Translate(MoveVector * MoveSpeed, Space.World);
        if (Time.time - spawnTime > FlightDuration) Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerHealth>().GetDamageServerRpc(Damage / GameManager.Singleton.PlayersCount);
            Destroy(gameObject);
        }
        if (other.tag == "Obstacle") Destroy(gameObject);
    }
}
