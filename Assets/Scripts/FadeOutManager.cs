using UnityEngine;

/// <summary>
/// This script handles 
/// </summary>

public class FadeOutManager : MonoBehaviour {

    public GameObject whiteOutPlane;

    public float whiteOutChangeSpeed = 3;
    private Material whiteOutMat;
    private Vector4 clearWhite;
    private bool whiteOutVisible = true;
    private bool whiteOutChange = false;

    private void Awake() {
        clearWhite = new Vector4(1, 1, 1, 0);

        whiteOutPlane.SetActive(whiteOutVisible);

        whiteOutMat = whiteOutPlane.GetComponent<Renderer>().materials[0];
        whiteOutMat.color = Color.white;
    }

    private void Update() {
        if (whiteOutChange) {
            if (whiteOutVisible == false) {
                whiteOutMat.color = (Color)Vector4.Lerp(whiteOutMat.color, Color.white, whiteOutChangeSpeed * Time.deltaTime);
            } else {
                whiteOutMat.color = (Color)Vector4.Lerp(whiteOutMat.color, clearWhite, whiteOutChangeSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdateTransition(Vector4 value, Vector4 target, float duration) {
        value = Vector4.Lerp(value, target, Mathf.Clamp01(duration * 10f * Time.deltaTime));
    }

    public void StartFadeIn() {
        CancelInvoke("EndFadeIn");
        CancelInvoke("EndFadeOut");
        EndFadeOut();
        whiteOutChange = true;
        whiteOutPlane.SetActive(true);
        Invoke("EndFadeIn", whiteOutChangeSpeed / 2);
    }

    public void StartFadeOut() {
        CancelInvoke("EndFadeIn");
        CancelInvoke("EndFadeOut");
        EndFadeIn();
        whiteOutChange = true;
        Invoke("EndFadeOut", whiteOutChangeSpeed / 2);
    }

    private void EndFadeIn() {
        whiteOutChange = false;
        whiteOutVisible = true;
    }

    private void EndFadeOut() {
        whiteOutChange = false;
        whiteOutPlane.SetActive(false);
        whiteOutVisible = false;
    }
}