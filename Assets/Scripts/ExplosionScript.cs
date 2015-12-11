using UnityEngine;

public class ExplosionScript : MonoBehaviour {
    private ParticleSystem m_particleSystem;
    private float m_objectLifetime = 2f;
    private AudioSource m_audioSource;
    private static CameraShake m_camShakeScript;
    public float m_shakeStrength = 0.5f;
    private bool m_firstActivation = true;

    private void Awake() {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_objectLifetime = m_particleSystem.duration;
        m_audioSource = GetComponent<AudioSource>();
        if (m_camShakeScript == null) {
            m_camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

    private void OnEnable() {
        if (m_firstActivation == false) {
            CancelInvoke();
            m_particleSystem.Stop();
            if (m_audioSource != null)
                m_audioSource.Stop();
            Invoke("DeactivateWithTimer", m_objectLifetime);
            if (m_audioSource != null)
                m_audioSource.Play();
            m_camShakeScript.StartCameraShake(m_shakeStrength, this.transform.position);
            m_particleSystem.Play();
        } else
            m_firstActivation = false;
    }

    private void DeactivateWithTimer() {
        m_particleSystem.Stop();
        if (m_audioSource != null)
            m_audioSource.Stop();
        this.gameObject.SetActive(false);
    }
}