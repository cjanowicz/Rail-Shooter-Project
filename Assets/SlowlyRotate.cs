using UnityEngine;
using System.Collections;

public class SlowlyRotate : MonoBehaviour {

	public Vector3 m_axis;
	public float m_speed = 1f;

	void Start () {
		if (m_axis.magnitude == 0) {
			m_axis = new Vector3(0,0,1);
		}
	}
	void Update () {
		transform.Rotate (m_axis * Time.deltaTime * m_speed);
	}
}
