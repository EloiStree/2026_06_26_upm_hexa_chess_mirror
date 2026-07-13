using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class HexaChessMono_SyncLocalPositionFromBoardAnchor : NetworkBehaviour
{

    [SerializeField] Transform m_toObserveAndMove;
    [SerializeField] Transform m_scaleAnchorPoint;
    [SerializeField] HexaChessMono_BoardCenterToRadius m_anchorPoint;

    public float m_angleThreshold = 1f;
    public float m_positionPercentThreshold = 0.01f;
    public bool m_useUpdateCheckForMovement = true;



    [SyncVar]
    [SerializeField] Vector3 m_localPositionInBoardPercent;
    [SyncVar]
    [SerializeField] Quaternion m_localRotationInBoard;



    public Vector3 m_currentPositionBoardInPercent;
    public Vector3 m_previousPositionBoardInPercent;
    public Quaternion m_currentRotation;
    public Quaternion m_previousRotation;

    public bool m_hasAuthority;
    public bool m_isOwned;


    public override void OnStartClient()
    {
        base.OnStartClient();
        m_hasAuthority = this.authority;
        m_isOwned = this.isOwned;
    }

    private bool IsPlayerMovingThePieceAllowed()
    {
        return m_toObserveAndMove != null  && isOwned;
    }



    public void Update()
    {
        m_hasAuthority = this.authority;
        m_isOwned = this.isOwned;

        if (m_useUpdateCheckForMovement)
        {
            if (IsPlayerMovingThePieceAllowed())
                CheckForAllowedMovement();
        }
    }


    public void CheckForAllowedMovement()
    {
        if (IsPlayerMovingThePieceAllowed())
        {
            ForceUpdateFromCurrentPosition();
        }

    }

    

    private void ForceUpdateFromCurrentPosition()
    {
        m_anchorPoint.GetInPercentPositionAndRotation(m_toObserveAndMove, out Vector3 localPercentPosition, out Quaternion localRotation);

        m_previousPositionBoardInPercent = m_currentPositionBoardInPercent;
        m_currentPositionBoardInPercent = localPercentPosition;

        m_previousRotation = m_currentRotation;
        m_currentRotation = localRotation;

        if (Vector3.Distance(m_previousPositionBoardInPercent, m_currentPositionBoardInPercent) > m_positionPercentThreshold
            ||
            Quaternion.Angle(m_previousRotation, m_currentRotation) > m_angleThreshold)
        {
            CmdUpdateLocalPositionAndRotation(m_currentPositionBoardInPercent, m_currentRotation);
        }
    }

    [Command]
    public void CmdUpdateLocalPositionAndRotation(Vector3 newLocalPercentPosition, Quaternion newLocalRotation)
    {
        m_localPositionInBoardPercent = newLocalPercentPosition;
        m_localRotationInBoard = newLocalRotation;
        RpcUpdateLocalPositionAndRotation(newLocalPercentPosition, newLocalRotation);
    }

    [ClientRpc]
    public void RpcUpdateLocalPositionAndRotation(Vector3 newLocalPercentPosition, Quaternion newLocalRotation)
    {
        if (!isOwned)
        { 
            m_anchorPoint.GetPositionAndRotationFromInPercentValue(
                newLocalPercentPosition, newLocalRotation,
                m_toObserveAndMove);
        }
    }






    public NetworkIdentity LookInSceneForLocalPlayer()
    {
        return NetworkClient.localPlayer;
    }

    [ContextMenu("Force Claim Authority")]
    public void ClaimAuthority()
    {

        NetworkIdentity localPlayer = LookInSceneForLocalPlayer();
        ClaimAuthorityFromClientNetworkId(localPlayer.netId);
    }

    [Command(requiresAuthority = false)]
    private void ClaimAuthorityFromClientNetworkId(uint playerNetId)
    {

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
        NetworkIdentity thisObject = this.GetComponent<NetworkIdentity>();
        thisObject.RemoveClientAuthority();
        thisObject.AssignClientAuthority(targetConnection);

        Debug.Log($"[Server] Authority transferred to {playerNetId} for {playerIdentity.name}");
    }
}
