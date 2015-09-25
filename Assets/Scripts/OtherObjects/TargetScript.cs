using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {
	public float groundSpeed = 25;
	int health = 1;
	public int maxHealth = 1;
	
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3(Random.Range(-5,5),Random.Range(-4,5), this.transform.localPosition.z + Random.Range(-10,30));
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += new Vector3(0,0, -groundSpeed * Time.deltaTime);
		if(transform.localPosition.z <= -10)
		{
			transform.localPosition = new Vector3(Random.Range(-5,5),Random.Range(-4,5), 200 + Random.Range(-30,30));
			this.GetComponent<Renderer>().enabled = true;
			this.GetComponent<Collider>().enabled = true;
			health = maxHealth;
		}
		transform.Rotate( new Vector3(-5,0,-5), Space.Self);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.SendMessage("ApplyDamage", 2, SendMessageOptions.DontRequireReceiver);
		}
	}
	//First shake and take damage, then explode.
	void ApplyDamage(int amount)
	{
		health -= amount;
		if(health <= 0)
		{
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
			BroadcastMessage("Explode");
		}
	}
}