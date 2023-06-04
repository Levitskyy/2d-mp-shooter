using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GunScript : NetworkBehaviour, IProjectileWeapon
{
    [SerializeField] private Transform fireTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private FireButton fireButton;
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private int maxCapacity;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileFlightDuration;
    private int capacity;
    public float Damage {get => damage; set => damage = value;}
    public float FireRate {get => fireRate; set => fireRate = value;}
    public float ReloadSpeed {get => reloadSpeed; set => reloadSpeed = value;}
    public int MaxCapacity {get => maxCapacity; set => maxCapacity = value;}
    public int Capacity
    {
        get { return capacity; } 
        set {
            if (value > MaxCapacity) capacity = MaxCapacity;
            else capacity = value;
        }
    }
    public float ProjectileSpeed {get => projectileSpeed; set => projectileSpeed = value;}
    public float ProjectileFlightDuration {get => projectileFlightDuration; set => projectileFlightDuration = value;}
    private PlayerController playerController;
    public NetworkVariable<float> LastShotTime = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) {
            fireButton.gameObject.SetActive(false);
        }
        playerController = transform.parent.GetComponent<PlayerController>();
        Capacity = MaxCapacity;
    }

    void Update() {
        if (!IsLocalPlayer || !GameManager.Singleton.IsGameStarted.Value) return;

        if (fireButton.IsFireButtonHeldOn &&
            Capacity > 0 &&
            NetworkManager.ServerTime.TimeAsFloat - LastShotTime.Value > fireRate) {
                LastShotTime.Value = NetworkManager.ServerTime.TimeAsFloat;
                ShootServerRpc();
                if (--Capacity <= 0) Reload();
            }
    }

    public void Shoot() {
        ProjectileScript projectileController = Instantiate(projectilePrefab, fireTransform).GetComponent<ProjectileScript>();
        var projNetOjb = projectileController.GetComponent<NetworkObject>();
        projNetOjb.Spawn();
        projectileController.ProjectileOwner = gameObject;
        projectileController.Damage = Damage;
        projectileController.SetVelocity(playerController.LastNonZeroMoveVector.Value.normalized * ProjectileSpeed);
        projectileController.FlightDuration = ProjectileFlightDuration;     
    }

    public void Reload() {
        StartCoroutine(ReloadCoroutine(ReloadSpeed));
    }

    private IEnumerator ReloadCoroutine(float reloadTime) {
        yield return new WaitForSeconds(reloadTime);
        Capacity = MaxCapacity;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc() {
        Shoot();
    }
}
