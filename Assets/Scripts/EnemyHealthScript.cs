using UnityEngine;
using System.Collections;

public class EnemyHealthScript : MonoBehaviour {
	
	public int m_healthMax = 3;
	public int m_health;
	private EnemyMovementScript m_enemyMov;
    public float m_timeSlowAmt = 0.75f;

	// Use this for initialization
	void Awake () {
		m_enemyMov = GetComponent<EnemyMovementScript>();
		m_health = m_healthMax;
	}

	
	void OnEnable() {
		m_health = m_healthMax;
	}

	// Update is called once per frame
	void Update () {

    }


    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "World") {
            ApplyDamage(3);
        } else if (collision.collider.tag == "Player") {
            ApplyDamage(3);
        }
    }


    void ApplyDamage(int damage) {
		if(m_health >0)
            m_health -= damage;
		if(m_health <=0) {
			m_enemyMov.DeathSequence();
		}
	}
}
