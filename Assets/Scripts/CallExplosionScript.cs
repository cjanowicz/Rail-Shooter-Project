using UnityEngine;
using System.Collections;

public class CallExplosionScript : MonoBehaviour {

    public static Transform m_FXManagerTran;
    public GameObject m_explosionPrefab;

    // Use this for initialization
    void Awake () {
	    if(m_FXManagerTran == null) {
            m_FXManagerTran = GameObject.Find("FXManager").transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
