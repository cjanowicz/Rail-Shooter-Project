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


    /// <summary>
    /// LimitY will have to be weird. I want It so be so that you are in a comfortable spot +- limitY-1. 
    /// If you try to go above LimitY, you'll skip like a fish.
    /// If you try to go below limitY, you'll encounter a strong ground effect that propels you back up.
    /// But you can go up ramps and the camera will change to reflect, 
    /// and also the camera will pan down 

        /// New idea
        /// Raycast against the ground:
        /// check height
        /// if height is below yLimitLow, apply a ground effect
        /// if height is above yLimitHigh, apply a softer ground effect/gravity.
        /// Problem, this system was designed with full control of your aim in mind. 
    /// </summary>
    [SerializeField]
    private float yLimitHigh = 8;
    [SerializeField]
    private float yLimitLow = 2;

    [SerializeField]
    private float floatLowMultiplier = 5;
    [SerializeField]
    private float floatHighMultiplier = 5;

    private float relativeY;
    private Rigidbody rb;

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

        rb = GetComponent<Rigidbody>();

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
            /// This clamped X variable acts as a multiplier.
            /// Since this function will only ever be called when X absolute position
            /// is greater than the limit we decided, it will only ever be -1 or 1.
            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            /// Thhen it is multiplied with the newXSpeed to make the speed negative when it needs to be.
            float newXSpeed = (absX - limitX) * clampedX;
            groundScript.SetXSpeed(newXSpeed * grndSpdMult);
        }

        /// This limits the player's Y position.
        /// Obsolete, remaking to make a version that has an imitation of gravity.
        /// Will need one for a lower limit too, like snowspeeders in star wars.
        /* 
        float absY = Mathf.Abs(transform.position.y);
        if (absY > limitY) {
            //If the player Y is higher than my limit
            //Simply set their position to what it was before except 
            transform.position = new Vector3(
                transform.position.x,
                limitY * Mathf.Clamp(transform.position.y, -1, 1),
                transform.position.z);
        }
        */
        relativeY = CheckHeight();
        if(relativeY < yLimitLow)
        {
            float delta = relativeY - yLimitLow;
            Vector3 force = new Vector3(0, delta, 0).normalized * floatLowMultiplier;
            rb.AddForce(force);

            Debug.Log("Beyond Low limit");
        }
        else if(relativeY > yLimitHigh)
        {
            float delta = relativeY - yLimitHigh;
            Vector3 force = new Vector3(0, delta, 0).normalized * floatHighMultiplier;
            rb.AddForce(force);
            Debug.Log("Beyond high limit");
        }
    }
    

    public float CheckHeight()
    {
        float relativePlayerHeight = 5;

        RaycastHit hit;
        if(Physics.Linecast(transform.position, transform.position + Vector3.down*20, out hit))
        {
            relativePlayerHeight = hit.distance;
        }
        else
        {
            relativePlayerHeight = 20;
        }
        return relativePlayerHeight;
    }

    public float Banking(float targetX, float currentX, float multiplier) {
        /// This returns the player's Z rotation according to the difference between the target reticle and the player position.
        return (targetX - currentX) * -multiplier;
    }
}