using UnityEngine;

public class LerpMoveScript : MonoBehaviour {
    public Transform m_target;
    private float m_damper = 0.5f;

    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        transform.position = Vector3.Lerp(transform.position, m_target.position, m_damper);
        transform.rotation = Quaternion.Lerp(transform.rotation, m_target.rotation, m_damper);
    }
}