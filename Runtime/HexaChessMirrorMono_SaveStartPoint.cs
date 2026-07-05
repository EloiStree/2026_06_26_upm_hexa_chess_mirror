using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class HexaChessMirrorMono_SaveStartPoint : NetworkBehaviour
{
    public static List<HexaChessMirrorMono_SaveStartPoint> Instances { get; } = new();

    [Header("References")]
    [SerializeField] private Transform m_whatToAffect;

    [SyncVar]
    private Vector3 m_startPointAtAwake;

    [SyncVar]
    private Quaternion m_startRotationAtAwake;


    private void Reset()
    {
        m_whatToAffect = transform;
    }


    void Awake()
    {
        Instances.Add(this);
    }

    // Use OnStartServer for SyncVar initialization (not Awake)
    public override void OnStartServer()
    {
        base.OnStartServer();
        if (m_whatToAffect != null)
        {
            m_startPointAtAwake = m_whatToAffect.position;
            m_startRotationAtAwake = m_whatToAffect.rotation;
        }
    }

    private void OnDestroy()
    {
        Instances.Remove(this);
    }

    [ContextMenu("Reset Pieces At Start Point")]
    [Command(requiresAuthority = false)]
    public void CmdResetAllPiecesAtStartPoint()
    {
        ResetAllPiecesAtStartPoint();
    }


    [Server]
    void ResetAllPiecesAtStartPoint()
    {
        if (!isServer) 
            return;

        foreach (var piece in Instances)
        {
            if (piece != null)
                piece.RpcResetAtStartPoint();
        }
    }

    [ClientRpc]
    private void RpcResetAtStartPoint()
    {
        if (m_whatToAffect != null) { 
       
            m_whatToAffect.position = m_startPointAtAwake;
            m_whatToAffect.rotation = m_startRotationAtAwake;
        }
    }
}