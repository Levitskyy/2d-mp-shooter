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
    private NetworkVariable<Vector2> lastNonZeroMoveVector = new NetworkVariable<Vector2>(new Vector2(1, 0),
                                                                                         NetworkVariableReadPermission.Everyone,
                                                                                         NetworkVariableWritePermission.Owner);

    public Vector2 LastNonZeroMoveVector {
        get => lastNonZeroMoveVector.Value;
        set => lastNonZeroMoveVector.Value = value;
    }
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            joystick.gameObject.SetActive(false);
        }
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    void Update() {
        if (!IsOwner) return;
        HandlePlayerMovement();
    }

    void FixedUpdate() {
        if (!IsOwner) return;
        
        transform.Translate(moveVector * moveSpeed, Space.World);
    }

    private void HandlePlayerMovement() {
        moveVector = new Vector2(joystick.Horizontal, joystick.Vertical);

        // set rotation
        if (moveVector.x == 0 && moveVector.y == 0) return;
        LastNonZeroMoveVector = moveVector;
        float zRotation = Mathf.Atan2(-moveVector.x, moveVector.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, zRotation);
    }

    public override void OnDestroy() {
        GameManager.Singleton.PlayersCount--;
    }
}
