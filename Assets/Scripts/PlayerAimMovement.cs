using UnityEngine;

/// <summary>
/// Script that handles player movement. 
/// It receives messages from the playerInputManager, as well as the PlayerHealthScript.
/// It is called "Player Aim Movement" because it functions by moving the player's crosshair
/// and having the ship move so that it is behind the crosshair over time.
/// Methods to call:
///     Move(float horizontal, float vertical): Called by the PlayerInputScript, handles logic for the player movement.
/// </summary>

public class PlayerAimMovement : MonoBehaviour {

    private GroundScroll groundScript;

    [SerializeField]
    private float grndSpdMult = 0.5f;
    private AppManager appManagerScript;
    private int invert;
    public Transform reticleFarTra;
    public Transform reticleCloseTra;
    [SerializeField]
    private float inputSpd = 50;
    [SerializeField]
    private float reticleSpd = 50;
    private Vector3 aimPos;
    [SerializeField]
    private float shipSpd = 3; 
    [SerializeField]
    private float maxReticleDiff = 10;
    [SerializeField]
    private float limitX = 5;
    [SerializeField]
    private float limitY = 5;
    [SerializeField]
    private float reticleYLimit = -4;
    [SerializeField]
    private float bankingZStr = 10;

    private void Start() {
        /// Set up references.
        appManagerScript = GameObject.Find("AppManager(Clone)").GetComponent<AppManager>();
        groundScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll>();
        /// First it gets the inverted player settings from the appManager.
        invert = appManagerScript.GetInvert();
        /// Then it sets a vector variable to the reticle transform.
        aimPos = reticleFarTra.position;
    }

    private void Update() {
        /// On every update, we set the position of the middle reticle element 
        /// to exactly the middle between the player and the far reticle element.
        reticleCloseTra.position = (reticleFarTra.position + transform.position) / 2;
    }

    public void Move(float h, float v) {
        /// here we accept player input and put it into a variable, and we dampen the input based on delta time and a local variable.
        aimPos += new Vector3(h, v * invert, 0) * inputSpd * Time.deltaTime;
        /// Next we clamp the vector so that the aiming position does not get too far away from the player.
        aimPos = new Vector3(Mathf.Clamp(aimPos.x, transform.position.x - maxReticleDiff, transform.position.x + maxReticleDiff) - groundScript.GetXSpeed() * 5 * Time.deltaTime,
                               Mathf.Clamp(aimPos.y, transform.position.y - maxReticleDiff, transform.position.y + maxReticleDiff),
                              aimPos.z);
        /// Next we lerp the player crosshair to the input vector position.
        reticleFarTra.position = Vector3.Lerp(reticleFarTra.position, aimPos,
                                             Time.deltaTime * reticleSpd);

        /// Finally we lerp the player to the reticle position, and rotate the player to face that crosshair.
        this.transform.position = Vector3.Lerp(this.transform.position,
                                   new Vector3(aimPos.x, aimPos.y, transform.position.z),
                                   Time.deltaTime * shipSpd);
        transform.LookAt(reticleFarTra.position);

        /// This line handles banking the ship by calling the Banking function to modify the Z rotation.
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y,
                                                 Banking(aimPos.x, transform.position.x, bankingZStr));

        /// This scrolls the ground once you get to the edges of the screen.
        /// It sets the ground's horizontal speed to be the difference between the player position and the limitX. 
        /// This works because the player is affected by the ground scroll script, so it is dragged back to the center of the screen.
        float absX = Mathf.Abs(transform.position.x);
        if (absX > limitX) {
            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            float newXSpeed = (absX - limitX) * clampedX;
            groundScript.SetXSpeed(newXSpeed * grndSpdMult);
        }

        /// This limits the player's Y position.
        float absY = Mathf.Abs(transform.position.y);
        if (absY > limitY) {
            transform.position = new Vector3(
                transform.position.x,
                limitY * Mathf.Clamp(transform.position.y, -1, 1),
                transform.position.z);
        }
    }

    public float Banking(float targetX, float currentX, float multiplier) {
        /// This returns the player's Z rotation according to the difference between the target reticle and the player position.
        return (targetX - currentX) * -multiplier;
    }
}