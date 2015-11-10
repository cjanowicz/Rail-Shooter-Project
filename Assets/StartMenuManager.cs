using UnityEngine;
using System.Collections;

public class StartMenuManager : MonoBehaviour {

	private Transform m_selectCube;
	public Transform[] m_uiObjects;
	public Transform m_startText;
	public Transform m_invertText;
	private Vector3 m_selectOffset;

	private int selectedUIElement = 0;

	// Use this for initialization
	void OnEnable () {
		m_selectCube = GameObject.Find ("SelectCube").transform;
		m_selectOffset = m_selectCube.position - m_startText.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") <= -0.1) {
			//Go Down

		}
		if (Input.GetAxisRaw ("Vertical") >= 0.1) {
		//Go up
		}
	}
}
