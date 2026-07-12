using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HexaChessMono_ListenToInputVector2D:MonoBehaviour
{
    public InputActionReference m_inputActionReference;
    public Vector2 m_value;
    public UnityEvent<Vector2> m_onScreenInputUpdated;
    private void OnEnable()
    {
        if (m_inputActionReference != null)
        {
            m_inputActionReference.action.Enable();
            m_inputActionReference.action.performed += OnInputPerformed;
            m_inputActionReference.action.canceled += OnInputCanceled;
        }
    }
    private void OnDisable()
    {
        if (m_inputActionReference != null)
        {
            m_inputActionReference.action.performed -= OnInputPerformed;
            m_inputActionReference.action.canceled -= OnInputCanceled;
            m_inputActionReference.action.Disable();
        }
    }
    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        m_value = context.ReadValue<Vector2>();
        m_onScreenInputUpdated?.Invoke(m_value);
    }
    private void OnInputCanceled(InputAction.CallbackContext context)
    {
        m_value = Vector2.zero;
        m_onScreenInputUpdated?.Invoke(m_value);
    }
}
