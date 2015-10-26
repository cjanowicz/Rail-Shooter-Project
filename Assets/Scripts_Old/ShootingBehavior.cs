using UnityEngine;
using System.Collections;

public class ShootingBehavior : MonoBehaviour {
    
	public float m_shotVelocity = 80.0f;
	public Transform shotTransform;

	private FXManager m_fXManagerScript;

    private bool bufferedShot = false;
    private bool shooting = false;
    public int burstShots = 3;
    public float burstShotDelay = 0.1f;


	// Use this for initialization
	void Awake () {
        m_fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if(bufferedShot == true && shooting == false) {
            shooting = true;
            bufferedShot = false;
            Invoke("FireBullet", burstShotDelay);
            Invoke("FireBullet", burstShotDelay*2);
            Invoke("FireBullet", burstShotDelay*3);
            Invoke("ResetShooting", burstShotDelay * 3);
        }
    }

    public void Shoot() {
            bufferedShot = true;
    }

    void ResetShooting() {
        shooting = false;
    }

    void FireBullet() {
        /*
        Rigidbody newBullet = Instantiate(bullet, shotTransform.position, shotTransform.rotation) as Rigidbody;
        newBullet.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
        */
        m_fXManagerScript.CallPlayerBullet(shotTransform.position, shotTransform.rotation, m_shotVelocity);
        m_fXManagerScript.CallPlayerMuzzleFlash(shotTransform.position);
    }

}
