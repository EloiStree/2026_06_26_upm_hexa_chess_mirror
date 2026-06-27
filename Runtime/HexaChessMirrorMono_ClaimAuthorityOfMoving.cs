using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HexaChessMirrorMono_ClaimAuthorityOfMoving : NetworkBehaviour
{
    [Header("Target Object")]
    [SerializeField] private NetworkIdentity m_toAffect;

    [SyncVar(hook = nameof(OnOwnerChanged))]
    public uint currentOwnerNetId = 0;

    private void Awake()
    {
        if (m_toAffect == null)
            m_toAffect = GetComponent<NetworkIdentity>();
    }

    private void Start()
    {
        if (m_toAffect == null)
        {
            Debug.LogError($"[{gameObject.name}] m_toAffect is not assigned!", this);
            return;
        }
    }

    [Client]
    public void OnInteracted()
    {
        ClaimAuthority();
    }

    public NetworkIdentity LookInSceneForLocalPlayer()
    {
        return NetworkClient.localPlayer;
    }

    [ContextMenu("Force Claim Authority")]
    public void ClaimAuthority()
    {
        if (m_toAffect == null) return;

        NetworkIdentity localPlayer = LookInSceneForLocalPlayer();
        if (localPlayer == null)
        {
            Debug.LogWarning("ClaimAuthority: Could not find local player.");
            return;
        }

        // Optional: prevent claiming if already owner
        if (currentOwnerNetId == localPlayer.netId)
        {
            Debug.Log("Already own this piece.");
            return;
        }

        ClaimAuthorityFromClientNetworkId(localPlayer.netId);
    }

    [Command(requiresAuthority = false)]
    private void ClaimAuthorityFromClientNetworkId(uint playerNetId)
    {
        if (m_toAffect == null) return;

        if (!NetworkServer.spawned.TryGetValue(playerNetId, out NetworkIdentity playerIdentity))
        {
            Debug.LogWarning($"ClaimAuthority: No player found with netId {playerNetId}.");
            return;
        }

        NetworkConnectionToClient targetConnection = playerIdentity.connectionToClient;
        if (targetConnection == null)
        {
            Debug.LogWarning($"ClaimAuthority: Player {playerNetId} has no client connection.");
            return;
        }

        m_toAffect.RemoveClientAuthority();
        m_toAffect.AssignClientAuthority(targetConnection);
        currentOwnerNetId = playerNetId;   // SyncVar

        Debug.Log($"[Server] Authority transferred to {playerNetId} for {m_toAffect.name}");
    }

    private void OnOwnerChanged(uint oldOwner, uint newOwner)
    {
        Debug.Log($"[Authority] Owner changed: {oldOwner} → {newOwner} | {m_toAffect?.name}");

        // Optional: React here (enable/disable dragging, highlight, etc.)
        bool isMine = newOwner == (NetworkClient.localPlayer?.netId ?? 0);
        // e.g. GetComponent<Draggable>()?.SetInteractable(isMine);
    }
    /// <summary>
    /// Returns true if the local client has authority over m_toAffect
    /// </summary>
    public bool HasAuthority()
    {
        if (m_toAffect == null) return false;

        // Preferred way: check from a NetworkBehaviour on the target object
        var nb = m_toAffect.GetComponent<NetworkBehaviour>();
        if (nb != null)
            return nb.isOwned;

        // Fallback (less common)
        return m_toAffect.isOwned; // NetworkIdentity also has isOwned in newer Mirror
    }
}