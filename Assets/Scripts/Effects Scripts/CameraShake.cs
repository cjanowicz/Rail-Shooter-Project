using UnityEngine;

/// <summary>
/// This script shakes the transform that this object is attached to over time.
/// Methods to call:
///     StartCameraShake(float incomingAmt, Vector3 origin): Called by any object that needs to create camera shake. 
/// </summary>

public class CameraShake : MonoBehaviour {

    private Transform camTransform;
    public float constantAmt = 0.1f;
    public float shakeAmt = 0f;
    public float decreaseFactor = 3f;
    public float shakeStr = 0.5f;
    public float maxDist = 20f;
    private Vector3 originalPos;

    private void Awake() {
        /// On awake, we cache the transform.
        camTransform = GetComponent<Transform>();
    }

    private void OnEnable() {
        /// On enable, we set our original position.
        originalPos = camTransform.localPosition;
    }

    private void Update() {
        /// On update, we set the camera's position to be the original position plus a random point inside a 1 unit radius sphere
        /// and multiply that sphere's random point by how strong we want our shake to be.
        camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeStr * (shakeAmt + constantAmt) * Time.timeScale;

        /// Shake amount is a float that is used as a timer and an amount of shaking we want.
        /// It decreases to zero with time as a function of our decreaseFactor variable.
        if (shakeAmt > 0) {
            shakeAmt -= Time.deltaTime * decreaseFactor;
        } else {
            shakeAmt = 0f;
        }
    }

    public void StartCameraShake(float incomingAmt, Vector3 origin) {
        /// When this function is called, it first calculates the relative distance between the camera and the calling object
        Vector3 relativeVec = origin - camTransform.position;
        /// Then it multiplies the incoming amount by the how close the object is.
        float newAmt = incomingAmt * Mathf.Clamp01((maxDist - relativeVec.magnitude) / maxDist);
        /// Finally it checks if the new amount is biggert than the amount of shaking currently happening. If so, it gets used.
        if (newAmt > shakeAmt) {
            shakeAmt = newAmt;
        }
    }
}