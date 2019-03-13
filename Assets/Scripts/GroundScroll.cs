using UnityEngine;

/// <summary>
/// This behavior is meant to do two different things, depending on what kind of object it is.
/// If it is the GroundPlane object, it increments the UV offset on its texture.
/// For all others, it applies partial or full z and x ground velocities.
/// All game objects affected by the ground scrolling effect share this script
/// so that changes to the speed affect all objects sharing the static variables in this script.
/// Methods to call:
///     SetXSpeed(float newXSpeed): Called by the PlayerAimMovement, to set what the new X axis velocity is.
///     float GetXSpeed(): Returns the current X velocity, used by WindAngleScript and PlayerAimScript.
/// </summary>

public class GroundScroll : MonoBehaviour {

    public LevelManager levelManagerScript;

    [Range(0.0f, 1.0f)]
    private static float groundScale;

    private delegate void MyDelegate();

    private MyDelegate myDelegate;

    private Renderer objectRenderer;
    private float timer = 0;
    private float xDistance = 0;
    private float zDistance = 0;

    private float scrollOnZ = 1;
    private float multiplierX = 3;

    private void Start() {

        levelManagerScript = GameObject.Find("LevelManager").GetComponent<LevelManager>();


        /// On start, we determine what kind of object this component is attached to,
        /// and depending on that, we set a delegate function to do different functions.
        if (transform.name == "GroundPlane") {
            /// If we are the ground plane, we make the delegate update the texture and set our references.
            myDelegate = MoveTexture;
            objectRenderer = GetComponent<Renderer>();
            groundScale = this.transform.localScale.z / objectRenderer.material.GetTextureScale("_MainTex").y * 10f;
        } else {
            ///Otherwise, we make the delegate move the position of the object.
            myDelegate = MoveSelf;
            if (this.gameObject.tag.Contains("Bullet") || this.gameObject.tag.Contains("Enemy") || this.gameObject.tag.Contains("Boss") || this.gameObject.tag.Contains("Player")) {
                /// And for some object types such as bullets and enemy characters, they are unaffected by the Z speed
                scrollOnZ = 0;
                if (tag == "Player") {
                    /// While the player is not affected by the X speed to the same degree as other objects.
                    multiplierX = 0.5f;
                    multiplierX = 1.0f;
                }
            }
        }
    }

    private void Update() {
        /// Call whatever function the delegate has been assigned.
        myDelegate();
    }

    private void MoveSelf() {
        /// If moving ourself, we change the object position.
        transform.position += new Vector3(
            -levelManagerScript.xSpeed * groundScale * Time.deltaTime * multiplierX, 0,
            -levelManagerScript.zSpeed * groundScale * Time.deltaTime * scrollOnZ);

        /// If we are significantly behind the camera and we are a background object, 
        /// we then move into the distance and come towards the camera again.
        if (transform.position.z <= -30 && tag == "World")
            transform.position = new Vector3(Random.Range(-500, 500), 0, Random.Range(1200, 2000));
    }

    private void MoveTexture() {
        /// If we are the ground plane, we increment the X and Z distance variables,
        /// and use modulo to make them fit between 0 and 1, then we set the material texture offset.
        timer = (Time.deltaTime + timer) % 1;
        zDistance = timer * levelManagerScript.zSpeed;
        xDistance += levelManagerScript.xSpeed * multiplierX * Time.deltaTime;
        xDistance = xDistance % 1;
        objectRenderer.material.SetTextureOffset("_MainTex", new Vector2(xDistance, zDistance));
    }

    public void SetXSpeed(float newXSpeed) {
        /// Set the X speed.
        levelManagerScript.xSpeed = newXSpeed;
    }

    public float GetXSpeed() {
        /// Return the X Speed.
        return levelManagerScript.xSpeed;
    }
}