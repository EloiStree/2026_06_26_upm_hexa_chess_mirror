using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Emits UnityEvents when the referenced XRGrabInteractable is grabbed or released.
/// Designed to be assigned by game designers in the Inspector.
/// </summary>
public class HexaChessMono_ListenToXRGrab : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The XR Grab Interactable component to listen to. If null, will try to find one on this GameObject.")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    [Header("Grab Events")]
    [Tooltip("Fired when an interactor grabs the object.")]
    [SerializeField] private UnityEvent onGrabbed;

    [Tooltip("Fired when an interactor releases the object. Includes the interactor that released it.")]
    [SerializeField] private UnityEvent<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor> onReleased;



    [Tooltip("If true, will log debug messages when grab/release events fire.")]
    [SerializeField] private bool debugLogs = false;

   

    #region Unity Lifecycle

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            SubscribeToEvents();
        }
   
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    #endregion

    #region Event Subscription

    private void SubscribeToEvents()
    {
        grabInteractable.selectEntered.AddListener(HandleSelectEntered);
        grabInteractable.selectExited.AddListener(HandleSelectExited);
    }

    private void UnsubscribeFromEvents()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(HandleSelectEntered);
            grabInteractable.selectExited.RemoveListener(HandleSelectExited);
        }
    }

    #endregion

    #region Event Handlers

    private void HandleSelectEntered(SelectEnterEventArgs args)
    {
        if (debugLogs)
        {
            Debug.Log($"[XRGrabEventEmitter] {gameObject.name} GRABBED by {args.interactorObject.transform.name}", this);
        }

        onGrabbed?.Invoke();
    }

    private void HandleSelectExited(SelectExitEventArgs args)
    {
        if (debugLogs)
        {
            Debug.Log($"[XRGrabEventEmitter] {gameObject.name} RELEASED by {args.interactorObject.transform.name}", this);
        }

        onReleased?.Invoke(args.interactorObject);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Manually re-subscribe to events (useful if grabInteractable is changed at runtime).
    /// </summary>
    public void RefreshSubscription()
    {
        UnsubscribeFromEvents();
        if (grabInteractable != null)
        {
            SubscribeToEvents();
        }
    }

    /// <summary>
    /// Sets a new grab interactable and refreshes the subscription.
    /// </summary>
    /// <param name="newInteractable">The new XRGrabInteractable to listen to.</param>
    public void SetGrabInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable newInteractable)
    {
        UnsubscribeFromEvents();
        grabInteractable = newInteractable;
        if (grabInteractable != null)
        {
            SubscribeToEvents();
        }
    }

    #endregion

}