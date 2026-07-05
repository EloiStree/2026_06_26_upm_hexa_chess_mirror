using UnityEngine;
using Mirror;
using UnityEngine.Events;
public class HexaChessMirrorMono_TimerState : NetworkBehaviour
{

    [SyncVar]
    [SerializeField] float m_timeLeftWhiteAtStart = 60f * 15f;
    [SyncVar]
    [SerializeField] float m_timeLeftBlackAtStart = 60f * 15f;

    [SyncVar]
    [SerializeField] float m_timeLeftWhite = 60f * 15f;
    [SyncVar]
    [SerializeField] float m_timeLeftBlack = 60f* 15f;


    [SyncVar]
    [SerializeField] bool m_isWhiteTurnOn = false;
    [SyncVar]
    [SerializeField] bool m_isBlackTurnOn = false;


    [SerializeField] Event m_event;
    [System.Serializable]
    public class Event {
        [Header("Button Change State")]
        public UnityEvent<bool> m_onClientIsWhiteTurnUpdated;
        public UnityEvent<bool> m_onClientIsBlackTurnUpdated;
        [Header("Time Left Changed State")]    
        public UnityEvent<float> m_onClientTimeLeftWhiteUpdated;
        public UnityEvent<float> m_onClientTimeLeftBlackUpdated;
        [Header("End of time")]
        public UnityEvent m_onClientWhiteMissingTimeEvent;
        public UnityEvent m_onClientBlackMissingTimeEvent;
    }


    public void FixedUpdate()
    {
        if (isServer)
        {
            if (m_isWhiteTurnOn)
            {
                if (m_timeLeftWhite > 0.0f) { 
                    m_timeLeftWhite -= Time.fixedDeltaTime;
                    if (m_timeLeftWhite < 0.0f) {
                        m_timeLeftWhite = 0.0f;
                        RpcWhiteMissingTime();
                    }
                }
            }
            if (m_isBlackTurnOn)
            {
                if (m_timeLeftBlack > 0.0f)
                {
                    m_timeLeftBlack -= Time.fixedDeltaTime;
                    if (m_timeLeftBlack < 0.0f)
                    {
                        m_timeLeftBlack = 0.0f;
                        RpcBlackMissingTime();
                    }
                }
            }

        } 
        m_event.m_onClientIsWhiteTurnUpdated?.Invoke(m_isWhiteTurnOn);
        m_event.m_onClientIsBlackTurnUpdated?.Invoke(m_isBlackTurnOn);
        m_event.m_onClientTimeLeftWhiteUpdated?.Invoke(m_timeLeftWhite);
        m_event.m_onClientTimeLeftBlackUpdated?.Invoke(m_timeLeftBlack);
    }




    [ClientRpc]
    void RpcWhiteMissingTime()
    {
        m_event.m_onClientWhiteMissingTimeEvent?.Invoke();
    }
    [ClientRpc]
    void RpcBlackMissingTime()
    {
        m_event.m_onClientBlackMissingTimeEvent?.Invoke();
    }



    [ContextMenu("Set as Black Turn")]
    [Command(requiresAuthority = false)]
    public void CmdSetAsBlackTurn()
    {
        m_isWhiteTurnOn = false;
        m_isBlackTurnOn = true;

    }
    [ContextMenu("Set as White Turn")]
    [Command(requiresAuthority = false)]
    public void CmdSetAsWhiteTurn()
    {
        m_isWhiteTurnOn = true;
        m_isBlackTurnOn = false;
    }

    [ContextMenu("Set as Pause State")]
    [Command(requiresAuthority = false)]
    public void CmdSetAsPauseState()
    {
        m_isWhiteTurnOn = false;
        m_isBlackTurnOn = false;
    }

    [ContextMenu("Reset the Game Time")]
    [Command(requiresAuthority = false)]
    public void CmdResetTheGameTime() {
        m_timeLeftWhite = m_timeLeftWhiteAtStart;
        m_timeLeftBlack = m_timeLeftBlackAtStart;
        m_isWhiteTurnOn = false;
        m_isBlackTurnOn = false;
    }

}
