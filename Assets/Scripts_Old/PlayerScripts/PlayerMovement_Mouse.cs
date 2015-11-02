using UnityEngine;
using System.Collections;

public class PlayerMovement_Mouse : MonoBehaviour {


    private Vector2 mInput = new Vector2(0, 0);
    private Vector2 mTurn = new Vector2(0, 0);

    private bool barrel = false;

    public float responsiveness = 5;
    public float turnSensitivity = 5;
    public float rotationSpeed = 5;
    public Vector2 movementDistance = new Vector2(0, 0);
    public Transform mTrans;
    public float screenArea;
    public Vector3 turnDegrees = new Vector3(0, 0, 0);

    private Vector2 oldPosition;

    //---=== GameLogic Variables ===---//
    private GameObject myGameManager;
    private GroundScroll m_groundScript;
    public float groundMult = 0.25f;
    public float m_limitX = 5f;
    public float m_limitY = 5f;

    void Start() {
        myGameManager = GameObject.Find("GameManager");
        m_groundScript = GameObject.Find("GroundPlane").GetComponent<GroundScroll>();

        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
        oldPosition = (Vector2)transform.position;
    }

    // Update is called once per frame
    void Update() {
        NewMovementAndRot();
    }

    IEnumerator DoABarrelRoll(float duration) {
        barrel = true;
        yield return new WaitForSeconds(duration);
        barrel = false;
    }


    void NewMovementAndRot() {

        Vector3 pos = Input.mousePosition;

        float x = -Mathf.Clamp((Screen.width * 0.5f - pos.x) / (Screen.width * screenArea), -1f, 1f);
        float y = -Mathf.Clamp((Screen.height * 0.5f - pos.y) / (Screen.height * screenArea), -1f, 1f);

        Vector2 vec = new Vector2(x, y);
        float mag = vec.magnitude;
        if (mag > 1f)
            vec *= 1.0f / mag;
        /*
        mInput = Vector2.Lerp(mInput, vec, Mathf.Clamp01(Time.deltaTime * responsiveness));
        mTurn = Vector2.Lerp(mTurn, vec - mInput, Mathf.Clamp01(Time.deltaTime * turnSensitivity));
        mTrans.localPosition = new Vector3(mInput.x * movementDistance.x, mInput.y * movementDistance.y, 10);
        */
        Vector2 deltaPosition = (Vector2)transform.position - oldPosition;
        mInput = Vector2.Lerp(mInput + deltaPosition, vec, Mathf.Clamp01(Time.deltaTime * responsiveness));
        mTurn = Vector2.Lerp(mTurn, vec - mInput, Mathf.Clamp01(Time.deltaTime * turnSensitivity));
        mTrans.localPosition = new Vector3(mInput.x * movementDistance.x, mInput.y * movementDistance.y, 10);

        oldPosition = transform.position;

        if (!barrel) {
            mTrans.localRotation = Quaternion.Euler(-mTurn.y * turnDegrees.x, mTurn.x * turnDegrees.y, -mTurn.x * turnDegrees.z);
        } else
            transform.Rotate(transform.forward, 20 * Time.deltaTime * rotationSpeed * 20f);


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
