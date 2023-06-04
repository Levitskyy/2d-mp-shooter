using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private float MaxHealth = 100f;
    public NetworkVariable<float> Health = new NetworkVariable<float>(100f);
    private float healthBarMaxScale;
    private GameObject HealthBarSlider;
    private GameObject HealthBarCanvas;
    private bool isInstantiated = false;

    public override void OnNetworkSpawn() {
        InstantiateFields();
    }

    public override void OnDestroy()
    {
        Health.OnValueChanged -= OnHealthChanged;
    }

    void OnHealthChanged(float prevValue, float newValue) {
         HealthBarSlider.transform.localScale = new Vector3(
                newValue * healthBarMaxScale / MaxHealth, 
                HealthBarSlider.transform.localScale.y,
                HealthBarSlider.transform.localScale.z
            );
    }
    private void InstantiateFields() {    
        isInstantiated = true;
        HealthBarSlider = HealthBar.GetComponent<HealthBarScript>().ScrollBar;
        healthBarMaxScale = HealthBarSlider.transform.localScale.x;
        HealthBarCanvas = HealthBar.transform.parent.gameObject;
        Health.OnValueChanged += OnHealthChanged;
    }

    public void Update() {
        if (!isInstantiated) return;
        Vector3 position = transform.position + new Vector3(0, 1, 0);
        HealthBarCanvas.transform.localPosition = transform.InverseTransformPoint(position);
        HealthBarCanvas.transform.rotation = Quaternion.Inverse(transform.rotation) * transform.rotation;
    }

    public void TakeDamage(float damage) {
        Health.Value -= damage;
        if (Health.Value <= 0) {
            Health.Value = 0;
            Die();
        }
    }

    private void Die() {
        Debug.Log("DIED");
        NetworkObject.Despawn();
    }


}
