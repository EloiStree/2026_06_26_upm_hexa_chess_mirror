using UnityEngine;
using UnityEngine.Events;

public class HexaChessMono_AnyCollisionEnterToUnityEvent : MonoBehaviour
{
    public UnityEvent m_onCollisionEnterEvent;

    public bool m_useCollisionEnter=true;
    public bool m_useTriggerEnter;

    public bool m_useTagNameFilter=true;
    public string m_tagNameFilter="TimerInteractable";


    private void OnCollisionEnter(Collision collision)
    {
        if (m_useCollisionEnter)
        {
            if (m_useTagNameFilter)
            {
                if (collision.gameObject.CompareTag(m_tagNameFilter))
                {
                    m_onCollisionEnterEvent.Invoke();
                }
            }
            else
            {
                m_onCollisionEnterEvent.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_useTriggerEnter)
        {
            if (m_useTagNameFilter)
            {
                if (other.CompareTag(m_tagNameFilter))
                {
                    m_onCollisionEnterEvent.Invoke();
                }
            }
            else
            {
                m_onCollisionEnterEvent.Invoke();
            }
        }
    }
}
