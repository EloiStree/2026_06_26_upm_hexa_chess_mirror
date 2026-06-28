using UnityEngine;

public class HexaChessMono_RotatingClockRotation : MonoBehaviour
{

    public Transform m_totalTimeLeftAnchorToRotate;
    public Transform m_secondAnchorToRotate;

    public float m_minuteForFullRotation = 15f;

    public float m_timeReceivedInSeconds = 0f;

    private void OnValidate()
    {
        UpdateCurrentTime(m_timeReceivedInSeconds);
    }

    public void UpdateCurrentTime(float time)
    {
        m_timeReceivedInSeconds = time;
        RefreshClock();
    }

    public void RefreshClock()
    {
        float timeReceived = m_timeReceivedInSeconds;
        float seconds = timeReceived % 60f;
        float totalMinutesPercentage = (m_timeReceivedInSeconds / (m_minuteForFullRotation * 60f));
        float percentageOfSecond = seconds / 60f;

        if (m_secondAnchorToRotate)
            m_secondAnchorToRotate.localEulerAngles = new Vector3(0f, percentageOfSecond * 360f, 0f);
        if (m_totalTimeLeftAnchorToRotate)
            m_totalTimeLeftAnchorToRotate.localEulerAngles = new Vector3(0f, totalMinutesPercentage  * 360f, 0f);
    }
}
