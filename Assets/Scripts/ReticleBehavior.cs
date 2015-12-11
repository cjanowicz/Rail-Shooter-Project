using UnityEngine;

public class ReticleBehavior : MonoBehaviour {

    //Manipulate the scale and position of these for recoil and auto-aim
    public Transform m_farRet;

    public Transform m_closeRet;

    //Maniulate color of these for lock on
    private Material[] m_retMats;

    public float m_ScaleSpeed = 4f;
    public float m_scaleAdd = 4f;

    // Use this for initialization
    private void Awake() {
        m_retMats = new Material[m_farRet.childCount + m_closeRet.childCount];
        for (int i = 0; i < m_closeRet.childCount; i++) {
            m_retMats[i] = m_closeRet.GetChild(i).GetComponent<Renderer>().material;
        }
        for (int i = 0; i < m_farRet.childCount; i++) {
            m_retMats[i + m_closeRet.childCount] = m_farRet.GetChild(i).GetComponent<Renderer>().material;
        }
    }

    public void ShotTaken() {
        m_farRet.localScale += Vector3.one * m_scaleAdd;
        m_closeRet.localScale += Vector3.one * m_scaleAdd;
    }

    // Update is called once per frame
    private void Update() {
        m_farRet.localScale = Vector3.Lerp(m_farRet.localScale, Vector3.one, Mathf.Clamp01(Time.deltaTime * m_ScaleSpeed));
        m_closeRet.localScale = Vector3.Lerp(m_closeRet.localScale, Vector3.one, Mathf.Clamp01(Time.deltaTime * m_ScaleSpeed));
    }

    public void LockOn() {
        for (int i = 0; i < m_retMats.Length; i++) {
            m_retMats[i].color = Color.red;
        }
    }

    public void LockOff() {
        for (int i = 0; i < m_retMats.Length; i++) {
            m_retMats[i].color = Color.green;
        }
    }
}