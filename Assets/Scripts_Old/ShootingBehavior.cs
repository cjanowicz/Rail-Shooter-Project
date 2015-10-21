using UnityEngine;
using System.Collections;

public class ShootingBehavior : MonoBehaviour {

	public Rigidbody bullet;
	public float velocity = 80.0f;
	public Transform shotTransform;

	private GameObject m_fXManager;

    private bool bufferedShot = false;
    private bool shooting = false;
    public int burstShots = 3;
    private int burstIterator;
    public float burstShotDelay = 0.1f;
    private float timer;


	// Use this for initialization
	void Awake () {
		m_fXManager = GameObject.Find("FXManager");
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetButtonDown("Fire1"))
		{
            bufferedShot = true;
		}

        if(bufferedShot == true && shooting == false) {
            shooting = true;
            bufferedShot = false;
            Invoke("FireBullet", burstShotDelay);
            Invoke("FireBullet", burstShotDelay*2);
            Invoke("FireBullet", burstShotDelay*3);
            Invoke("ResetShooting", burstShotDelay * 3);
        }
    }

    void ResetShooting() {
        shooting = false;
    }

    void FireBullet() {
        Rigidbody newBullet = Instantiate(bullet, shotTransform.position, shotTransform.rotation) as Rigidbody;
        newBullet.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
        RenderSettings.haloStrength = 0f;
        m_fXManager.SendMessage("CallPlayerMuzzleFlash", shotTransform.position);
    }

}
