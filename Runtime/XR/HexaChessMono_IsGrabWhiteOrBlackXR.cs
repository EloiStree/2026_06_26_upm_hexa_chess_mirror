using Eloi.HexaChess;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HexaChessMono_IsGrabWhiteOrBlackXR : MonoBehaviour
{
    [Header("XR Interactors (Controllers)")]
    [SerializeField] private XRBaseInteractor[] m_interactors;

    [Header("Release Events")]
    public UnityEvent m_onReleaseWhite;
    public UnityEvent m_onReleaseBlack;
    public UnityEvent m_onReleaseNotChessPiece;

    private GameObject m_lastReleasedObject;

    private void OnEnable()
    {
        if (m_interactors == null || m_interactors.Length == 0)
        {
            m_interactors = FindObjectsByType<XRBaseInteractor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        foreach (var interactor in m_interactors)
        {
            if (interactor != null)
            {
                interactor.selectExited.AddListener(OnObjectReleased);
            }
        }

        if (m_interactors == null || m_interactors.Length == 0)
        {
            Debug.LogError("No XRBaseInteractor found in scene.", this);
        }
    }

    private void OnDisable()
    {
        if (m_interactors == null)
            return;

        foreach (var interactor in m_interactors)
        {
            if (interactor != null)
            {
                interactor.selectExited.RemoveListener(OnObjectReleased);
            }
        }
    }

    private void OnObjectReleased(SelectExitEventArgs args)
    {
        if (args.interactableObject == null)
        {
            m_onReleaseNotChessPiece?.Invoke();
            return;
        }

        m_lastReleasedObject = args.interactableObject.transform.gameObject;

        if (m_lastReleasedObject.TryGetComponent(out HexaChessMono_TagChessEnumType chessTag))
        {
            if (chessTag.IsWhiteColor())
            {
                m_onReleaseWhite?.Invoke();
            }
            else
            {
                m_onReleaseBlack?.Invoke();
            }
        }
        else
        {
            m_onReleaseNotChessPiece?.Invoke();
        }
    }

    public bool IsWhiteColor()
    {
        if (m_lastReleasedObject == null)
            return false;

        if (m_lastReleasedObject.TryGetComponent(out HexaChessMono_TagChessEnumType chessTag))
        {
            return chessTag.IsWhiteColor();
        }

        return false;
    }
}