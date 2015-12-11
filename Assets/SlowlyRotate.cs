using UnityEngine;

public class SlowlyRotate : MonoBehaviour {
    public Vector3 m_axis;
    public float m_defaultSpeed = 1f;
    private float m_speed;
    public float m_speedLerpSpeed = 10f;

    public bool m_isUI = false;
    private float m_lastRealTime = 0;

    private void Start() {
        m_speed = m_defaultSpeed;
        if (m_axis.magnitude == 0) {
            m_axis = new Vector3(0, 0, 1);
        }
    }

    private void OnEnable() {
        m_lastRealTime = Time.realtimeSinceStartup;
    }

    private void Update() {
        if (m_isUI == false) {
            RotateRegular();
        }
        if (m_isUI) {
            RotateUI();
        }
    }

    private void RotateRegular() {
        transform.Rotate(m_axis * Time.deltaTime * m_speed);
        m_speed = Mathf.Lerp(m_speed, m_defaultSpeed,
                              Mathf.Clamp01(Time.deltaTime * m_speedLerpSpeed));
    }

    private void RotateUI() {
        float realDeltaTime = Time.realtimeSinceStartup - m_lastRealTime;
        m_lastRealTime = Time.realtimeSinceStartup;
        transform.Rotate(m_axis * realDeltaTime * m_speed);
        m_speed = Mathf.Lerp(m_speed, m_defaultSpeed,
                              Mathf.Clamp01(realDeltaTime * m_speedLerpSpeed));
    }

    public void SetSpeed(float newSpeed) {
        m_speed = newSpeed;
    }

    public void ResetSpeed() {
        m_speed = m_defaultSpeed;
    }
}