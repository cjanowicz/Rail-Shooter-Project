using UnityEngine;

public class EnemyHealthScript : MonoBehaviour {
    public int healthMax = 3;
    public int health;
    private EnemyMovementScript enemyMov;
    private Renderer modelRenderer;
    private Material modelMat;
    public Material hitFlashMat;
    public float flashLength = 0.1f;

    private void Awake() {
        enemyMov = GetComponent<EnemyMovementScript>();
        modelRenderer = enemyMov.shipModel.GetComponent<Renderer>();
        modelMat = modelRenderer.material;
        health = healthMax;
    }

    private void OnEnable() {
        health = healthMax;
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
        if (health > 0)
            health -= damage;
        if (health <= 0) {
            enemyMov.DeathSequence();
        }
        PlayHitEffects();
    }

    private void PlayHitEffects() {
        CancelInvoke("ResetColor");
        modelRenderer.material = hitFlashMat;
        enemyMov.RotationPunch();
        if (enemyMov.isBoss == false)
            enemyMov.PositionPunch();
        Invoke("ResetColor", flashLength);
    }

    private void ResetColor() {
        modelRenderer.material = modelMat;
    }
}