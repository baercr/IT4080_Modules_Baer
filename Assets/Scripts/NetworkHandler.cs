using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class NetworkHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
    }

    private bool hasPrinted = false;
    private void PrintMe() {
        if (hasPrinted) {
            return;
        }
        Debug.Log("I AM");
        hasPrinted = true;
        if (IsServer) {
            Debug.Log($"   the Server! {NetworkManager.ServerClientId}");
        }
        if (IsHost) {
            Debug.Log($"   the Host!  {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");
        }
        if (IsClient) {
            Debug.Log($"   a Client! {NetworkManager.LocalClientId}");
        }
        if (!IsServer && !IsClient) {
            Debug.Log("   Nothing yet");
            hasPrinted = false;
        }
    }

    private void OnClientStarted() 
    {
        Debug.Log("!! Client Started !!");

        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDisconnected;
        NetworkManager.OnServerStopped += ClientOnClientStopped;

        PrintMe();
    }

    // -------------------------------------
    // Client Actions
    // -------------------------------------

    private void ClientOnClientConnected(ulong clientId) { 
        PrintMe();
        // Print I {clientId} have connected to the server
        Debug.Log($"{clientId}");
        // handle the case when we are the client running on the host.  Some
        // other client connected.
    }

    private void ClientOnClientDisconnected(ulong clientId) { 
        // print I {clientID} have disconnected from the server.
        // Handle the case of being the client on the host as well.
    }

    private void ClientOnClientStopped(bool indicator) { 
        Debug.Log("!! Client Stopped !!");
        hasPrinted = false;

        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDisconnected;
        NetworkManager.OnServerStopped -= ClientOnClientStopped;
    }



    // -------------------------------------
    // Server Actions
    // -------------------------------------

    private void OnServerStarted() 
    {
        Debug.Log("!! Server Started !!");

        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDisconnected;
        NetworkManager.OnServerStopped += ServerOnServerStopped;

        PrintMe();
    }

    private void ServerOnClientConnected(ulong clientId) { 
        Debug.Log($"Client {clientId} connected to the server");
    }

    private void ServerOnClientDisconnected(ulong clientId) { 
        Debug.Log($"Client {clientId} disconnected from the server");
    }

    private void ServerOnServerStopped(bool indicator) { 
        Debug.Log("!! Server Stopped !!");
        hasPrinted = false;

        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDisconnected;
        NetworkManager.OnServerStopped -= ServerOnServerStopped;
    }
}
