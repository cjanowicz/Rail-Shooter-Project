using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    public float m_axisDamping = 0.25f;
    public float m_limitX = 6f;
    public float m_limitY = 5f;
    public float m_targetX = 3f;

    //---=== GameLogic Variables ===---//
    private GameObject myGameManager;
    private GroundScroll m_groundScript;
    public float groundMult = 0.25f;

    //---=== Movement Variables ===---//
    public float movementSpeed = 8.0f;
    public float maxDegreesDelta = 120.0f;
    public float maxVertical = 5.5f;

    //Movement interpolation Variables; (NOTE: MAKE SURE TO CHANGE EDITOR VARIABLES TO CODE)

    //The input variable
    Vector3 steerInput = new Vector3();
    //The Vector of where the ship was facing last frame
    Vector3 steerStored = new Vector3();
    //The current variable of where the ship is facing.
    Vector3 steerCurrent = new Vector3();
    //The clamp for the difference between the 
    public float steerMaxLimiter = 0.5f;
    //The t value fed into the lerp function (percent every frame)
    public float steerT = 0.1f;
    //How far into the z axis the current steering is placed;
    public float steerZOffset = 2.0f;

    

    void Awake() {
        myGameManager = GameObject.Find("GameManager");
        m_groundScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll>();
    }

    // Use this for initialization
    void Start() {
        steerStored = new Vector3(0, 0, 0);
        steerCurrent.Set(0, 0, steerZOffset);
    }


    // Update is called once per frame
    public void Move(float h, float v) {
        //---=== New Code ===---//

        steerInput = new Vector3(h, -v, 0);
        Vector3 deltaInput = steerInput - steerStored;
        deltaInput = Vector3.ClampMagnitude(deltaInput, steerMaxLimiter);

        steerStored = Vector3.Lerp(steerStored, steerStored + deltaInput, steerT);
        steerCurrent.Set(steerStored.x, steerStored.y, steerZOffset);

        transform.localPosition += steerStored * movementSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation, Quaternion.LookRotation(steerCurrent),
            Mathf.Deg2Rad * maxDegreesDelta);

        //////---===== Banking ======------
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y,
                                                 steerCurrent.x * -30);

        //Scroll the ground once you get to the edges of the screen.
        float absX = Mathf.Abs(transform.position.x);
        if (absX > m_limitX) {
            Debug.Log("Player is outside X Limit");

            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            float newXSpeed = (absX - m_limitX) * clampedX;
            m_groundScript.SetXSpeed(newXSpeed * groundMult);
        }
            /*
            ///////----------============== POSITION LIMITING =================------------------
            if (transform.localPosition.x < -maxHorizontal) {
                transform.localPosition = new Vector3
                    (-maxHorizontal + maxAdjust, transform.localPosition.y, transform.localPosition.z);
                m_groundScript.SetXSpeed(h * groundMult);
            }
            if (transform.localPosition.x > maxHorizontal) {
                transform.localPosition = new Vector3
                    (maxHorizontal - maxAdjust, transform.localPosition.y, transform.localPosition.z);
                m_groundScript.SetXSpeed(h * groundMult);
            }
            if (transform.localPosition.y < -maxVertical) {
                transform.localPosition = new Vector3
                    (transform.localPosition.x, -maxVertical + maxAdjust, transform.localPosition.z);
                m_groundScript.SetXSpeed(h * groundMult);
            }
            if (transform.localPosition.y > maxVertical) {
                transform.localPosition = new Vector3
                    (transform.localPosition.x, maxVertical - maxAdjust, transform.localPosition.z);
                m_groundScript.SetXSpeed(h * groundMult);
            }
            */
        }

    /*
    public void Move(float h, float v) {
        transform.position += new Vector3(h, v) * m_axisDamping;

        if(Mathf.Abs(transform.position.x) > m_limitX) {
            //if(m_goingToTarget = )
            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            m_groundScroll.SetXSpeed(Mathf.Abs(transform.position.x) - m_limitX * clampedX);
            transform.position = Vector3.Lerp(transform.position, new Vector3( clampedX * m_limitX, 
                transform.position.y, transform.position.z), 0.05f);

        }
        else
            m_groundScroll.SetXSpeed(0);
    }*/
}
