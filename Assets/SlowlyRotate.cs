using UnityEngine;
using System.Collections;

public class SlowlyRotate : MonoBehaviour {

	public Vector3 m_axis;
	public float m_defaultSpeed = 1f;
	private float m_speed;
	public float m_speedLerpSpeed = 10f;


	void Start () {
		m_speed = m_defaultSpeed;
		if (m_axis.magnitude == 0) {
			m_axis = new Vector3(0,0,1);
		}
	}
	void Update () {
		transform.Rotate (m_axis * Time.deltaTime * m_speed);
		m_speed = Mathf.Lerp (m_speed, m_defaultSpeed, 
		                     Mathf.Clamp01 (Time.deltaTime * m_speedLerpSpeed));

	}

	public void SetSpeed(float newSpeed){
		m_speed = newSpeed;
	}

	public void ResetSpeed(){
		m_speed = m_defaultSpeed;
	}
}
