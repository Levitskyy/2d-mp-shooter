using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    public float Health
    {
        get { return health; }
        set 
        {
            health = value;
            if (health <= 0) Die();
        }
    }


    private void Die() {
        Debug.Log("DIED");
    }
}
