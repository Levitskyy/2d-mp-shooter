using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreUI : NetworkBehaviour
{
    public TMPro.TMP_Text scoreText;

    void Start() {
        if (!IsLocalPlayer) {
            scoreText.gameObject.SetActive(false);
            return;
        }
        scoreText.text = "0";
    }
}
