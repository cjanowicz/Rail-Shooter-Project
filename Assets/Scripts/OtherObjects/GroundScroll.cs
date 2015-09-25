using UnityEngine;
using System.Collections;

public class GroundScroll : MonoBehaviour {

	public static float groundSpeed = 25;

	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(0,0, -groundSpeed * Time.deltaTime);
	}
}
