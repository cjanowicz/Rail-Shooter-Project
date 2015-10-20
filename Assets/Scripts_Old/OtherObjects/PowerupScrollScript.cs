using UnityEngine;
using System.Collections;

public class PowerupScrollScript : MonoBehaviour {

	public float groundSpeed = 25;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += new Vector3(0,0, -groundSpeed * Time.deltaTime);
		if(transform.localPosition.z <= -10)
		{
			transform.localPosition = new Vector3(Random.Range(-5,5),Random.Range(-4,5), 200 + Random.Range(-30,30));
			this.GetComponent<Renderer>().enabled = true;
			this.GetComponent<Collider>().enabled = true;
		}
		transform.Rotate( new Vector3(0,5,0), Space.Self);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{		
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
		}
	}
}
