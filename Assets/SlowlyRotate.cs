using UnityEngine;

public class SlowlyRotate : MonoBehaviour {
    public Vector3 axis;
    public float defaultSpeed = 40f;
    private float speed;
    public float speedLerpSpeed = 2f;

    public bool isUI = true;
    private float lastRealTime = 0;

    private void Start() {
        speed = defaultSpeed;
        if (axis.magnitude == 0) {
            axis = new Vector3(1,0,0);
        }
    }

    private void OnEnable() {
        lastRealTime = Time.realtimeSinceStartup;
    }

    private void Update() {
        if (isUI == false) {
            RotateRegular();
        }
        if (isUI) {
            RotateUI();
        }
    }

    private void RotateRegular() {
        transform.Rotate(axis * Time.deltaTime * speed);
        speed = Mathf.Lerp(speed, defaultSpeed,
                              Mathf.Clamp01(Time.deltaTime * speedLerpSpeed));
    }

    private void RotateUI() {
        float realDeltaTime = Time.realtimeSinceStartup - lastRealTime;
        lastRealTime = Time.realtimeSinceStartup;
        transform.Rotate(axis * realDeltaTime * speed);
        speed = Mathf.Lerp(speed, defaultSpeed,
                              Mathf.Clamp01(realDeltaTime * speedLerpSpeed));
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    public void ResetSpeed() {
        speed = defaultSpeed;
    }
}