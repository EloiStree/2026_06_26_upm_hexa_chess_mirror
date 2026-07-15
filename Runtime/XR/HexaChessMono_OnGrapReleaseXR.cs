using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HexaChessMono_OnGrapReleaseXR : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private XRGrabInteractable m_grapInteraction;
    [SerializeField] private bool m_useGravityOnRelease = false;
    [SerializeField] private bool m_disableGravityOnRelease = true;

    public UnityEvent m_onRelease;


    private void Reset()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_grapInteraction = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_grapInteraction = GetComponent<XRGrabInteractable>();

        m_grapInteraction.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (m_useGravityOnRelease)
        {
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
        }
        if (m_disableGravityOnRelease)
        {
            m_rigidbody.useGravity = false;
            m_rigidbody.isKinematic = true;
        }
        m_onRelease?.Invoke();
    }

    private void OnDestroy()
    {
        m_grapInteraction.selectExited.RemoveListener(OnRelease);
    }
}