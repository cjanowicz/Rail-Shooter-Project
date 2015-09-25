﻿using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

	public Transform playerObject;
	private Vector3 newPosition = new Vector3();
	public float followDistance = 5f;
	private Vector3 newRotation = new Vector3();
	public float rotationMultiplier = 10.0f;
	private Vector3 storedRotation = new Vector3();

	public float dampenerX = 1.5f;
	public float dampenerY = 2.0f;
	public float dampenerNewRotation = 3.0f;
	public float dampenerAngleX = 0.1f;
	public float dampenerAngleY = 0.1f;
	public float dampenerPos = 0.1f;

	private float zInterpolation;
	public float zLerpT = 0.1f;
	private float zPlayerRotation;
	private float zPlayerDampener = 0.3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		newPosition.Set(playerObject.position.x/dampenerX, playerObject.position.y/dampenerY, followDistance);

		//(xRotation, yRotation, zRotation)
		//(yDistance, xDistance,none)

		//Note to self: make this reliant on delta time so that it does not jump if a frame drops.

		/*old and broken, even with delta time it does not work. */
		/*
	newRotation.Set(transform.localPosition.y - newPosition.y,
		                newPosition.x - transform.localPosition.x,
		                transform.localPosition.x - newPosition.x);
		newRotation *= rotationMultiplier * Time.deltaTime;
		this.transform.eulerAngles = newRotation;
		*/
		//Ok, I have the player transform. The camera angle

		/*
		this.transform.eulerAngles = playerObject.eulerAngles/4;
		Debug.Log("rotation = " + this.transform.eulerAngles);
		//This does not work because 355 degrees/4 = 90;
		*/
		//
		newRotation.x = playerObject.localEulerAngles.x;
		newRotation.y = playerObject.localEulerAngles.y;

		if(newRotation.x >180)
			newRotation.x -= 360;
		if(newRotation.y >180)
			newRotation.y -= 360;
		newRotation/=dampenerNewRotation;

		//Solve this by having another vector store what the angle would be in negative numbrs
		//because every frame, unity switches the local euler angles back to postives, which makes my
		//lerp function try to interpolate between 355 and -5 every frame.

		//this.transform.eulerAngles = newRotation;

		//ZInterpolation;
		zPlayerRotation = playerObject.rotation.eulerAngles.z;
		if(zPlayerRotation > 180)
			zPlayerRotation -= 360;
		zPlayerRotation *= zPlayerDampener;
		zInterpolation = Mathf.Lerp(zInterpolation, zPlayerRotation, zLerpT);

		//The reason I must store the rotation I want into a vector is because
		//every frame, unity re-sets the euler angles to be in positive numbers only
		//making my conversion from positives to negatives counter-active against itself.
		storedRotation = new Vector3(Mathf.Lerp(storedRotation.x, newRotation.x, dampenerAngleX),
		                             Mathf.Lerp(storedRotation.y, newRotation.y, dampenerAngleY),
		                             zInterpolation );
		this.transform.eulerAngles = storedRotation;

		///this.transform.localPosition = newPosition;
		this.transform.localPosition = transform.localPosition + (newPosition - transform.localPosition)* dampenerPos;

		//This system itself is kinda broken and not what we want.
		//I want a system that will 


		//Level Management test:
		if(Input.GetKeyDown("h"))
		{
			int level = Application.loadedLevel +1;
			if(level >= Application.levelCount)
				level = 0;
			Physics.gravity = new Vector3(0,-9.81f,0);
			Application.LoadLevel(level);
		}		
		if(Input.GetKeyDown("k"))
		{
			Application.LoadLevel(Application.loadedLevel);
		}

	}
}
