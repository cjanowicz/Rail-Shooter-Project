using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    public float m_objectLifetime = 2f;
    private AudioSource m_audioSource;
    private static CameraShake m_camShakeScript;
    public float m_shakeStrength = 0.5f;
    private bool m_firstActivation = true;

    void Awake() {
        m_audioSource = GetComponent<AudioSource>();
        if(m_camShakeScript == null) {
            m_camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

	// Use this for initialization
	void OnEnable () {
        if (m_firstActivation == false) {
            StartCoroutine(DeactivateWithTimer());
            if (m_audioSource != null)
                m_audioSource.Play();
            m_camShakeScript.StartCameraShake(m_shakeStrength, this.transform.position);
        } else
            m_firstActivation = false;
    }

    IEnumerator DeactivateWithTimer() {
        yield return new WaitForSeconds(m_objectLifetime);
        this.gameObject.SetActive(false);
    }
}
