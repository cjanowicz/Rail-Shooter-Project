using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {


	public class TransitionF{
		float m_value;
		float m_duration;
		float m_originValue;
		float m_targetValue;
		float m_refSpeed;
		bool m_reversed;

		TransitionF(ref float value){
			SetDefaults();
			m_value = value;
		}
		TransitionF(ref float value, float duration, float target){
			SetDefaults();
			m_value = value;
			m_duration = duration;
			m_targetValue = target;
		}

		void SetDefaults(){
			m_duration = 2f;
			m_originValue = m_value;
		}

		void StartTransition(){

		}

		void RunTransition(){
			m_value = Mathf.SmoothDamp (m_value, m_targetValue, ref m_refSpeed, m_duration);
		}
	}
	public TransitionF[] transitionArray;
	const int transitionArraySize = 3;

	//Must be set in Editor:
	public GameObject m_whiteOutPlane;
	public float m_whiteOutChangeSpeed = 3;
	private Material m_whiteOutMat;
	private Vector4 m_clearWhite;
	private bool m_whiteOutVisible = true;
	private bool m_whiteOutChange = false;


	// Use this for initialization
	void Awake() {
		transitionArray = new TransitionF[3];
		m_clearWhite = new Vector4 (1, 1, 1, 0);

		m_whiteOutPlane.SetActive(m_whiteOutVisible);

		m_whiteOutMat = m_whiteOutPlane.GetComponent<Renderer> ().materials [0];
		m_whiteOutMat.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_whiteOutChange) {
			if(m_whiteOutVisible == false){
				m_whiteOutMat.color = (Color)Vector4.Lerp(m_whiteOutMat.color, Color.white, m_whiteOutChangeSpeed* Time.deltaTime);
				Debug.Log("Lerp for Fade In To White Called");
				//UpdateTransition((Vector4)m_whiteOutMat.color, m_clearWhite, m_whiteOutChangeSpeed);
			}else{
				m_whiteOutMat.color = (Color)Vector4.Lerp(m_whiteOutMat.color, m_clearWhite, m_whiteOutChangeSpeed* Time.deltaTime);
			}
		}
	}

	void UpdateTransition(ref float value, float target, ref float speed, float duration){
		value = Mathf.SmoothDamp(value, target, ref speed, duration);
	}
	void UpdateTransition(Vector4 value, Vector4 target, float duration){
		value = Vector4.Lerp (value, target, duration * 10f * Time.deltaTime);
	}

	public void StartFadeIn(){
		CancelInvoke ("EndFadeIn");
		CancelInvoke ("EndFadeOut");
		EndFadeOut ();
		m_whiteOutChange = true;
		m_whiteOutPlane.SetActive (true);
		Debug.Log ("Start Fade In Called");
		Invoke ("EndFadeIn", m_whiteOutChangeSpeed/2);
	}
	public void StartFadeOut(){
		CancelInvoke ("EndFadeIn");
		CancelInvoke ("EndFadeOut");
		EndFadeIn ();
		m_whiteOutChange = true;
		Invoke ("EndFadeOut", m_whiteOutChangeSpeed/2);

	}
	private void EndFadeIn(){
		m_whiteOutChange = false;
		m_whiteOutVisible = true;
	}
	private void EndFadeOut(){
		m_whiteOutChange = false;
		m_whiteOutPlane.SetActive (false);
		m_whiteOutVisible = false;
	}
}
