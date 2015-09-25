using UnityEngine;
using System.Collections;

public class PlayerMovement_Mouse : MonoBehaviour {


	private Vector2 mInput = new Vector2(0,0);
	private Vector2 mTurn = new Vector2(0,0);

	private bool barrel = false;

	public float responsiveness = 5;
	public float turnSensitivity = 5;
	public float rotationSpeed = 5;
	public Vector2 movementDistance = new Vector2(0,0);
	public Transform mTrans;
	public float screenArea;
	public Vector3 turnDegrees = new Vector3(0,0,0);

	void Start()
	{

	}

	// Update is called once per frame
	void Update () {
		NewMovementAndRot();
	}
	
	IEnumerator DoABarrelRoll (float duration)
	{
		barrel = true;
		yield return new WaitForSeconds (duration);
		barrel = false;
	}
	
	
	void NewMovementAndRot ()
	{
		
		Vector3 pos = Input.mousePosition;
		
		float x = -Mathf.Clamp ((Screen.width * 0.5f - pos.x) / (Screen.width * screenArea), -1f, 1f);
		float y = -Mathf.Clamp ((Screen.height * 0.5f - pos.y) / (Screen.height * screenArea), -1f, 1f);
		
		Vector2 vec = new Vector2 (x, y);
		float mag = vec.magnitude;
		if (mag > 1f)
			vec *= 1.0f / mag;
		
		mInput = Vector2.Lerp (mInput, vec, Mathf.Clamp01 (Time.deltaTime * responsiveness));
		mTurn = Vector2.Lerp (mTurn, vec - mInput, Mathf.Clamp01 (Time.deltaTime * turnSensitivity));
		
		mTrans.localPosition = new Vector3 (mInput.x * movementDistance.x, mInput.y * movementDistance.y, 10);
		if (!barrel) {
			mTrans.localRotation = Quaternion.Euler (-mTurn.y * turnDegrees.x, mTurn.x * turnDegrees.y, -mTurn.x * turnDegrees.z);
		} else
			transform.Rotate (transform.forward, 20 * Time.deltaTime * rotationSpeed * 20f);         
	}
}
