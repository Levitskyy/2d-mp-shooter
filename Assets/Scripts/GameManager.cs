using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public int PlayersCount {get; set;} = 0;
    void Start() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
    }
}
