using UnityEngine;
using System.Collections;

public class ShootingBehavior : MonoBehaviour {
    
	public float m_shotVelocity = 80.0f;
	public Transform shotTransform;

	private FXManager m_fXManagerScript;

	[HideInInspector]
    public bool bufferedShot = false;
    private bool shooting = false;
    public int burstShots = 3;
    public float burstShotDelay = 0.1f;

	
	public ReticleBehavior m_retScript;

	// Use this for initialization
	void Awake () {
        m_fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if(bufferedShot == true && shooting == false) {
            shooting = true;
            //bufferedShot = false;
			for(int i = 0; i < burstShots; i++){
            	Invoke("FireBullet", burstShotDelay * i);
			}
			Invoke("ResetShooting", burstShotDelay * burstShots);
        }


		RaycastHit hit;
		
		if (Physics.Raycast(shotTransform.position, shotTransform.forward, out hit)) {
			if(hit.transform.tag == "Enemy"){
				m_retScript.LockOn();
			}else
				m_retScript.LockOff();
			
		}

    }

    public void Shoot() {
            bufferedShot = true;
    }
	public bool GetBufferedShot(){
		return bufferedShot;
	}

    void ResetShooting() {
        shooting = false;
		bufferedShot = false;
    }

    void FireBullet() {
        /*
        Rigidbody newBullet = Instantiate(bullet, shotTransform.position, shotTransform.rotation) as Rigidbody;
        newBullet.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
        */
        m_fXManagerScript.CallPlayerBullet(shotTransform.position, shotTransform.rotation, m_shotVelocity);
		m_retScript.ShotTaken ();
	}

}
