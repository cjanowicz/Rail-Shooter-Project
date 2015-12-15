using UnityEngine;

public class CameraShake : MonoBehaviour {

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    public float constantAmt = 0.1f;

    public float shakeAmt = 0f;
    public float decreaseFactor = 3f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeStr = 30f;

    public float maxDist = 20f;

    private Vector3 originalPos;

    private void Awake() {
        camTransform = GetComponent<Transform>();
    }

    private void OnEnable() {
        originalPos = camTransform.localPosition;
    }

    private void Update() {
        camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeStr * (shakeAmt + constantAmt) * Time.deltaTime;

        if (shakeAmt > 0) {
            shakeAmt -= Time.deltaTime * decreaseFactor;
        } else {
            shakeAmt = 0f;
        }
    }

    public void StartCameraShake(float incomingAmt, Vector3 origin) {
        Vector3 relativeVec = origin - camTransform.position;
        float newAmt = Mathf.Clamp01((maxDist - relativeVec.magnitude) / maxDist) * incomingAmt;
        if (newAmt > shakeAmt) {
            shakeAmt = newAmt;
        }
    }
}