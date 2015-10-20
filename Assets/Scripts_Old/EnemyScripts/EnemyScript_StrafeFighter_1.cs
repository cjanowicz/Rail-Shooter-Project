using UnityEngine;
using System.Collections;

/* Alright, Here's what I need for this guy.
 * 
 * ....
 * 
 * Ok. 
 * 
 * Needs to follow an animation path. 
 * Needs to shoot the player. (on animation command is easy
 * Needs to track his health and then explode in interesting ways, 
 * throw sweet wreckage all over the place.
 * 
 * 
 * Also he should have a variable stating whether he is free to be re-set into a new formation or not.
 * 
 * ... Maybe he should be able to swap out his mesh,  and act differently to be different enemies.
 * 
 * 
 */

public class EnemyScript_StrafeFighter_1 : MonoBehaviour {

	public Transform targetTransform;
	
	public Rigidbody bullet;
	public float bullerSpeed = 40.0f;	
	public Transform bulletOrigin;
	public Transform rootTransform;
	
	public int healthMax = 5;
	private int health;



	// Use this for initialization
	void Start() 
	{
		health = healthMax;
		GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}
	
	void ApplyDamage(int damage)
	{
		health -= damage;
		if(health <= 0)
		{
			Destroy();
			//Start killing self,
			BroadcastMessage("Explode");
			//this.renderer.enabled = false;
			//this.collider.enabled = false;
			health = healthMax;

		}
	}

	void Destroy()
	{
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Animation>().Stop();
		//Turn off animation for now
		//Explode
		//make the collider into a trigger collider,
		//If it hits something, explode
		//start emitting wreckage particles

	}
}
