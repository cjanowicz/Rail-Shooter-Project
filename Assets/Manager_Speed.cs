using UnityEngine;
using System.Collections;

public class Manager_Speed : MonoBehaviour {

	public float cameraSpeed = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += new Vector3(0,0,cameraSpeed * Time.deltaTime);
	}
}
