using UnityEngine;
using System.Collections;

public class ScrollGridScript : MonoBehaviour {

	public Renderer m_objectRenderer;
	private float m_timer = 0;
    [HideInInspector]
	public float xDistance = 0;
	public float zSpeed = 10;
	public float xSpeed = 10;
	
	// Update is called once per frame
	void Update () {
		m_timer += Time.deltaTime * zSpeed;
		m_timer = m_timer%1;
		xDistance += xSpeed * Time.deltaTime;
		xDistance = xDistance % 1;
		m_objectRenderer.material.SetTextureOffset ("_MainTex", new Vector2 (xDistance, m_timer));
	}


}
