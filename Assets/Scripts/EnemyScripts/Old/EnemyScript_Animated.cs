using UnityEngine;
using System.Collections;

public class EnemyScript_Animated : MonoBehaviour {
	
	
	public Transform targetTransform;
	
	public Rigidbody bullet;
	public float velocity = 40.0f;	
	public Transform bulletOrigin;
	public Transform rootTransform;
	
	public int healthMax = 5;
	private int health;
	
	void Start()
	{
		health = healthMax;
	}
	
	void FireOnTarget()
	{
		bulletOrigin.LookAt(targetTransform.position);
		Rigidbody newBullet = Instantiate(bullet, bulletOrigin.position, bulletOrigin.rotation) as Rigidbody;
		newBullet.transform.parent = rootTransform;
		newBullet.AddForce(newBullet.transform.forward * velocity,ForceMode.VelocityChange);
	}
	
	void ApplyDamage(int damage)
	{
		health -= damage;
		if(health <= 0)
		{
			BroadcastMessage("Explode");
			//this.renderer.enabled = false;
			//this.collider.enabled = false;
			health = healthMax;
		}
	}
	void Revive()
	{
		this.GetComponent<Renderer>().enabled = false;
		this.GetComponent<Collider>().enabled = false;
		health = healthMax;
	}
	
	

}
