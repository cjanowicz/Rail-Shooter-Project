using UnityEngine;
using System.Collections;

public class ParticleGroundScroll : MonoBehaviour {

	public GroundScroll m_grndScrollRef;
	public float m_rotationMult = 2;
	
	// Update is called once per frame
	void Update () {
		transform.localRotation = Quaternion.AngleAxis(m_grndScrollRef.GetXSpeed () * m_rotationMult, Vector3.up);
	}
}
