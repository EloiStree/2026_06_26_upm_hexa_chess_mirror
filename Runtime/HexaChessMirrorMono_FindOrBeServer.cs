using UnityEngine;
using Mirror;
using System.Collections;
using Mirror.Discovery;

public class HexaChessMirrorMono_FindOrBeServer : MonoBehaviour
{
    [Tooltip("How long to wait for a server reply before giving up and becoming a host (in seconds)")]
    public float searchTimeout = 5f;
    public NetworkDiscovery discovery;

    private bool serverFound = false;

    void Start()
    {
        discovery.OnServerFound.AddListener(OnServerFound);
        StartCoroutine(AutoConnectRoutine());
    }

    private IEnumerator AutoConnectRoutine()
    {
        Debug.Log("Starting LAN search for a server...");

        serverFound = false;
        discovery.StartDiscovery();
        yield return new WaitForSeconds(searchTimeout);
        if (!serverFound)
        {
            Debug.Log("No server found within timeframe. Becoming the Server/Host.");

            // Stop searching
            discovery.StopDiscovery();

            // Start the host (acts as both server and client)
            NetworkManager.singleton.StartHost();

            // Start advertising so other players can find us
            discovery.AdvertiseServer();
        }
    }

    // This is triggered automatically by NetworkDiscovery if a server replies
    private void OnServerFound(ServerResponse info)
    {
        // If we already found a server, ignore any duplicate responses
        if (serverFound) return;

        // Mark that we found one
        serverFound = true;

        Debug.Log($"Server found at {info.uri.Host}. Connecting...");

        // Stop searching
        discovery.StopDiscovery();

        // Stop the coroutine timeout so it doesn't accidentally start a host while we are connecting
        StopAllCoroutines();

        // Connect to the discovered server
        NetworkManager.singleton.StartClient(info.uri);
    }

    void OnDestroy()
    {
        // Clean up the event listener when this script is destroyed
        if (discovery != null)
        {
            discovery.OnServerFound.RemoveListener(OnServerFound);
        }
    }
}