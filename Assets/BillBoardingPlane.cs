using UnityEngine;

/// <summary>
/// This script is a one-use script 
/// that rotates the attached game object to face the main camera every frame,
/// and flickers the size of the object every frame.
/// </summary>

public class BillBoardingPlane : MonoBehaviour {
    private Transform myTransform;
    private Transform target;
    private Vector3 rightAngle = new Vector3(90, 0, 0);
    private bool flicker = true;
    public Vector3 scaleAlt = new Vector3(0.04f, 0, 0.04f);

    private void Awake() {
        /// here we cache our transform and make a reference to the main camera.
        myTransform = this.transform; 
        target = Camera.main.transform;
    }

    private void LateUpdate() {
        /// After all other transformations have been applied, 
        /// we rotate the object to face the target object, which in Awake is set to the camera.
        myTransform.LookAt(target);
        myTransform.Rotate(rightAngle);

        /// Here we flicker the size of the object.
        if (flicker == true) {
            myTransform.localScale += scaleAlt;
            flicker = false;
        } else {
            myTransform.localScale -= scaleAlt;
            flicker = true;
        }
    }
}