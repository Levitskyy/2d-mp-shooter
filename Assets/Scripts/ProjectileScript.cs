using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour, IProjectile
{
    public GameObject ProjectileOwner {get; set;} = null;
    public float Damage {get; set;} = 0;
    public float MoveSpeed {get; set;} = 0;
    public Vector2 MoveVector {get; set;} = new Vector2(0, 0);
    public float FlightDuration {get; set;} = 0;


    void Start() {
        var localScale = transform.localScale;
        transform.SetParent(null);
        transform.localScale = localScale;
    }

    void FixedUpdate() {
        transform.Translate(MoveVector * MoveSpeed, Space.World);
    }

    public IEnumerator FlightCoroutine(float flightDuration) {
        yield return new WaitForSeconds(flightDuration);
        if (gameObject == null) yield break;
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerHealth>().Health -= Damage;
            Destroy(gameObject);
        }
        if (other.tag == "Obstacle") Destroy(gameObject);
    }
}
