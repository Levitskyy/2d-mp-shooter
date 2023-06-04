using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Singleton;
    public NetworkVariable<int> PlayersCount = new NetworkVariable<int>(0);
    public NetworkVariable<bool> IsGameStarted = new NetworkVariable<bool>(false);
    void Start() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }

        PlayersCount.OnValueChanged += OnPlayersCountChange;
    }

    public override void OnDestroy()
    {
        PlayersCount.OnValueChanged -= OnPlayersCountChange;
    }

    void OnPlayersCountChange(int oldValue, int newValue) {
        if (!IsGameStarted.Value && newValue > 1) {
            IsGameStarted.Value = true;
        }
    }

    [ClientRpc]
    public void ShowWinnerClientRpc(Color winnerColor, int score) {
        PlayerUI.Singleton.ShowWinner(winnerColor, score);
    }
}
