using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private float MaxHealth = 100f;
    public float Health {
        get => health;
        set {
            health = value;
            if (health <= 0) {
                    Die();
                    health = 0;
            }
            HealthBarSlider.transform.localScale = new Vector3(
                health * healthBarMaxScale / MaxHealth, 
                HealthBarSlider.transform.localScale.y,
                HealthBarSlider.transform.localScale.z
            );
        }
    }
    private float health;
    private float healthBarMaxScale;
    private GameObject HealthBarSlider;
    private GameObject HealthBarCanvas;
    private bool isInstantiated = false;

    public override void OnNetworkSpawn() {
        Debug.Log(gameObject.name + " nspawn");
        InstantiateFields();
    }

    public void Start() {
        Debug.Log(gameObject.name + " start");
        InstantiateFields();
    }
    private void InstantiateFields() {
        isInstantiated = true;
        HealthBarSlider = HealthBar.GetComponent<HealthBarScript>().ScrollBar;
        healthBarMaxScale = HealthBarSlider.transform.localScale.x;
        HealthBarCanvas = HealthBar.transform.parent.gameObject;
        SetHealthServerRpc(MaxHealth);
    }

    public void Update() {
        if (!isInstantiated) return;
        Vector3 position = transform.position + new Vector3(0, 1, 0);
        HealthBarCanvas.transform.localPosition = transform.InverseTransformPoint(position);
        HealthBarCanvas.transform.rotation = Quaternion.Inverse(transform.rotation) * transform.rotation;
    }
    [ServerRpc(RequireOwnership = false)]
    public void GetDamageServerRpc(float damage) {
        GetDamageClientRpc(damage);
    }

    [ClientRpc]
    public void GetDamageClientRpc(float damage) {
        SetHealthClientRpc(Health - damage);
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetHealthServerRpc(float newHealth) {
        SetHealthClientRpc(newHealth);
    }

    [ClientRpc]
    public void SetHealthClientRpc(float newHealth) {
        Health = newHealth;
    }


    private void Die() {
        Debug.Log("DIED");
    }


}
