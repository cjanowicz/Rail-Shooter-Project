using UnityEngine;

public class LerpMoveScript : MonoBehaviour {
    public Transform target;
    private float damper = 0.5f;

    // Update is called once per frame
    private void Update() {
        transform.position = Vector3.Lerp(transform.position, target.position, damper);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, damper);
    }
}