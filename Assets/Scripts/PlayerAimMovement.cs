using UnityEngine;
using System.Collections;

public class PlayerAimMovement : MonoBehaviour {

	public Transform m_reticleFarTra;
	public Transform m_reticleCloseTra;
	[SerializeField]float m_reticleSpd = 3;
	private Vector3 m_aimPos;
	[SerializeField]float m_shipSpd = 3;
	[SerializeField]float m_shipRotateSpd = 100;

	[Range(0,1)]
	public float m_inputDamper = 0.5f;

	// Use this for initialization
	void Start () {
		m_aimPos = m_reticleFarTra.position;
	}
	
	// Update is called once per frame
	void Update () {
		m_reticleCloseTra.position = (m_reticleFarTra.position + transform.position) / 2;
	}

	public void Move(float h, float v) {
		m_aimPos += new Vector3(h, v, 0) * m_inputDamper;
		m_reticleFarTra.position = Vector3.Lerp(m_reticleFarTra.position, m_aimPos, 
		                                     Time.deltaTime * m_reticleSpd);
		this.transform.position = Vector3.Lerp(this.transform.position, 
                                   new Vector3(m_aimPos.x, m_aimPos.y, transform.position.z), 
                                   Time.deltaTime * m_shipSpd);
		/*this.transform.localRotation = Quaternion.RotateTowards(
				transform.localRotation, Quaternion.LookRotation(m_reticleFarTra.position),
				Mathf.Deg2Rad * Time.deltaTime);*/
		transform.LookAt (m_reticleFarTra.position);
	}
}
