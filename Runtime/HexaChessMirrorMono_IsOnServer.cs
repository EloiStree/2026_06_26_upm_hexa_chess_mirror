using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class HexaChessMirrorMono_IsOnServer : NetworkBehaviour
{

    public UnityEvent m_onClientStart;
    public UnityEvent m_onServerStart;

    public bool m_isOnHost;

    public UnityEvent<bool> m_onIsOnHostChanged;
    public UnityEvent m_onClientHost;
    public UnityEvent m_onClientNotHost;



    public override void OnStartClient()
    {
        base.OnStartClient();
        m_onClientStart?.Invoke();

        m_isOnHost = NetworkServer.active && NetworkClient.isConnected;
        m_onIsOnHostChanged?.Invoke(m_isOnHost);
        if (m_isOnHost)
        {
            m_onClientHost?.Invoke();
        }
        else
        {
            m_onClientNotHost?.Invoke();
        }
    }

    override public void OnStartServer()
    {
        base.OnStartServer();
        m_onServerStart?.Invoke();
        
    }
}
