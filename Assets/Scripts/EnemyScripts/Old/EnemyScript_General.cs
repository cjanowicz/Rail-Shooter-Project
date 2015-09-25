using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]

public class EnemyScript_General : MonoBehaviour {

	//---===Variables===---//

	public string state;
		//States = Wait NO I should enum this!!! ugh.
			//Ok: Normal (playing an animation and shooting and shit)
			//Steering: Following a behavior


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//---===CALLED FUNCTIONS===---//

	//Call this when the ship gets Shot
	void ApplyDamage(int damage)
	{
		//Shake, check if health is depleted. If so, call Die.
	}

	//Call this when health <= 0
	void Die()
	{
		//Turn on gravity, 
		//start spewing the wreckage/fire particles
		//Apply a twisting rotation
		//angle down over time

		//state= dying

	}

	void Explode()
	{
		//Play the explosion effect and spew the last wreckage.
		//state = Inactive.
	}

	//Shooting at the player if non-specified
	void FireOnTarget()
	{
		//Shoot at the player
	}

	
	//Shooting at a specified target
	void FireOnTarget(Transform newTarget)
	{
		//Shoot at the player
	}

	//Call this when I want to re-use the ship.
	void Revive()
	{

	}


}
