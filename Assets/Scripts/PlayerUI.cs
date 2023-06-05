using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputField;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private Image winnerImage;
    [SerializeField] private Image popUpImage;
    [SerializeField] private Button leaveButton;
    
    public static PlayerUI Singleton;

    void Awake() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }

        hostButton.onClick.AddListener( () => {
            Relay.Singleton.CreateRelay();
            HideConnectionButtons();
            
        });
        clientButton.onClick.AddListener( () => {
            Relay.Singleton.JoinRelay(inputField.text);
            HideConnectionButtons();
            
        });

        // Переход на стартовый экран
        leaveButton.onClick.AddListener( () => {
            NetworkManager.Singleton.Shutdown();
            popUpImage.gameObject.SetActive(false);
            ShowConnectionButtons();
            PlayerSpawner.Singleton.playersSpawned = 0;
            NetworkManager.Singleton.OnClientConnectedCallback += Relay.Singleton.SpawnPlayer;
        });

    }

    public void HideConnectionButtons() {
        hostButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
    }
    public void ShowConnectionButtons() {
        hostButton.gameObject.SetActive(true);
        clientButton.gameObject.SetActive(true);
        inputField.gameObject.SetActive(true);
        inputField.text = "";
    }

    public void ShowWinner(Color winnerColor, int score) {
        popUpImage.gameObject.SetActive(true);
        winnerImage.color = winnerColor;
        scoreText.text = score.ToString();
    }
}
