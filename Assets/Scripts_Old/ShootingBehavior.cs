using UnityEngine;
using System.Collections;

public class ShootingBehavior : MonoBehaviour {

	public Rigidbody bullet;
	public float velocity = 80.0f;
	public Transform shotTransform;

	private GameObject m_fXManager;


	// Use this for initialization
	void Awake () {
		m_fXManager = GameObject.Find("FXManager");
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetButtonDown("Fire1"))
		{
			Rigidbody newBullet = Instantiate(bullet, shotTransform.position, shotTransform.rotation) as Rigidbody;
			newBullet.AddForce(transform.forward * velocity,ForceMode.VelocityChange);
			RenderSettings.haloStrength = 0f;
			m_fXManager.SendMessage("CallPlayerMuzzleFlash", shotTransform.position);
		}
	}

}
