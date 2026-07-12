using UnityEngine;
using UnityEngine.Events;

public class HexaChessMirrorMono_AwakeStartEnable : MonoBehaviour
{

    public UnityEvent m_onAwake;
    public UnityEvent m_onStart;
    public UnityEvent m_onEnable;
    public UnityEvent m_onDisable;

    public void Awake()
    {
        m_onAwake?.Invoke();
    }
    public void Start()
    {
        m_onStart?.Invoke();
    }
    public void OnEnable()
    {
        m_onEnable?.Invoke();
    }
    public void OnDisable()
    {
        m_onDisable?.Invoke();
    }

}
