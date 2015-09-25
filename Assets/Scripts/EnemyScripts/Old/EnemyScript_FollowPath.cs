using UnityEngine;
using System.Collections;

public class EnemyScript_FollowPath : MonoBehaviour {

	private int state = 0;
	public Transform rootTransform;
	public GameObject shipModel;

	//Moving
	private float timer = 0.0f;
	private float movingTimeMax = 5.0f;
	
	//Shooting
	public float shootTimerMax = 1.0f;
	public Transform enemyTransform;
	public Rigidbody bullet;
	public float velocity = 40.0f;	
	public Transform bulletOrigin;
	private bool canShoot = false;

	//Health
	public int healthMax = 5;
	private int health;

	private float forceFloat = 500.0f;
	
	void Start()
	{
		health = healthMax;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(state == 0)
		{
			this.transform.localPosition = new Vector3(
				transform.localPosition.x,
				2.0f * Mathf.Pow(timer-5,2),
				2.0f * Mathf.Pow(timer-5,2) + 15);
			
			if(timer >= movingTimeMax)
			{
				state =1;
				timer = 0;
			}
		}
		else if(state == 1){
			if(canShoot == true)
			{
				FireOnTarget(enemyTransform);
				canShoot = false;
			}
			else{
				if(timer > shootTimerMax)
				{
					timer = 0.0f;
					canShoot = true;
				}
			}
		}
		//Invisible
		else if(state == 3 || state == 2)
		{
			//Timer is over, ready to reset.

			if(timer >= 3.0f)
			{
				timer = 0;
				state = 0;
				MakeVisible();
			}
		}
		
		timer+= Time.deltaTime;
		
	}
	
	void FireOnTarget(Transform target)
	{
		bulletOrigin.LookAt(target.position);
		Rigidbody newBullet = Instantiate(bullet, bulletOrigin.position, bulletOrigin.rotation) as Rigidbody;
		newBullet.transform.parent = rootTransform;
		newBullet.AddForce(newBullet.transform.forward * velocity,ForceMode.VelocityChange);
	}
	
	void ApplyDamage(int damage)
	{
		health -= damage;

		//if the enemy is destroyed, I want it to explode, then spew smoke and tumble out of the sky. 
		//Then when it hits another object, I want it to explode. 
		if(health <= 0)
		{
			if(state == 1)
			{
				state = 2;
				timer = 0.0f;
				health = healthMax;
				DeathSequence();
			}
			else if(state == 2)
			{
				MakeInvisible();
			}

		}
	}
	//have it tumble through the sky, remove constraints.
	void DeathSequence()
	{
		state = 2;
		BroadcastMessage("Explode");
		timer = 0;

		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		
		GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-forceFloat,forceFloat), 
		                                       Random.Range(0,forceFloat), 
		                                       Random.Range(-forceFloat,forceFloat)));
		
		GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-forceFloat,forceFloat), 
		                                        Random.Range(-forceFloat,forceFloat), 
		                                        Random.Range(-forceFloat,forceFloat)));
	}
	//Make it invisible, disable collision box.
	void MakeInvisible()
	{
		state = 3;
		BroadcastMessage("Explode");				
		shipModel.GetComponent<Renderer>().enabled = false;
		timer = 0;

		//this.attachedRigidbody.useGravity = true;
	}
	//Re-enable collision box, put constraints back on.
	void MakeVisible()
	{
		shipModel.GetComponent<Renderer>().enabled = true;
		
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		transform.rotation = new Quaternion(0,0,0,0);
		
		transform.position= Vector3.zero;
	}

	//Add function here that is OnTriggerEnter: when something hits them they explode, This would be after the
	//guy is dead and his trigger is set to a trigger.
	//make sure to reset that.
}


/*Ok, gotta figure this shit out. he's got like 4 states: Moving in, shooting, dead and invisible.
Ok, lets use an int for his FSM (finite state machine.)
0 is moving, 1 is shooting, 2 is dead. Fuck this multiple boolean crap.

*/

