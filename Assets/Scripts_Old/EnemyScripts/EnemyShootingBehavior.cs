using UnityEngine;
using System.Collections;

public class EnemyShootingBehavior : MonoBehaviour {
	
	public Rigidbody bullet;
	public float velocity = 40.0f;
	public Transform target;
	public Transform rootTransform;
	
	// Update is called once per frame
	void FireOnTarget(Transform newTarget)
	{
		Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
		newBullet.transform.parent = rootTransform;
		newBullet.AddForce(newBullet.transform.forward * velocity,ForceMode.VelocityChange);
	}
}
