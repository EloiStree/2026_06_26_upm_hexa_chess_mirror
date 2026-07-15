using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HexaChessMirrorMono_MoveToStartAnchor : NetworkBehaviour
{
    public static List<HexaChessMirrorMono_MoveToStartAnchor> Instances { get; } = new();

    [Header("References")]
    [SerializeField] private Transform m_whatToAffect;
    [Header("References")]
    [SerializeField] private Transform m_whereToSpawn;



    private void Reset()
    {
        m_whatToAffect = transform;
    }


    void Awake()
    {
        Instances.Add(this);
    }


    private void OnDestroy()
    {
        Instances.Remove(this);
    }


    [ContextMenu("Reset Pieces At Start Point In Editor")]
    public void ResetPieceToStartPointInEditor() {

        if (m_whatToAffect != null && m_whereToSpawn != null)
        {
            m_whatToAffect.position = m_whereToSpawn.position;
            m_whatToAffect.rotation = m_whereToSpawn.rotation;
        }
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
        if (m_whatToAffect != null && m_whereToSpawn != null)
        {
            m_whatToAffect.position = m_whereToSpawn.position;
            m_whatToAffect.rotation = m_whereToSpawn.rotation;
        }
    }

    public static void S_ResetThemAllAtStart()
    {
        foreach (var piece in Instances)
        {
            if (piece != null)
                piece.CmdResetAllPiecesAtStartPoint();
        }
    }
}
