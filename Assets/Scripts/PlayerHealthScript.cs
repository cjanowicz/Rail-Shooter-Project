using UnityEngine;
using System.Collections;

public class PlayerHealthScript : MonoBehaviour {

    [SerializeField]
    private int m_maxHealth = 5;
    [SerializeField]
    private int m_health;
    private AudioSource m_audioSource;
    public AudioClip m_hurtSound;
    private Animator m_animator;

	// Use this for initialization
	void Start () {
        m_health = m_maxHealth;
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "World") {
            ApplyDamage();
        } else if (other.tag == "Powerup") {
            ApplyDamage();
        } else if (other.tag == "Enemy") {
            ApplyDamage();
        } else if (other.tag == "EnemyBullet") {
            ApplyDamage();
        }
    }

    void ApplyDamage() {
        Debug.Log("PlayerHit");
        m_health--;
        m_audioSource.PlayOneShot(m_hurtSound);
        m_animator.SetTrigger("GotHurt");
    }
}
