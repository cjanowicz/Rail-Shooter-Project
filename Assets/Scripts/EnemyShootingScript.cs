using UnityEngine;

public class EnemyShootingScript : MonoBehaviour {
    private static FXManager fXManagerScript;
    private static Transform playerTrans;
    public Transform shotTransform;
    public float shotVelocity = 40f;

    // Use this for initialization
    private void Awake() {
        if (fXManagerScript == null) {
            fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
            playerTrans = GameObject.Find("PlayerShape").transform;
        }
    }

    public void FireAtPlayer() {
        shotTransform.LookAt(playerTrans);
        fXManagerScript.CallEnemyBullet(shotTransform.position, shotTransform.rotation, shotVelocity);
    }
}