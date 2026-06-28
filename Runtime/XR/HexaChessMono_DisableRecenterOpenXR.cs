using UnityEngine;
using UnityEngine.XR.OpenXR;

public class HexaChessMono_DisableRecenterOpenXR : MonoBehaviour
{
    void Start()
    {
        OpenXRSettings.SetAllowRecentering(false);
    }
}
