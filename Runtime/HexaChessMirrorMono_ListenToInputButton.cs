using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HexaChessMirrorMono_ListenToInputButton:MonoBehaviour
{
    public InputActionReference m_inputActionReference;
    public bool m_value;
    public UnityEvent<bool> m_onScreenInputUpdated;
    public UnityEvent m_onButtonDown;
    public UnityEvent m_onButtonUp;
    private void OnEnable()
    {
        if (m_inputActionReference != null)
        {
            m_inputActionReference.action.Enable();
            m_inputActionReference.action.performed += OnInputPerformed;
            m_inputActionReference.action.canceled += OnInputPerformed;
            m_inputActionReference.action.started += OnInputPerformed;
        }
    }
    private void OnDisable()
    {
        if (m_inputActionReference != null)
        {
            m_inputActionReference.action.performed -= OnInputPerformed;
            m_inputActionReference.action.canceled -= OnInputPerformed;
            m_inputActionReference.action.started -= OnInputPerformed;
            m_inputActionReference.action.Disable();
        }
    }
    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        bool value = context.ReadValue<float>() > 0.5f;
        bool changed = value != m_value;
        m_value = value;
        m_onScreenInputUpdated?.Invoke(m_value);
        if (changed)
        {
            if (m_value)
                m_onButtonDown?.Invoke();
            else
                m_onButtonUp?.Invoke();
        }
    }
}
