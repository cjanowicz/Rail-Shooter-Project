using UnityEngine;

/// <summary>
/// This script handles the enemy character health, as well as 
/// playing feedback for when it receives damage. It communicates
/// with the enemyMovementScript to let it know that it should
/// start the enemy death sequence.
/// Methods to call:
///     ApplyDamage(int damage): Applies damage to the enemy.
/// </summary>

public class EnemyHealthScript : MonoBehaviour {
    public int healthMax = 3;
    public int health;
    public int damageFromCollision = 3;
    private EnemyMovementScript enemyMov;
    private Renderer modelRenderer;
    private Material modelMat;
    public Material hitFlashMat;
    public float flashLength = 0.1f;

    private void Awake() {
        /// Setting references.
        enemyMov = GetComponent<EnemyMovementScript>();
        modelRenderer = enemyMov.shipModel.GetComponent<Renderer>();
        modelMat = modelRenderer.material;
        health = healthMax;
    }

    private void OnEnable() {
        /// Every time the object is enabled, its health resets.
        health = healthMax;
    }

    private void OnCollisionEnter(Collision collision) {
        /// If the enemy collides with objects that have certain tags
        /// it receives three damage.
        if (collision.collider.tag == "World") {
            ApplyDamage(damageFromCollision);
        } else if (collision.collider.tag == "Player") {
            ApplyDamage(damageFromCollision);
        }
    }

    private void ApplyDamage(int damage) {
        /// This function is called by anything that would deal damage to the enemy.
        /// It applies damage until the enemy's health is below zero, in which case
        /// it tells the movement script to start the death sequence.
        if (health > 0)
            health -= damage;
        if (health <= 0) {
            enemyMov.DeathSequence();
        }
        /// Here we call our function to play the damage effects.
        PlayHitEffects();
    }

    private void PlayHitEffects() {
        /// This function cancels our previous invoke calls,
        /// then creates the following feedback:
        ///     change the enemy color to pure white
        ///     rotate the enemy by a random amount,
        ///     then call the function to reset the colour in flashLength seconds.
        CancelInvoke("ResetColor");
        modelRenderer.material = hitFlashMat;
        enemyMov.RotationPunch();
        Invoke("ResetColor", flashLength);
    }

    private void ResetColor() {
        /// after flashLength has expired, we reset the model's colour.
        modelRenderer.material = modelMat;
    }
}