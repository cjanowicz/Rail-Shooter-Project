using UnityEngine;

/// <summary>
/// This script handles visual and audio feedback from spawning a player bullet, 
/// and deactivates itself after colliding with another object, sending it an "ApplyDamage" message.
/// </summary>

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
    
    private void Awake() {
        /// Setting up references.
        fxManager = GameObject.Find("FXManager");
        audioSource = GetComponent<AudioSource>();
        if (camShakeScript == null) {
            camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

    private void OnEnable() {
        /// In order to prevent audio from being played and the deactivate function being called,
        /// we first check if this is the first time the object has been enabled, which would
        /// happen when the FX manager instantiates it.
        if (firstActivation == true) {
            firstActivation = false;
        } else {
            /// if not the first time, we play the audio source, and kill the game object after a set lifetime.
            Invoke("Deactivate", lifeTime);
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {
        /// According to what we hit, we call different effects from the FXManager,
        /// call a short time-slow for feedback, and then we send an ApplyDamage message,
        /// upon which we call the Destroy function.
        if (other.tag != "Powerup" && other.tag != "Player" && other.tag != "EnemyBullet") {
            if (other.tag == "Enemy" || other.tag == "Boss") {
                fxManager.SendMessage("CallEnemyHurt", this.transform.position);
                Time.timeScale = hitTimeSlowAmt;
                Invoke("ResetTimeSlow", hittimeSlowEnd);
            } else
                fxManager.SendMessage("CallSmallExplosion", this.transform.position);
            other.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            DestroySelf();
        }
    }

    private void ResetTimeSlow() {
        /// After a set time has elapsed, we set the timeScale back to default.
        Time.timeScale = 1f;
    }

    private void DestroySelf() {
        /// On the DestroySelf call, the object has hit a target and will cancel the existing
        /// invoke for "Deactivate"
        Deactivate();
        CancelInvoke("Deactivate");
    }

    private void Deactivate() {
        /// Set the object to inactive for future use.
        this.gameObject.SetActive(false);
    }
}