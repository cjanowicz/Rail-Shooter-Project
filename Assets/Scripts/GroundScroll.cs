using UnityEngine;

public class GroundScroll : MonoBehaviour {
    public static float xSpeed = 0;
    public static float zSpeed = 10;

    [Range(0.0f, 1.0f)]
    public static float groundScale;

    private delegate void MyDelegate();

    private MyDelegate myDelegate;

    private Renderer objectRenderer;
    private float timer = 0;
    private float xDistance = 0;
    private float zDistance = 0;

    private float scrollOnZ = 1;
    private float multiplierX = 3;

    private void Start() {
        if (transform.name == "GroundPlane") {
            myDelegate = MoveTexture;
            objectRenderer = GetComponent<Renderer>();
            groundScale = this.transform.localScale.z / objectRenderer.material.GetTextureScale("_MainTex").y * 10f;
        } else {
            myDelegate = MoveSelf;
            if (this.gameObject.tag.Contains("Bullet") || this.gameObject.tag.Contains("Enemy") || this.gameObject.tag.Contains("Boss") || this.gameObject.tag.Contains("Player")) {
                scrollOnZ = 0;
                if (tag == "Player") {
                    multiplierX = 0.5f;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        myDelegate();
    }

    private void MoveSelf() {
        transform.position += new Vector3(
            -xSpeed * groundScale * Time.deltaTime * multiplierX, 0,
            -zSpeed * groundScale * Time.deltaTime * scrollOnZ);

        if (transform.position.z <= -30 && tag == "World")
            transform.position = new Vector3(Random.Range(-500, 500), 0, Random.Range(1200, 2000));
    }

    private void MoveTexture() {
        timer = (Time.deltaTime + timer) % 1;
        zDistance = timer * zSpeed;
        xDistance += xSpeed * multiplierX * Time.deltaTime;
        xDistance = xDistance % 1;
        objectRenderer.material.SetTextureOffset("_MainTex", new Vector2(xDistance, zDistance));
    }

    public void SetXSpeed(float newXSpeed) {
        xSpeed = newXSpeed;
    }

    public float GetXSpeed() {
        return xSpeed;
    }
}