using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> Score = new NetworkVariable<int>(0);

    public void Start() {
        Score.OnValueChanged += OnScoreChanged;
    }

    public override void OnDestroy() {
        Score.OnValueChanged -= OnScoreChanged;
    }

    void OnScoreChanged(int oldValue, int newValue) {
        GetComponent<ScoreUI>().scoreText.text = newValue.ToString();
    }
}
