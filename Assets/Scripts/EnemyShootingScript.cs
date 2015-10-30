using UnityEngine;
using System.Collections;

public class EnemyShootingScript : MonoBehaviour {

    private static FXManager m_fXManagerScript;
    private static Transform m_playerTrans;
    public Transform shotTransform;
    public float shotVelocity = 40f;

	// Use this for initialization
	void Awake () {
        if(m_fXManagerScript == null) {
            m_fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
            m_playerTrans = GameObject.Find("PlayerShape").transform;
        }
    }

    public void FireAtPlayer() {
        shotTransform.LookAt(m_playerTrans);
		m_fXManagerScript.CallEnemyBullet(shotTransform.position, shotTransform.rotation, shotVelocity);
        m_fXManagerScript.CallPlayerMuzzleFlash(shotTransform.position);
    }
}

