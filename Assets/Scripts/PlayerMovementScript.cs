using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {


    enum State { Cutscene, Playing, JustHurt, GameOver };
    State m_state;

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
    public float m_steerMaxDiff = 0.5f;
    //The t value fed into the lerp function (percent every frame)
    public float m_steerSensitivity = 7f;
    //How far into the z axis the current steering is placed;
    public float m_steerZOffset = 2.0f;



    void Awake() {
        myGameManager = GameObject.Find("GameManager");
        m_groundScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll>();
        m_state = State.Cutscene;
    }

    // Use this for initialization
    void Start() {
        steerStored = new Vector3(0, 0, 0);
        steerCurrent.Set(0, 0, m_steerZOffset);
        m_state = State.JustHurt;
    }


    // Update is called once per frame
    public void Move(float h, float v) {
        //---=== New Code ===---//


        switch (m_state) {
            case State.JustHurt:
            case State.Playing:

                //TODO: Implement Inverted/Not inverted controls
                steerInput = new Vector3(h, v, 0);
                Vector3 deltaInput = steerInput - steerStored;
                deltaInput = Vector3.ClampMagnitude(deltaInput, m_steerMaxDiff);

                steerStored = Vector3.Lerp(steerStored, steerStored + deltaInput, Mathf.Clamp01(Time.deltaTime * m_steerSensitivity));
                steerCurrent.Set(steerStored.x, steerStored.y, m_steerZOffset);

                transform.localPosition += steerStored * movementSpeed * Time.deltaTime;

                transform.localRotation = Quaternion.RotateTowards(
                    transform.localRotation, Quaternion.LookRotation(steerCurrent),
                    Mathf.Deg2Rad * maxDegreesDelta * Time.deltaTime);

                break;
            
        }

        //////---===== Banking ======------
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                     transform.localEulerAngles.y,
                                                     steerCurrent.x * -30);



        //Scroll the ground once you get to the edges of the screen.
        float absX = Mathf.Abs(transform.position.x);
        if (absX > m_limitX) {

            float clampedX = Mathf.Clamp(transform.position.x, -1, 1);
            float newXSpeed = (absX - m_limitX) * clampedX;
            m_groundScript.SetXSpeed(newXSpeed * groundMult);
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
