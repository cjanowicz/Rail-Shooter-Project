using UnityEngine;
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

	void Start () {
	
	}

	//NOTE: TODO: GOTTA REWRITE THIS GIVEN WHAT I NOW KNOW
	void Update () {
		
		//newPosition.Set(playerObject.position.x * dampenerX, playerObject.position.y * dampenerY, followDistance);
		newPosition.Set(0,0, followDistance);

		newRotation.x = playerObject.localEulerAngles.x;
		newRotation.y = playerObject.localEulerAngles.y;

		if(newRotation.x >180)
			newRotation.x -= 360;
		if(newRotation.y >180)
			newRotation.y -= 360;
		newRotation/=dampenerNewRotation;

		//ZInterpolation;
		zPlayerRotation = playerObject.rotation.eulerAngles.z;
		if(zPlayerRotation > 180)
			zPlayerRotation -= 360;
		zPlayerRotation *= zPlayerDampener;
		zInterpolation = Mathf.Lerp(zInterpolation, zPlayerRotation, zLerpT);

		storedRotation = new Vector3(Mathf.Lerp(storedRotation.x, newRotation.x, dampenerAngleX),
		                             Mathf.Lerp(storedRotation.y, newRotation.y, dampenerAngleY),
		                             zInterpolation );
		this.transform.eulerAngles = storedRotation;

		this.transform.localPosition += (newPosition - transform.localPosition)* dampenerPos * Time.deltaTime;

	}

	void OnDrawGizmos() {
		transform.position = new Vector3 (playerObject.position.x / dampenerX, playerObject.position.y / dampenerY, followDistance);
	}
}
