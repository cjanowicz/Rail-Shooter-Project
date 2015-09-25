using UnityEngine;
using System.Collections;

public class PlayerBulletScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("LifeTimer");
	}

	void OnTriggerEnter(Collider other)
	{

		if(other.tag != "Powerup" && other.tag != "Player" && other.tag != "EnemyBullet")
		{
			other.SendMessage("ApplyDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
			//Play Effects
			//Debug.Log("Player Bullet Hit: " + other.name);
			DestroySelf();
		}

	}

	IEnumerator LifeTimer() {
		yield return new WaitForSeconds(4.0f);
		DestroySelf();
	}

	void DestroySelf()
	{
		Destroy(this.gameObject);
	}
}
