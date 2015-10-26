using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    public float m_axisDamping = 0.25f;
    public float m_limitX = 6f;
    public float m_targetX = 3f;
    private bool m_goingToTarget = false;
    private float m_currentTargetX; 

    public GroundScroll m_groundScroll;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(float h, float v) {
        transform.position += new Vector3(h, v) * m_axisDamping;

        if(Mathf.Abs(transform.position.x) > m_limitX) {
            //if(m_goingToTarget = )
            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            m_groundScroll.SetXSpeed(Mathf.Abs(transform.position.x) - m_limitX * clampedX);
            transform.position = Vector3.Lerp(transform.position, new Vector3( clampedX * m_limitX, 
                transform.position.y, transform.position.z), 0.05f);

        }
        else
            m_groundScroll.SetXSpeed(0);
    }
}
