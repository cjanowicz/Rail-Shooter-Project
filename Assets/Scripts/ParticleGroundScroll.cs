using UnityEngine;

public class ParticleGroundScroll : MonoBehaviour {
    public GroundScroll grndScrollRef;
    public float rotationMult = 2;

    // Update is called once per frame
    private void Update() {
        transform.localRotation = Quaternion.AngleAxis(grndScrollRef.GetXSpeed() * rotationMult, Vector3.up);
    }
}