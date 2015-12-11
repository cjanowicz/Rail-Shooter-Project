using UnityEngine;

public class UIManager : MonoBehaviour {

    //Must be set in Editor:
    public GameObject m_whiteOutPlane;

    public float m_whiteOutChangeSpeed = 3;
    private Material m_whiteOutMat;
    private Vector4 m_clearWhite;
    private bool m_whiteOutVisible = true;
    private bool m_whiteOutChange = false;

    public GameObject m_gameManager;

    private void Awake() {
        m_clearWhite = new Vector4(1, 1, 1, 0);

        m_whiteOutPlane.SetActive(m_whiteOutVisible);

        m_whiteOutMat = m_whiteOutPlane.GetComponent<Renderer>().materials[0];
        m_whiteOutMat.color = Color.white;
    }

    private void Update() {
        if (m_whiteOutChange) {
            if (m_whiteOutVisible == false) {
                m_whiteOutMat.color = (Color)Vector4.Lerp(m_whiteOutMat.color, Color.white, m_whiteOutChangeSpeed * Time.deltaTime);
            } else {
                m_whiteOutMat.color = (Color)Vector4.Lerp(m_whiteOutMat.color, m_clearWhite, m_whiteOutChangeSpeed * Time.deltaTime);
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
        m_whiteOutChange = true;
        m_whiteOutPlane.SetActive(true);
        Invoke("EndFadeIn", m_whiteOutChangeSpeed / 2);
    }

    public void StartFadeOut() {
        CancelInvoke("EndFadeIn");
        CancelInvoke("EndFadeOut");
        EndFadeIn();
        m_whiteOutChange = true;
        Invoke("EndFadeOut", m_whiteOutChangeSpeed / 2);
    }

    private void EndFadeIn() {
        m_whiteOutChange = false;
        m_whiteOutVisible = true;
    }

    private void EndFadeOut() {
        m_whiteOutChange = false;
        m_whiteOutPlane.SetActive(false);
        m_whiteOutVisible = false;
    }
}