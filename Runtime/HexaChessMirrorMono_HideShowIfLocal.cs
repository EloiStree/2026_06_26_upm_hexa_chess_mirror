using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class HexaChessMirrorMono_HideShowIfLocal : NetworkBehaviour
{
    [SerializeField] private UnityEvent m_onIsLocalPlayerEvent;
    [SerializeField] private UnityEvent m_onIsNotLocalPlayerEvent;
    [SerializeField] private UnityEvent<bool> m_onIsLocalPlayerBoolEvent;
    [SerializeField] private UnityEvent<bool> m_onIsNotLocalPlayerBoolEvent;

    [SerializeField]
    private bool m_resultDebugIsLocalPlayer = false;

    public override void OnStartClient()
    {
        Refresh();
    }

    public override void OnStartLocalPlayer()
    {
        Refresh();
    }

    private void Refresh()
    {
        m_resultDebugIsLocalPlayer = isLocalPlayer;
        if (isLocalPlayer)
        {
            m_onIsLocalPlayerEvent?.Invoke();
            m_onIsLocalPlayerBoolEvent?.Invoke(true);
            m_onIsNotLocalPlayerBoolEvent?.Invoke(false);
        }
        else
        {
            m_onIsNotLocalPlayerEvent?.Invoke();
            m_onIsLocalPlayerBoolEvent?.Invoke(false);
            m_onIsNotLocalPlayerBoolEvent?.Invoke(true);
        }
    }
}