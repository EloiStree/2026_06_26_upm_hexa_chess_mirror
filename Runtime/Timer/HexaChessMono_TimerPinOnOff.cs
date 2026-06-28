using UnityEngine;

public class HexaChessMono_TimerPinOnOff : MonoBehaviour {

    public Transform m_pinToMove;
    public Transform m_pinOnAnchor;
    public Transform m_pinOffAnchor;

    public void SetPinOnOffState(bool pinIsOn) {

        if (pinIsOn)
        {
            m_pinToMove.position = m_pinOnAnchor.position;
            m_pinToMove.rotation = m_pinOnAnchor.rotation;
        }
        else
        {
            m_pinToMove.position = m_pinOffAnchor.position;
            m_pinToMove.rotation = m_pinOffAnchor.rotation;
        }
    }
    [ContextMenu("Set Pin On")]
    public void SetPinOn()
    {
        SetPinOnOffState(true);
    }
    [ContextMenu("Set Pin Off")]
    public void SetPinOff()
    {
        SetPinOnOffState(false);
    }
}
