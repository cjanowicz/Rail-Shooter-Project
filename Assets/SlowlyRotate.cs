using UnityEngine;

/// <summary>
/// This behavior rotates the object along the specified axis by the default speed, 
/// and it interpolates it's speed back to the default over time.
/// Methods to call:
///     SetSpeed: Increase the rotation speed, which goes back to default over time.
/// </summary>

public class SlowlyRotate : MonoBehaviour {
    public Vector3 axis;
    public float defaultSpeed = 40f;
    private float speed;
    public float speedLerpSpeed = 2f;

    public bool isUI = true;
    private float lastRealTime = 0;

    private void Start() {
        /// In start, we set the default values.
        speed = defaultSpeed;
        if (axis.magnitude == 0) {
            axis = new Vector3(1,0,0);
        }
    }

    private void OnEnable() {
        /// When this game object is enabled, we set a local variable that uses unity's realtime,
        /// so that we can use delta time for smooth UI movement without relying on Time.timeScale,
        /// which we use to pause or slow down the game.
        lastRealTime = Time.realtimeSinceStartup;
    }

    private void Update() {
        /// Every frame we call the rotate function. 
        Rotate();
    }

    private void Rotate() {
        /// Here we update our local time.
        float realDeltaTime = Time.realtimeSinceStartup - lastRealTime;
        lastRealTime = Time.realtimeSinceStartup;
        /// Call the function to rotate the object transform.
        transform.Rotate(axis * realDeltaTime * speed);
        /// We interpolate the speed back to default. 
        speed = Mathf.Lerp(speed, defaultSpeed,
                              Mathf.Clamp01(realDeltaTime * speedLerpSpeed));
    }

    public void SetSpeed(float newSpeed) {
        /// This function is called by the menuManager script to make the object spin faster. 
        speed = newSpeed;
    }
}