using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localPosition.z >= 200 || transform.localPosition.y >= 200)
		{
			DestroySelf();
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Powerup" && other.tag != "Enemy" && other.tag != "Bullet")
		{
			other.SendMessage("ApplyDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
			//Play Effects
			DestroySelf();
		}
		
	}
	
	void DestroySelf()
	{
		Destroy(this.gameObject);
	}
}
