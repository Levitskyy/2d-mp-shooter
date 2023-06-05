using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    // Количество подобранных игроком монет
    public NetworkVariable<int> Score = new NetworkVariable<int>(0);

    public void Start() {
        Score.OnValueChanged += OnScoreChanged;
        GameManager.Singleton.PlayersCount.OnValueChanged += OnGameEnded;
    }

    public override void OnDestroy() {
        Score.OnValueChanged -= OnScoreChanged;
        GameManager.Singleton.PlayersCount.OnValueChanged -= OnGameEnded;
    }

    void OnScoreChanged(int oldValue, int newValue) {
        GetComponent<ScoreUI>().scoreText.text = newValue.ToString();
    }

    // Проверка на окончание матча
    void OnGameEnded(int oldValue, int newValue) {
        if (GameManager.Singleton.IsGameStarted.Value && newValue == 1 && NetworkObject.IsSpawned) {
            SendWinnerColorServerRpc(GetComponent<SpriteRenderer>().color, Score.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendWinnerColorServerRpc(Color color, int score) {
        GameManager.Singleton.ShowWinnerClientRpc(color, score);
    }
}
