using UnityEngine;
using System.Collections;

public class GroundScroll : MonoBehaviour {

	public static float m_xSpeed = 0;
	public static float m_zSpeed = 10;
    [Range(0.0f, 1.0f)]

	public static float m_groundScale;

    delegate void MyDelegate();
    MyDelegate myDelegate;

	private Renderer m_objectRenderer;
	private float m_timer = 0;
	private float m_xDistance = 0;
	private float m_zDistance = 0;

	private float m_scrollOnZ = 1;
	private float m_multiplierX = 3;

    void Start() {
		if(transform.name == "GroundPlane"){
			myDelegate = MoveTexture;
			m_objectRenderer = GetComponent<Renderer>();
			m_groundScale = this.transform.localScale.z / m_objectRenderer.material.GetTextureScale("_MainTex").y * 10f; 
		} else {
            myDelegate = MoveSelf;
			if(this.gameObject.tag.Contains("Bullet") || this.gameObject.tag.Contains("Enemy") || this.gameObject.tag.Contains("Boss") || this.gameObject.tag.Contains("Player")) {
                m_scrollOnZ = 0;
				if(tag == "Player"){
					m_multiplierX = 1f;
				}
			}
        }
    }

	// Update is called once per frame
	void Update () {
        myDelegate();
	}

    void MoveSelf() {
		transform.position += new Vector3(
			-m_xSpeed * m_groundScale * Time.deltaTime * m_multiplierX, 0, 
			-m_zSpeed * m_groundScale * Time.deltaTime * m_scrollOnZ);

        if (transform.position.z <= -30 && tag == "World") 
            transform.position = new Vector3(Random.Range(-500, 500), 0, Random.Range(1200, 2000));
       
    }

	void MoveTexture(){
		m_timer = (Time.deltaTime + m_timer)%1;
		m_zDistance = m_timer * m_zSpeed;
		m_xDistance += m_xSpeed * m_multiplierX * Time.deltaTime;
		m_xDistance = m_xDistance % 1;
		m_objectRenderer.material.SetTextureOffset ("_MainTex", new Vector2 (m_xDistance, m_zDistance));
	}

	public void SetXSpeed(float newXSpeed){
		m_xSpeed = newXSpeed;
	}

	public float GetXSpeed(){
		return m_xSpeed;
	}
}
