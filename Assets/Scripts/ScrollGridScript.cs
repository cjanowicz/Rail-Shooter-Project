using UnityEngine;
using System.Collections;

public class ScrollGridScript : MonoBehaviour {

	public Renderer objectRenderer;
	private float timer = 0;
	public float speed = 1;
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime * speed;
		timer = timer%1;
		objectRenderer.material.SetTextureOffset ("_MainTex", new Vector2 (0, timer));
	}
}
