using UnityEngine;
public class HexaChessMono_BoardScaleRadiusTag : MonoBehaviour
{

    private static HexaChessMono_BoardScaleRadiusTag m_instanceInScene;
    public static HexaChessMono_BoardScaleRadiusTag GetInstanceInScene()
    {
        return m_instanceInScene;
    }

    public void Awake()
    {
        m_instanceInScene = this;
    }
    public static float GetDistanceCenterToRadius()
    {
        if (HexaChessMono_BoardCenterAnchorTag.GetInstanceInScene() != null)
        {
            float distanceCenterToRadius = Vector3.Distance(HexaChessMono_BoardCenterAnchorTag.GetInstanceInScene().transform.position, HexaChessMono_BoardScaleRadiusTag.GetInstanceInScene().transform.position);
            return distanceCenterToRadius;
        }
        return 0;
    }
}
