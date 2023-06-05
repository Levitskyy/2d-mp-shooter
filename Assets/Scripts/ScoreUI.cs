using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreUI : NetworkBehaviour
{
    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text codeText;

    void Start() {
        // Отображать счёт и код лобби на определенных клиентах
        if (!IsLocalPlayer) {
            scoreText.gameObject.SetActive(false);
            codeText.transform.parent.gameObject.SetActive(false);
        }
        scoreText.text = "0";
        if (IsLocalPlayer && IsHost) {
            codeText.transform.parent.gameObject.SetActive(true);
            codeText.text = Relay.Singleton.joinCode;
        }
        else {
            codeText.transform.parent.gameObject.SetActive(false);
        }
    }

}
