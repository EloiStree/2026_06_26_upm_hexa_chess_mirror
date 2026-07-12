using UnityEngine;

public class HexaChessMirrorMono_MouseRaycastMovePiece : MonoBehaviour
{
    public Camera m_cameraToUse;
    public Transform m_debugChessClickedOn;
    public Transform m_debugNotChessClickedOn;

    [Header("Debug")]
    public Vector2 m_screenPosition;
    public Vector3 m_positionWhenClickedOn;
    public Vector3 m_positionBoardClicked;
    public HexaChessMirrorMono_ClaimAuthorityOfMoving m_lastClickedOn;

    private void Awake()
    {
        if (m_cameraToUse == null)
        {
            m_cameraToUse = Camera.main;
        }
    }

    public void SetScreenPosition(Vector2 screenPosition)
    {
        m_screenPosition = screenPosition;
    }

    public void NotifyClickDetected()
    {
        // Pseudocode plan:
        // 1. Ensure a camera exists before creating a ray.
        // 2. Create a ray from the stored screen position.
        // 3. Stop if nothing was hit.
        // 4. Try to resolve a chess piece component from the hit collider's attached rigidbody.
        // 5. If a piece was clicked:
        //    - claim authority on that piece
        //    - store it as the last clicked piece
        //    - update debug markers safely
        //    - store the piece position at click time
        // 6. If a non-piece surface was clicked:
        //    - update board debug data safely
        //    - if a previously selected piece exists, claim authority again and move it to the hit point
        // 7. Use helper methods to reduce duplication and avoid null reference exceptions.

        if (m_cameraToUse == null)
        {
            return;
        }

        Ray ray = m_cameraToUse.ScreenPointToRay(m_screenPosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            return;
        }

        if (TryGetClickedPiece(hitInfo, out HexaChessMirrorMono_ClaimAuthorityOfMoving clickedOn))
        {
            HandlePieceClicked(clickedOn, hitInfo.point);
            return;
        }

        HandleBoardClicked(hitInfo.point);
    }

    private bool TryGetClickedPiece(RaycastHit hitInfo, out HexaChessMirrorMono_ClaimAuthorityOfMoving clickedOn)
    {
        clickedOn = null;

        Rigidbody attachedRigidbody = hitInfo.collider != null ? hitInfo.collider.attachedRigidbody : null;
        if (attachedRigidbody == null)
        {
            return false;
        }

        return attachedRigidbody.TryGetComponent(out clickedOn);
    }

    private void HandlePieceClicked(HexaChessMirrorMono_ClaimAuthorityOfMoving clickedOn, Vector3 hitPoint)
    {
        clickedOn.ClaimAuthority();
        m_lastClickedOn = clickedOn;
        m_positionWhenClickedOn = clickedOn.transform.position;

        SetDebugTransformPosition(m_debugChessClickedOn, hitPoint);
        SetDebugTransformPosition(m_debugNotChessClickedOn, Vector3.zero);
    }

    private void HandleBoardClicked(Vector3 hitPoint)
    {
        m_positionBoardClicked = hitPoint;

        SetDebugTransformPosition(m_debugNotChessClickedOn, hitPoint);
        SetDebugTransformPosition(m_debugChessClickedOn, Vector3.zero);

        if (m_lastClickedOn == null)
        {
            return;
        }

        m_lastClickedOn.ClaimAuthority();
        m_lastClickedOn.transform.position = hitPoint;
    }

    private static void SetDebugTransformPosition(Transform target, Vector3 position)
    {
        if (target != null)
        {
            target.position = position;
        }
    }
}