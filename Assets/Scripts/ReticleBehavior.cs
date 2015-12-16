using UnityEngine;

/// <summary>
/// This behavior handles some feedback for the reticle objects,
/// namely how the reticle expands when the player is firing
/// and how the reticle turns red when an enemy is in sight.
/// Methods to call:
///     ShotTaken(): called by the PlayerShootingBehavior, increases the reticle scale.
///     LockOn(): called by the same, switches renderer materials to red.
///     LockOff(): called by the same, switches renderer materials to green.
/// </summary>

public class ReticleBehavior : MonoBehaviour {
    
    public Transform farRet;

    public Transform closeRet;
    
    private Material[] retMats;

    public float ScaleSpeed = 4f;
    public float scaleAdd = 4f;
    
    private void Awake() {
        /// Set references to all the object renderers in the specified objets.
        retMats = new Material[farRet.childCount + closeRet.childCount];
        for (int i = 0; i < closeRet.childCount; i++) {
            retMats[i] = closeRet.GetChild(i).GetComponent<Renderer>().material;
        }
        for (int i = 0; i < farRet.childCount; i++) {
            retMats[i + closeRet.childCount] = farRet.GetChild(i).GetComponent<Renderer>().material;
        }
    }

    public void ShotTaken() {
        /// Increase the scale after taking a shot.
        farRet.localScale += Vector3.one * scaleAdd;
        closeRet.localScale += Vector3.one * scaleAdd;
    }
    
    private void Update() {
        /// Every frame, the scales of the reticle objects is interpolated back down.
        farRet.localScale = Vector3.Lerp(farRet.localScale, Vector3.one, Mathf.Clamp01(Time.deltaTime * ScaleSpeed));
        closeRet.localScale = Vector3.Lerp(closeRet.localScale, Vector3.one, Mathf.Clamp01(Time.deltaTime * ScaleSpeed));
    }

    public void LockOn() {
        /// When called, all the reticle object materials are set to red.
        for (int i = 0; i < retMats.Length; i++) {
            retMats[i].color = Color.red;
        }
    }

    public void LockOff() {
        /// When called, all the reticle object materials are set to green.
        for (int i = 0; i < retMats.Length; i++) {
            retMats[i].color = Color.green;
        }
    }
}