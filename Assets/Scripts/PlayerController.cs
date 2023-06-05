using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] FixedJoystick joystick;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private Vector2 moveVector;
    public NetworkVariable<Vector2> LastNonZeroMoveVector = new NetworkVariable<Vector2>(
        new Vector2(1f, 0f),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<Color> playerColor = new NetworkVariable<Color>(
        new Color(),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            joystick.gameObject.SetActive(false);
        }
        if (IsLocalPlayer) {
            NetworkManager.Singleton.OnClientConnectedCallback -= Relay.Singleton.SpawnPlayer;
        }
        // Инициализация начальных значений игровых параметров
        if (IsHost && IsOwner) {
            Debug.Log("HOST INIT");
            GameManager.Singleton.IsGameStarted.Value = false;
            GameManager.Singleton.PlayersCount.Value = 1;
        }
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        GetComponent<SpriteRenderer>().color = playerColor.Value;
        playerColor.OnValueChanged += OnColorChanged;
        
    }
    void OnColorChanged (Color oldValue, Color newValue) {
        GetComponent<SpriteRenderer>().color = newValue;
    }
 

    // Запрет на передвижение, пока в матче менее 2 игроков
    void Update() {
        if (!IsOwner || !GameManager.Singleton.IsGameStarted.Value) return;
        HandlePlayerMovement();
    }

    void FixedUpdate() {
        if (!IsOwner || !GameManager.Singleton.IsGameStarted.Value) return;
        
        transform.Translate(moveVector * moveSpeed, Space.World);
    }

    private void HandlePlayerMovement() {
        moveVector = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (moveVector.x == 0 && moveVector.y == 0) return;

        // Вектор перемещения пули
        LastNonZeroMoveVector.Value = moveVector;
        float zRotation = Mathf.Atan2(-moveVector.x, moveVector.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, zRotation);
    }

    public override void OnDestroy() {
        if (IsServer) {
            GameManager.Singleton.PlayersCount.Value--;
        }
        playerColor.OnValueChanged -= OnColorChanged;
    }
}
