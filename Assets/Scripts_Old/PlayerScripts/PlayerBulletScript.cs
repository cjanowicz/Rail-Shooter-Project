using UnityEngine;
using System.Collections;

public class PlayerBulletScript : MonoBehaviour {

    private static GameObject m_fxManager;
    public int m_damage = 1;
    public float m_lifeTime = 2f;
    private AudioSource m_audioSource;

    // Use this for initialization
    void Awake() {
        m_fxManager = GameObject.Find("FXManager");
        m_audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() {
        Invoke("Deactivate", m_lifeTime);
        m_audioSource.Play();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Powerup" && other.tag != "Player" && other.tag != "EnemyBullet") {
            other.SendMessage("ApplyDamage", m_damage, SendMessageOptions.DontRequireReceiver);
            //Play Effects
            //Debug.Log("Player Bullet Hit: " + other.name);
            DestroySelf();
        }

    }

    void DestroySelf() {
        m_fxManager.SendMessage("CallSmallExplosion", this.transform.position);
        Deactivate();
        CancelInvoke("Deactivate");
    }

    void Deactivate() {
        this.gameObject.SetActive(false);
    }
}
