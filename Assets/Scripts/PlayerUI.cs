using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;

    void Awake() {

        hostButton.onClick.AddListener( () => {
            Relay.Singleton.CreateRelay();
            HideConnectionButtons();
            
        });
        clientButton.onClick.AddListener( () => {
            Relay.Singleton.JoinRelay(inputField.text);
            HideConnectionButtons();
            
        });

    }

    public void HideConnectionButtons() {
        hostButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
    }
}
