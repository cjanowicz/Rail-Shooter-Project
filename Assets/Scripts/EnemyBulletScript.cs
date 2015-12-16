using UnityEngine;

/// <summary>
/// This script is for the Enemy Bullet Behavior. 
/// It is meant to play a sound effect and call relevant effects.
/// If it collides with an object that isn't another enemy or another bullet, 
/// it will send a damage message and deactivate itself for use later.
/// It is made to be working in conjunction with the FX manager script,
/// by being activated with the "OnEnable" function, and then deactivating itself for later use.
/// </summary>

public class EnemyBulletScript : MonoBehaviour {
    private static GameObject fxManager;
    public int damage = 1;
    public float lifeTime = 3f;
    private AudioSource audioSource;

    private void Awake() {
        /// First we set our references to the FX manager and our audio source.
        fxManager = GameObject.Find("FXManager");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        /// When enabled, we set up a function that will deactivate the bullet if it is alive for too long,
        /// and we play the sound effect.
        Invoke("Deactivate", lifeTime);
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other) {
        /// If we don't collide with a powerup, an enemy, a bullet, we send a message and call our destroy function.
        if (other.tag != "Powerup" && other.tag != "Enemy" && other.tag != "Bullet" && other.tag != "EnemyBullet") {
            other.SendMessageUpwards("ApplyDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
            DestroySelf();
        }
    }

    private void DestroySelf() {
        /// This function plays when the bullet hits another object, and calls the FX manager to make a hit effect,
        /// then it deactivates itself and cancels the invoke for the deactivate function.
        fxManager.SendMessage("CallSmallExplosion", this.transform.position);
        Deactivate();
        CancelInvoke("Deactivate");
    }

    private void Deactivate() {
        /// This function is called to deactivate itself for later use.
        this.gameObject.SetActive(false);
    }
}