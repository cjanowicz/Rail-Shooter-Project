using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    public float m_objectLifetime = 2f;
    private AudioSource m_audioSource;

    void Awake() {
        m_audioSource = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void OnEnable () {
        StartCoroutine(DeactivateWithTimer());
        if(m_audioSource != null)
            m_audioSource.Play();
    }

    IEnumerator DeactivateWithTimer() {
        yield return new WaitForSeconds(m_objectLifetime);
        this.gameObject.SetActive(false);
    }
}
