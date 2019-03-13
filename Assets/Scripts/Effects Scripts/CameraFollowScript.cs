using UnityEngine;

/// <summary>
/// This script rotates and moves the camera to look in the same direction as the player
/// as well as rotate to tilt the camera. 
/// </summary>

public class CameraFollowScript : MonoBehaviour {
    ///The playerObject is the transform that we track as our target.
    public Transform playerObject;

    private Vector3 newPosition = new Vector3();
    public float followDistance = 5f;
    private Vector3 newRotation = new Vector3();
    public float rotationMultiplier = 10.0f;
    private Vector3 storedRotation = new Vector3();

    public float dampenerX = 1.5f;
    public float dampenerY = 2.0f;
    public float dampenerNewRotation = 3.0f;
    public float dampenerAngleX = 0.1f;
    public float dampenerAngleY = 0.1f;
    public float dampenerPos = 0.1f;

    private float zInterpolation;
    public float zLerpT = 0.1f;
    private float zPlayerRotation;
    private float zPlayerDampener = 0.3f;

    private void Update() {
        /// In the update function we set the camera's position...
        newPosition.Set(0, 0, followDistance);

        /// Set our destination rotation, which will be the player's
        /// rotation with a damper on it.
        newRotation.x = playerObject.localEulerAngles.x;
        newRotation.y = playerObject.localEulerAngles.y;

        if (newRotation.x > 180)
            newRotation.x -= 360;
        if (newRotation.y > 180)
            newRotation.y -= 360;
        newRotation /= dampenerNewRotation;

        zPlayerRotation = playerObject.rotation.eulerAngles.z;
        if (zPlayerRotation > 180)
            zPlayerRotation -= 360;
        zPlayerRotation *= zPlayerDampener;
        zInterpolation = Mathf.Lerp(zInterpolation, zPlayerRotation, zLerpT);

        /// Then we set the rotation that the camera will use to be an interpolation between the old and the new rotations.
        storedRotation = new Vector3(Mathf.Lerp(storedRotation.x, newRotation.x, dampenerAngleX),
                                     Mathf.Lerp(storedRotation.y, newRotation.y, dampenerAngleY),
                                     zInterpolation);
        this.transform.eulerAngles = storedRotation;
    }
}