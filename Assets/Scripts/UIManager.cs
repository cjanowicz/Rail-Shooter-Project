using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public class Transition{
		float m_refValue;
		float m_duration;
		float m_originValue;
		float m_targetValue;
		float m_refSpeed;

		Transition(ref float value){
			m_refValue = value;
			SetDefaults();
		}
		Transition(ref float value, float duration, float target){
			m_refValue = value;
			m_duration = duration;
			m_targetValue = target;
		}

		void SetDefaults(){
			m_duration = 2f;
			m_originValue = m_refValue;
		}
	}
	public Transition[] transitionArray;
	const int transitionArraySize

	public GameObject m_fadeOutPlane;
	private float m_fadeOutAlpha;



	// Use this for initialization
	void Start () {
		transitionArray = new Transition[3];
	}
	
	// Update is called once per frame
	void Update () {

	}

	void UpdateTransitionF(ref float value, float target, ref float speed, float duration){
		value = Mathf.SmoothDamp(value, target, ref speed, duration);
	}

	public void StartFadeIn(){

	}
}
