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
	public TextMesh m_healthText;

	// Use this for initialization
	void Start () {
        m_health = m_maxHealth;
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
		if(m_healthText == null){
			m_healthText = GameObject.Find("HealthText").GetComponent<TextMesh>();
			m_healthText.text = m_health.ToString();
		} else{
			m_healthText.text = m_health.ToString();
		}

    }

	void OnCollisionEnter(Collision collision) {
		if(collision.transform.tag == "World"){
			ApplyDamage();
		}
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "World") {
            ApplyDamage();
        } else if (other.tag == "Powerup") {
            ApplyDamage();
        } else if (other.tag == "Enemy") {
            ApplyDamage();
        }
    }

    void ApplyDamage() {
        m_health--;
        m_audioSource.PlayOneShot(m_hurtSound);
        m_animator.SetTrigger("GotHurt");
		m_healthText.text = m_health.ToString();

		if(m_health == 0){
			///Start Death Sequence
		}
    }
}
