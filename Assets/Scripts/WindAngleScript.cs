using UnityEngine;

/// <summary>
/// Simple script that sets the angle of this object to correlate with the horizontal speed of the ground plane.
/// </summary>

public class WindAngleScript : MonoBehaviour {

    public GroundScroll grndScrollRef;
    public float rotationMult = 2;
    
    private void Update() {
        /// Ever frame, the object rotates itself to be rotated on the X axis
        /// by the same amount that the ground speed is on the X axis,
        /// with a multiplier.
        transform.localRotation = Quaternion.AngleAxis(grndScrollRef.GetXSpeed() * rotationMult, Vector3.up);
    }
}