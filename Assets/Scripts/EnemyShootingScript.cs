using UnityEngine;

/// <summary>
/// This simple behavior fires a bullet of a pre-set type at the player when told to. 
/// Methods to call:
///     FireAtPlayer(): Fire a bullet at the player.
/// </summary>

public class EnemyShootingScript : MonoBehaviour {
    private static FXManager fXManagerScript;
    private static Transform playerTrans;
    public Transform shotTransform;
    public float shotVelocity = 40f;
    
    private void Awake() {
        /// Set up references.
        if (fXManagerScript == null) {
            fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
            playerTrans = GameObject.Find("PlayerShape").transform;
        }
    }

    public void FireAtPlayer() {
        /// Look at the player with object that is the shooting origin,
        /// then ask the FX manager to enable a bullet at the specified origin.
        shotTransform.LookAt(playerTrans);
        fXManagerScript.CallEnemyBullet(shotTransform.position, shotTransform.rotation, shotVelocity);
    }
}