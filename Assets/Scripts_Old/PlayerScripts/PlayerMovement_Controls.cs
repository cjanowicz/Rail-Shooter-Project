using UnityEngine;
using System.Collections;

public class PlayerMovement_Controls : MonoBehaviour {

	//---=== GameLogic Variables ===---//
	private GameObject myGameManager;
	private GroundScroll groundPlaneScript;

	//---=== Movement Variables ===---//
	private float horizontal;
	private float vertical;

	public float movementSpeed = 8.0f;
	public float maxDegreesDelta = 120.0f;
	public float maxHorizontal = 8.5f;
	public float maxVertical = 5.5f;
	private float maxAdjust = 0.05f;

	//Movement interpolation Variables; (NOTE: MAKE SURE TO CHANGE EDITOR VARIABLES TO CODE)

	//The input variable
	Vector3 steerInput = new Vector3();
	//The Vector of where the ship was facing last frame
	Vector3 steerStored = new Vector3();
	//The current variable of where the ship is facing.
	Vector3 steerCurrent = new Vector3();
	//The clamp for the difference between the 
	public float steerMaxLimiter = 0.5f;
	//The t value fed into the lerp function (percent every frame)
	public float steerT = 0.1f;
	//How far into the z axis the current steering is placed;
	public float steerZOffset = 2.0f;

	void Awake(){
		myGameManager = GameObject.Find ("GameManager");
		groundPlaneScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll> ();
	}

	// Use this for initialization
	void Start () {
	
		steerStored = new Vector3(0,0,0);
		steerCurrent.Set(0,0,steerZOffset);
	}

	// Update is called once per frame
	void Update () {
		//---=== New Code ===---//
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
	
		steerInput = new Vector3(horizontal, -vertical, 0);
		Vector3 deltaInput = steerInput - steerStored;
		deltaInput = Vector3.ClampMagnitude(deltaInput, steerMaxLimiter);

		steerStored = Vector3.Lerp( steerStored, steerStored + deltaInput, steerT);
		steerCurrent.Set(steerStored.x, steerStored.y, steerZOffset);

		transform.localPosition+= steerStored * movementSpeed * Time.deltaTime;

		transform.localRotation = Quaternion.RotateTowards(
			transform.localRotation, Quaternion.LookRotation(steerCurrent), 
			Mathf.Deg2Rad* maxDegreesDelta);

		//////---===== Banking ======------
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
		                                         transform.localEulerAngles.y,
		                                         steerCurrent.x * -30);

		groundPlaneScript.SetXSpeed(horizontal *4f);

	
	///////----------============== POSITION LIMITING =================------------------
		if(transform.localPosition.x < -maxHorizontal)
		{
			transform.localPosition = new Vector3
				(-maxHorizontal+maxAdjust,transform.localPosition.y, transform.localPosition.z);
		}
		if(transform.localPosition.x > maxHorizontal)
		{
			transform.localPosition = new Vector3
				(maxHorizontal-maxAdjust,transform.localPosition.y, transform.localPosition.z);
		}
		if(transform.localPosition.y < -maxVertical)
		{
			transform.localPosition = new Vector3
				(transform.localPosition.x,-maxVertical+maxAdjust, transform.localPosition.z);
		}
		if(transform.localPosition.y > maxVertical)
		{
			transform.localPosition = new Vector3
				(transform.localPosition.x,maxVertical-maxAdjust, transform.localPosition.z);
		}


		
	}

	

	void BankingRight()
	{

	}
	void BankingRightEnd()
	{

	}
	void BarrelRolling()
	{
		movementSpeed *= 2.0f;
	}
	void BarrelRollingEnd()
	{
		movementSpeed /= 2.0f;
	}
}
