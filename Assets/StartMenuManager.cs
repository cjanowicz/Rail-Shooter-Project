using UnityEngine;
using System.Collections;

public class StartMenuManager : MonoBehaviour {


	private Transform m_selectCube;
	private const int m_numUI = 2;
	public Transform[] m_UIObj = new Transform[m_numUI];
	private string[] m_UIMethod = new string[m_numUI];
	private Vector3 m_selectOffset;
	public float m_selectSpeed = 10;

	private int m_UIIter = 0;
	private bool m_inputPressed;

	private SlowlyRotate m_selectRotateScript;
	public float m_newRotateSpeed = 20f;

	// Use this for initialization
	void OnEnable () {
		m_selectCube = GameObject.Find ("SelectCube").transform;
		m_selectOffset = m_selectCube.position - m_UIObj[0].position;
		for (int i = 0; i < m_UIObj.Length; i++) {
			m_UIMethod[i] = m_UIObj[i].name;
		}
		m_selectRotateScript = m_selectCube.GetComponent<SlowlyRotate> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") <= -0.1) {
			//Go Down
			if(m_inputPressed == false){
				m_UIIter = (m_UIIter + 1) % m_UIObj.Length;
				m_inputPressed = true;
			}
		} else if (Input.GetAxisRaw ("Vertical") >= 0.1) {
			//Go up
			if(m_inputPressed == false){
				m_UIIter = m_UIIter - 1;
				if(m_UIIter < 0){
					m_UIIter = m_UIObj.Length-1;
				}
				m_inputPressed = true;
			}
		} else {
			m_inputPressed = false;
		}


		if(Input.GetButtonDown("Fire1")){
			SendMessage(m_UIMethod[m_UIIter]);
		}


		m_selectCube.position = Vector3.Lerp (m_selectCube.position,
                              m_UIObj [m_UIIter].position + m_selectOffset,
                   				Mathf.Clamp01 (Time.deltaTime * m_selectSpeed));
	}

	void StartText(){
		m_selectRotateScript.SetSpeed (m_newRotateSpeed);
		Invoke ("DelayLoadLevel", 1f);
	}
	void DelayLoadLevel(){
		Application.LoadLevel ("TestScene");
	}
	void InvertText(){
		m_selectRotateScript.SetSpeed (m_newRotateSpeed);
	}
}
