using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    public float m_objectLifetime = 2f;
    private float m_timer;

	// Use this for initialization
	void OnEnable () {
        StartCoroutine(DeactivateWithTimer());
	}

    IEnumerator DeactivateWithTimer() {
        yield return new WaitForSeconds(m_objectLifetime);
        this.gameObject.SetActive(false);
    }
}
