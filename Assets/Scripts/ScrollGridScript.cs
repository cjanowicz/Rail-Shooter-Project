using UnityEngine;

public class ScrollGridScript : MonoBehaviour {
    public Renderer objectRenderer;
    private float timer = 0;

    [HideInInspector]
    public float xDistance = 0;

    public float zSpeed = 10;
    public float xSpeed = 10;

    // Update is called once per frame
    private void Update() {
        timer += Time.deltaTime * zSpeed;
        timer = timer % 1;
        xDistance += xSpeed * Time.deltaTime;
        xDistance = xDistance % 1;
        objectRenderer.material.SetTextureOffset("_MainTex", new Vector2(xDistance, timer));
    }
}