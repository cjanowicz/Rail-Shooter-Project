using UnityEngine;
using System.Collections;

public class EnemyHealthScript : MonoBehaviour {
	
	public int m_healthMax = 3;
	private int m_health;
	private EnemyMovementScript m_enemyMov;

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

	
	
	void ApplyDamage(int damage) {
		m_health -= damage;
		if (m_health <= 0) {
			m_enemyMov.DeathSequence();
			
		}
	}
}
