using UnityEngine;

public class PlayerAimMovement : MonoBehaviour {
    private GroundScroll m_groundScript;

    [SerializeField]
    private float m_grndSpdMult = 20;

    private AppManager m_appManagerScript;
    private int m_invert;

    public Transform m_reticleFarTra;
    public Transform m_reticleCloseTra;
    private Material m_reticleMat;
    private Color m_retDefColor;

    [SerializeField]
    private float m_inputSpd = 20;

    [SerializeField]
    private float m_reticleSpd = 20;

    private Vector3 m_aimPos;

    [SerializeField]
    private float m_shipSpd = 3;

    [SerializeField]
    private float m_shipRotateSpd = 100;

    [SerializeField]
    private float m_maxReticleDiff = 5;

    [SerializeField]
    private float m_limitX = 5;

    [SerializeField]
    private float m_limitY = 5;

    [SerializeField]
    private float m_reticleYLimit = -4;

    // Use this for initialization
    private void Start() {
        m_appManagerScript = GameObject.Find("AppManager(Clone)").GetComponent<AppManager>();
        m_invert = m_appManagerScript.GetInvert();
        m_aimPos = m_reticleFarTra.position;
        m_groundScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll>();
        m_reticleMat = m_reticleCloseTra.GetChild(0).GetComponent<Renderer>().materials[0];
        m_retDefColor = m_reticleMat.color;
    }

    // Update is called once per frame
    private void Update() {
        m_reticleCloseTra.position = (m_reticleFarTra.position + transform.position) / 2;
    }

    public void Move(float h, float v) {
        // Accept Player Input
        m_aimPos += new Vector3(h, v * m_invert, 0) * m_inputSpd * Time.deltaTime;
        m_aimPos = new Vector3(Mathf.Clamp(m_aimPos.x, transform.position.x - m_maxReticleDiff, transform.position.x + m_maxReticleDiff) - m_groundScript.GetXSpeed() * 5 * Time.deltaTime,
                               Mathf.Clamp(m_aimPos.y, transform.position.y - m_maxReticleDiff, transform.position.y + m_maxReticleDiff),
                              m_aimPos.z);
        m_aimPos.y = Mathf.Max(m_aimPos.y, m_reticleYLimit);
        m_reticleFarTra.position = Vector3.Lerp(m_reticleFarTra.position, m_aimPos,
                                             Time.deltaTime * m_reticleSpd);

        this.transform.position = Vector3.Lerp(this.transform.position,
                                   new Vector3(m_aimPos.x, m_aimPos.y, transform.position.z),
                                   Time.deltaTime * m_shipSpd);
        transform.LookAt(m_reticleFarTra.position);

        // This line handles banking the ship
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y,
                                                 (m_aimPos.x - transform.localPosition.x) * -10);

        //Scroll the ground once you get to the edges of the screen.
        float absX = Mathf.Abs(transform.position.x);
        if (absX > m_limitX) {
            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            float newXSpeed = (absX - m_limitX) * clampedX;
            m_groundScript.SetXSpeed(newXSpeed * m_grndSpdMult);
        }

        float absY = Mathf.Abs(transform.position.y);
        if (absY > m_limitY) {
            transform.position = new Vector3(
                transform.position.x,
                m_limitY * Mathf.Clamp(transform.position.y, -1, 1),
                transform.position.z);
        }
    }
}