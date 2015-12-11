using UnityEngine;

public class BillBoardingPlane : MonoBehaviour {
    private Transform myTransform;
    private Transform target;
    private Vector3 rightAngle = new Vector3(90, 0, 0);
    private bool flicker = true;
    public Vector3 scaleAlt = new Vector3(0.04f, 0, 0.04f);

    private void LateUpdate() {
        myTransform.LookAt(target);
        myTransform.Rotate(rightAngle);

        if (flicker == true) {
            myTransform.localScale += scaleAlt;
            flicker = false;
        } else {
            myTransform.localScale -= scaleAlt;
            flicker = true;
        }
    }

    private void Awake() {
        myTransform = this.transform; //cache the transform
        target = Camera.main.transform; //cache the transform of the camera
    }
}