using UnityEngine;

public class HexaChessMono_FollowIt : MonoBehaviour
{
    public Transform m_whatToFollow;
    public Transform m_whatToMove;


    private void Reset()
    {
        m_whatToMove = this.transform;
    }
    void Update()
    {
        if (m_whatToFollow != null && m_whatToMove != null)
        {
            m_whatToMove.position = m_whatToFollow.position;
            m_whatToMove.rotation = m_whatToFollow.rotation;
        }
    }
}
