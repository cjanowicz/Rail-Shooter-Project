using UnityEngine;

public class EnemyBulletScript : MonoBehaviour {
    private static GameObject m_fxManager;
    public int m_damage = 1;
    public float m_lifeTime = 2f;
    private AudioSource m_audioSource;

    private void Awake() {
        m_fxManager = GameObject.Find("FXManager");
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        Invoke("Deactivate", m_lifeTime);
        m_audioSource.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Powerup" && other.tag != "Enemy" && other.tag != "Bullet" && other.tag != "EnemyBullet") {
            other.SendMessageUpwards("ApplyDamage", 1.0f, SendMessageOptions.DontRequireReceiver);
            DestroySelf();
        }
    }

    private void DestroySelf() {
        m_fxManager.SendMessage("CallSmallExplosion", this.transform.position);
        Deactivate();
        CancelInvoke("Deactivate");
    }

    private void Deactivate() {
        this.gameObject.SetActive(false);
    }
}