using UnityEngine;

public class PlayerAimMovement : MonoBehaviour {
    private GroundScroll groundScript;

    [SerializeField]
    private float grndSpdMult = 0.5f;

    private AppManager appManagerScript;
    private int invert;

    public Transform reticleFarTra;
    public Transform reticleCloseTra;
    private Material reticleMat;

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

    // Use this for initialization
    private void Start() {
        appManagerScript = GameObject.Find("AppManager(Clone)").GetComponent<AppManager>();
        invert = appManagerScript.GetInvert();
        aimPos = reticleFarTra.position;
        groundScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll>();
        reticleMat = reticleCloseTra.GetChild(0).GetComponent<Renderer>().materials[0];
    }

    // Update is called once per frame
    private void Update() {
        reticleCloseTra.position = (reticleFarTra.position + transform.position) / 2;
    }

    public void Move(float h, float v) {
        // Accept Player Input
        aimPos += new Vector3(h, v * invert, 0) * inputSpd * Time.deltaTime;
        aimPos = new Vector3(Mathf.Clamp(aimPos.x, transform.position.x - maxReticleDiff, transform.position.x + maxReticleDiff) - groundScript.GetXSpeed() * 5 * Time.deltaTime,
                               Mathf.Clamp(aimPos.y, transform.position.y - maxReticleDiff, transform.position.y + maxReticleDiff),
                              aimPos.z);
        aimPos.y = Mathf.Max(aimPos.y, reticleYLimit);
        reticleFarTra.position = Vector3.Lerp(reticleFarTra.position, aimPos,
                                             Time.deltaTime * reticleSpd);

        this.transform.position = Vector3.Lerp(this.transform.position,
                                   new Vector3(aimPos.x, aimPos.y, transform.position.z),
                                   Time.deltaTime * shipSpd);
        transform.LookAt(reticleFarTra.position);

        // This line handles banking the ship
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y,
                                                 Banking(aimPos.x, transform.position.x, bankingZStr));

        //Scroll the ground once you get to the edges of the screen.
        float absX = Mathf.Abs(transform.position.x);
        if (absX > limitX) {
            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            float newXSpeed = (absX - limitX) * clampedX;
            groundScript.SetXSpeed(newXSpeed * grndSpdMult);
        }

        float absY = Mathf.Abs(transform.position.y);
        if (absY > limitY) {
            transform.position = new Vector3(
                transform.position.x,
                limitY * Mathf.Clamp(transform.position.y, -1, 1),
                transform.position.z);
        }
    }

    public float Banking(float targetX, float currentX, float multiplier) {
        return (targetX - currentX) * -multiplier;
    }
}