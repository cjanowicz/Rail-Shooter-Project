using UnityEngine;

public class EnemyBulletScript : MonoBehaviour {
    private static GameObject fxManager;
    public int damage = 1;
    public float lifeTime = 3f;
    private AudioSource audioSource;

    private void Awake() {
        fxManager = GameObject.Find("FXManager");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        Invoke("Deactivate", lifeTime);
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Powerup" && other.tag != "Enemy" && other.tag != "Bullet" && other.tag != "EnemyBullet") {
            other.SendMessageUpwards("ApplyDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
            DestroySelf();
        }
    }

    private void DestroySelf() {
        fxManager.SendMessage("CallSmallExplosion", this.transform.position);
        Deactivate();
        CancelInvoke("Deactivate");
    }

    private void Deactivate() {
        this.gameObject.SetActive(false);
    }
}