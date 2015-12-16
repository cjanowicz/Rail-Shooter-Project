using UnityEngine;

/// <summary>
/// This script handles fading in and out of a transparent object in front of the camera. 
/// It is a simple behavior that has two methods that can be called, fade in and fade out.
/// This script needs a reference to the object that it is fading in and out.
/// The transition is done in the behavior's update function, where the plane's colour
/// is interpolated to the desired value over time.
/// Methods to call:
///     StartFadeIn(): Begins fading the transparent object in.
///     StartFadeOut(): Begins fading the transparent object out.
/// </summary>

public class FadeOutManager : MonoBehaviour {

    public GameObject whiteOutPlane;
    public float whiteOutChangeSpeed = 3;
    private Material whiteOutMat;
    private Vector4 clearWhite;
    private bool whiteOutVisible = true;
    private bool whiteOutChange = false;


    private void Awake() {
        /// In the awake function, we set our color varaibles,
        /// and set our plane by default to be enabled, and have it fade from opaque to clear.
        clearWhite = new Vector4(1, 1, 1, 0);
        whiteOutPlane.SetActive(whiteOutVisible);
        whiteOutMat = whiteOutPlane.GetComponent<Renderer>().materials[0];
        whiteOutMat.color = Color.white;
        Invoke("EndFadeIn", whiteOutChangeSpeed / 2);
    }

    private void Update() {
        /// If we want a transition to be happening,
        /// and depending on whether we want the plane to fade in or out,
        /// we interpolate the plane's color to our desired value.
        if (whiteOutChange) {
            if (whiteOutVisible == false) {
                whiteOutMat.color = (Color)Vector4.Lerp(whiteOutMat.color, Color.white, whiteOutChangeSpeed * Time.deltaTime);
            } else {
                whiteOutMat.color = (Color)Vector4.Lerp(whiteOutMat.color, clearWhite, whiteOutChangeSpeed * Time.deltaTime);
            }
        }
    }

    public void StartFadeIn() {
        /// Here we cancel whatever transitions we had running, 
        /// and then set up a transition from transparent to opaque.
        CancelInvoke("EndFadeIn");
        CancelInvoke("EndFadeOut");
        EndFadeOut();
        whiteOutChange = true;
        whiteOutPlane.SetActive(true);
        Invoke("EndFadeIn", whiteOutChangeSpeed / 2);
    }

    public void StartFadeOut() {
        /// Here we cancel whatever transitions we had running, 
        /// and then set up a transition from opaque to transparent.
        CancelInvoke("EndFadeIn");
        CancelInvoke("EndFadeOut");
        EndFadeIn();
        whiteOutChange = true;
        Invoke("EndFadeOut", whiteOutChangeSpeed / 2);
    }

    private void EndFadeIn() {
        /// After time has elapsed, we disable the transition flag,
        /// and enable the flag that tracks the plane's visibility.
        whiteOutChange = false;
        whiteOutVisible = true;
    }

    private void EndFadeOut() {
        /// After time has elapsed, we end the fade out process by setting our flags,
        /// and disabling the plane object so it does not get drawn anymore.
        whiteOutChange = false;
        whiteOutPlane.SetActive(false);
        whiteOutVisible = false;
    }
}