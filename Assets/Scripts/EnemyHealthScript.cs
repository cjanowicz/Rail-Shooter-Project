using UnityEngine;

public class EnemyHealthScript : MonoBehaviour {
    public int m_healthMax = 3;
    public int m_health;
    private EnemyMovementScript m_enemyMov;
    private Renderer m_modelRenderer;
    private Material m_modelMat;
    public Material m_hitFlashMat;
    public float m_flashLength = 0.1f;

    private void Awake() {
        m_enemyMov = GetComponent<EnemyMovementScript>();
        m_modelRenderer = m_enemyMov.shipModel.GetComponent<Renderer>();
        m_modelMat = m_modelRenderer.material;
        m_health = m_healthMax;
    }

    private void OnEnable() {
        m_health = m_healthMax;
    }

    private void Update() {
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "World") {
            ApplyDamage(3);
        } else if (collision.collider.tag == "Player") {
            ApplyDamage(3);
        }
    }

    private void ApplyDamage(int damage) {
        if (m_health > 0)
            m_health -= damage;
        if (m_health <= 0) {
            m_enemyMov.DeathSequence();
        }
        PlayHitEffects();
    }

    private void PlayHitEffects() {
        CancelInvoke("ResetColor");
        m_modelRenderer.material = m_hitFlashMat;
        m_enemyMov.RotationPunch();
        if (m_enemyMov.m_isBoss == false)
            m_enemyMov.PositionPunch();
        Invoke("ResetColor", m_flashLength);
    }

    private void ResetColor() {
        m_modelRenderer.material = m_modelMat;
    }
}