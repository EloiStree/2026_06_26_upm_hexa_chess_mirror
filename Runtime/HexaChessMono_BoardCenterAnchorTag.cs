using UnityEngine;

public class HexaChessMono_BoardCenterAnchorTag :MonoBehaviour{

    private static HexaChessMono_BoardCenterAnchorTag m_instanceInScene;
    public static HexaChessMono_BoardCenterAnchorTag GetInstanceInScene() { 
        return m_instanceInScene;
    }
    
    public void Awake()
    {
            m_instanceInScene = this;
    }
}
