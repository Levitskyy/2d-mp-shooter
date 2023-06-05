using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public static Relay Singleton;

    public string joinCode; // Код для присоединения к серверу
    private async void Start()
    {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;

        await UnityServices.InitializeAsync();

        // Добавляем функцию, которая будет выполнена при авторизации
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
       await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Авторизуемся анонимно
    }
    // Функция, выполняемая при присоединении клиента
    public void SpawnPlayer(ulong u) {
        PlayerSpawner.Singleton.SpawnPlayerServerRpc();
        Debug.Log("Client connected");
    }
    // Создание сервера и подключение к нему хоста
    public async void CreateRelay() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            Debug.Log("join code: " + joinCode);
        } catch (RelayServiceException e){
            Debug.Log(e);
        }
    }
    // Присоединение к серверу
    public async void JoinRelay(string joinCode) {
        try {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);      
            NetworkManager.Singleton.StartClient();
            
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
    // Debug
    public static void ShowConnectedIds() {
        foreach(var item in NetworkManager.Singleton.ConnectedClientsIds) {
            Debug.Log(item);
        }
    }
}
