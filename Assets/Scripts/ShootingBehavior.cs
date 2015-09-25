using UnityEngine;
using System.Collections;

public class ShootingBehavior : MonoBehaviour {

	public Rigidbody bullet;
	public float velocity = 80.0f;
	public Transform rootTransform;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetButtonDown("Fire1"))
		{
			Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
			newBullet.transform.parent = rootTransform;
			newBullet.AddForce(transform.forward * velocity,ForceMode.VelocityChange);
			RenderSettings.haloStrength = 0f;
		}
	}

}
