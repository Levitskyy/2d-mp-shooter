using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private float MaxHealth = 100f;
    private NetworkVariable<float> health = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner);
    private float healthBarMaxScale;
    private GameObject HealthBarSlider;
    private GameObject HealthBarCanvas;
    private Transform playerTransform;
    public float Health
    {
        get { return health.Value; }
        set 
        {
            health.Value = value;
            if (health.Value <= 0) {
                 Die();
                 health.Value = 0;
            }
            HealthBarSlider.transform.localScale = new Vector3(
                Health * healthBarMaxScale / MaxHealth, 
                HealthBarSlider.transform.localScale.y,
                HealthBarSlider.transform.localScale.z
            );
            
        }
    }

    public void Start() {
        HealthBarSlider = HealthBar.GetComponent<HealthBarScript>().ScrollBar;
        healthBarMaxScale = HealthBarSlider.transform.localScale.x;
        Health = MaxHealth;
        playerTransform = transform;
        HealthBarCanvas = HealthBar.transform.parent.gameObject;
        HealthBarCanvas.transform.SetParent(null);
    }

    public void Update() {
        Vector3 position = playerTransform.position + new Vector3(0, 1, 0);
        HealthBarCanvas.transform.position = position;
    }

    public void GetDamage(float damage) {
        Health -= damage;
    }


    private void Die() {
        Debug.Log("DIED");
    }
}
