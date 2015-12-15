using UnityEngine;

public class ReticleBehavior : MonoBehaviour {

    //Manipulate the scale and position of these for recoil and auto-aim
    public Transform farRet;

    public Transform closeRet;

    //Maniulate color of these for lock on
    private Material[] retMats;

    public float ScaleSpeed = 4f;
    public float scaleAdd = 4f;

    // Use this for initialization
    private void Awake() {
        retMats = new Material[farRet.childCount + closeRet.childCount];
        for (int i = 0; i < closeRet.childCount; i++) {
            retMats[i] = closeRet.GetChild(i).GetComponent<Renderer>().material;
        }
        for (int i = 0; i < farRet.childCount; i++) {
            retMats[i + closeRet.childCount] = farRet.GetChild(i).GetComponent<Renderer>().material;
        }
    }

    public void ShotTaken() {
        farRet.localScale += Vector3.one * scaleAdd;
        closeRet.localScale += Vector3.one * scaleAdd;
    }

    // Update is called once per frame
    private void Update() {
        farRet.localScale = Vector3.Lerp(farRet.localScale, Vector3.one, Mathf.Clamp01(Time.deltaTime * ScaleSpeed));
        closeRet.localScale = Vector3.Lerp(closeRet.localScale, Vector3.one, Mathf.Clamp01(Time.deltaTime * ScaleSpeed));
    }

    public void LockOn() {
        for (int i = 0; i < retMats.Length; i++) {
            retMats[i].color = Color.red;
        }
    }

    public void LockOff() {
        for (int i = 0; i < retMats.Length; i++) {
            retMats[i].color = Color.green;
        }
    }
}