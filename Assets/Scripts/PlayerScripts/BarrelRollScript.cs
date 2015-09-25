using UnityEngine;
using System.Collections;

public class BarrelRollScript : MonoBehaviour {
	
	private float bankAxis;

	public float maxDegreesDelta = 120.0f;

	private float time = float.MaxValue;
	private bool buttonDown = false;
	public float doubleTapDelay = 0.2f;
	private bool inBarrelRoll = false;
	
	public float barrelRollDuration = 0.2f;
	
	// Update is called once per frame
	void Update () {
		if(!inBarrelRoll)
		{
			bankAxis = Input.GetAxis("Bank");
			//////////---------- ROTATE THE SHIP: BARREL ROLLING CODE 
			//In Z, if you rotate to the right (clockwise) its negative

			Quaternion newRotationEuler = transform.localRotation;
			newRotationEuler.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x,
			                                           transform.localRotation.eulerAngles.y,
			                                           90 * Input.GetAxis("Bank"));
			transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotationEuler, Time.deltaTime*10);

			/*

			Vector3 newRotationEuler = transform.rotation.eulerAngles;
			newRotationEuler.z = 90.0f * bankAxis;
			Quaternion newQuat = new Quaternion(0,0,0,0);
			newQuat.eulerAngles = newRotationEuler;
			transform.rotation = newQuat;
			*/

			////Next Attempt (Trying to make the rotation slower.
			//Quaternion.RotateTowards(transform.rotation, newQuat, Mathf.Deg2Rad* maxDegreesDelta * Time.deltaTime);
			////Note to self, try to make it more binary

			//We want a timer that increments if you hit the button axis. 
			//Timer starts/
			//it resets if you go hit the button again, and does a barrel roll if you do it under the time limit
			//if the timer goes over, it resets.

			if(bankAxis == 0.0f)
			{
				buttonDown = false;
			}
			else if(buttonDown == false)
			{
				buttonDown = true;
				if(time < doubleTapDelay)
				{
					if(bankAxis> 0.0f)
						StartCoroutine("BarrelRollLeft");
					else if(bankAxis< 0.0f)
						StartCoroutine("BarrelRollRight");
				}
				time = 0.0f;
			}
			time += Time.deltaTime;
		}
	}

	IEnumerator BarrelRollLeft()
	{
		this.transform.parent.SendMessage("BarrelRolling");
		inBarrelRoll = true;
		float t = 0.0f;

		float initialZ = transform.localRotation.eulerAngles.z;
		float goalZ = initialZ;
		goalZ += 180.0f;

		float currentZ = initialZ;

		while (t<barrelRollDuration/2.0f)
		{
			currentZ = Mathf.Lerp(initialZ, goalZ, t/(barrelRollDuration/2.0f));
			transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 
			                                                       transform.localRotation.eulerAngles.y,
			                                                       currentZ));
			t+= Time.deltaTime;
			yield return null;
		}

		t=0;

		initialZ = transform.localRotation.eulerAngles.z;
		goalZ = initialZ;
		goalZ += 180.0f;

		while(t< barrelRollDuration/2.0f)
		{
			currentZ = Mathf.Lerp(initialZ, goalZ, t/(barrelRollDuration/2.0f));
				transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 
				                                                       transform.localRotation.eulerAngles.y,
				                                                       currentZ));
			t += Time.deltaTime;
			yield return null;
		}
		inBarrelRoll = false;
		//Note: could try to use transform.rotateAround(Vector3.zero, Vector3.forward, 20* Time.deltatime);
		this.transform.parent.SendMessage("BarrelRollingEnd");
	}
	IEnumerator BarrelRollRight()
	{
		this.transform.parent.SendMessage("BarrelRolling");
		inBarrelRoll = true;
		float t = 0.0f;
		
		float initialZ = transform.rotation.eulerAngles.z;
		float goalZ = initialZ;
		goalZ -= 180.0f;
		
		float currentZ = initialZ;
		
		while (t<barrelRollDuration/2.0f)
		{
			currentZ = Mathf.Lerp(initialZ, goalZ, t/(barrelRollDuration/2.0f));
			transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 
			                                                       transform.localRotation.eulerAngles.y,
			                                                       currentZ));
			t+= Time.deltaTime;
			yield return null;
		}
		
		t=0;
		
		initialZ = transform.localRotation.eulerAngles.z;
		goalZ = initialZ;
		goalZ -= 180.0f;
		
		while(t< barrelRollDuration/2.0f)
		{
			currentZ = Mathf.Lerp(initialZ, goalZ, t/(barrelRollDuration/2.0f));
				transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 
				                                                       transform.localRotation.eulerAngles.y,
				                                                       currentZ));
			t += Time.deltaTime;
			yield return null;
		}
		inBarrelRoll = false;
		//Note: could try to use transform.rotateAround(Vector3.zero, Vector3.forward, 20* Time.deltatime);		
		this.transform.parent.SendMessage("BarrelRollingEnd");
	}
}
