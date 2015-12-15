using UnityEngine;

public class PlayerBulletScript : MonoBehaviour {
    private static GameObject fxManager;
    public int damage = 1;
    public float lifeTime = 2f;
    private AudioSource audioSource;
    private static CameraShake camShakeScript;
    public float shakeStrength = 0.2f;
    private bool firstActivation = true;
    public float hitTimeSlowAmt = 0.2f;
    public float hittimeSlowEnd = 0.1f;

    // Use this for initialization
    private void Awake() {
        fxManager = GameObject.Find("FXManager");
        audioSource = GetComponent<AudioSource>();
        if (camShakeScript == null) {
            camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

    private void OnEnable() {
        if (firstActivation == true) {
            firstActivation = false;
        } else {
            Invoke("Deactivate", lifeTime);
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Powerup" && other.tag != "Player" && other.tag != "EnemyBullet") {
            if (other.tag == "Enemy" || other.tag == "Boss")
                fxManager.SendMessage("CallEnemyHurt", this.transform.position);
            else
                fxManager.SendMessage("CallSmallExplosion", this.transform.position);
            other.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            DestroySelf();
            if (other.tag == "Enemy" || other.tag == "Boss") {
                Time.timeScale = hitTimeSlowAmt;
                Invoke("ResetTimeSlow", hittimeSlowEnd);
            }
        }
    }

    private void ResetTimeSlow() {
        Time.timeScale = 1f;
    }

    private void DestroySelf() {
        Deactivate();
        CancelInvoke("Deactivate");
    }

    private void Deactivate() {
        this.gameObject.SetActive(false);
    }
}