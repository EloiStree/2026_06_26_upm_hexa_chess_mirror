using System;
using UnityEngine;

public class HexaChessMono_BoardCenterToRadius : MonoBehaviour
{

    private static HexaChessMono_BoardCenterToRadius m_instanceInScene;
    public static HexaChessMono_BoardCenterToRadius GetInstanceInScene()
    {
        return m_instanceInScene;
    }

    public Transform m_centerPointOfBoard;
    public Transform m_radiusPointOfBoard;
    public float m_debugRadiusDistance = 1f;

    private void Awake()
    {
        m_instanceInScene = this;
    }
    private void Update()
    {
        m_debugRadiusDistance= Vector3.Distance(m_centerPointOfBoard.position, m_radiusPointOfBoard.position);
    }

    public Vector3 GetCenterWorldPosition()
    {
        return m_centerPointOfBoard.position;
    }

    public Quaternion getCenterWorldRotation()
    {
        return m_centerPointOfBoard.rotation;
    }

    public float GetRadiusDistance()
    {
        return Vector3.Distance(m_centerPointOfBoard.position, m_radiusPointOfBoard.position);
    }

    public void GetInPercentPositionAndRotation(Transform worldPoint, out Vector3 localPercentPosition, out Quaternion localRotation) { 
    

        GetWorldToLocal_DirectionalPoint(
            worldPoint.position, worldPoint.rotation,
            m_centerPointOfBoard.position, m_centerPointOfBoard.rotation,
            out Vector3 localPosition, out Quaternion localRot);
        float radiusDistance = GetRadiusDistance();
        if (radiusDistance > 0)
        {
            localPercentPosition = localPosition / radiusDistance;
        }
        else
        {
            localPercentPosition = Vector3.zero;
        }
        localRotation = localRot;

    }

    public void GetPositionAndRotationFromInPercentValue(in Vector3 localPercentPosition, in Quaternion localRotation, Transform toMove) { 
    
        float radiusDistance = GetRadiusDistance();
        Vector3 localPosition = localPercentPosition * radiusDistance;
        GetLocalToWorld_DirectionalPoint(
            localPosition, localRotation,
            m_centerPointOfBoard.position, m_centerPointOfBoard.rotation,
            out Vector3 worldPosition, out Quaternion worldRot);
        toMove.position = worldPosition;
        toMove.rotation = worldRot;
    }






    public static void GetWorldToLocal_DirectionalPoint(in Vector3 worldPosition, in Quaternion worldRotation, in Vector3 positionReference, in Quaternion rotationReference, out Vector3 localPosition, out Quaternion localRotation)
    {
        localRotation = Quaternion.Inverse(rotationReference) * worldRotation;
        localPosition = Quaternion.Inverse(rotationReference) * (worldPosition - positionReference);
    }
    public static void GetLocalToWorld_DirectionalPoint(in Vector3 localPosition, in Quaternion localRotation, in Vector3 positionReference, in Quaternion rotationReference, out Vector3 worldPosition, out Quaternion worldRotation)
    {
        worldRotation = rotationReference * localRotation;
        worldPosition = (rotationReference * localPosition) + (positionReference);
    }
}
