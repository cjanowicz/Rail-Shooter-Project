using UnityEngine;
/// <summary>
/// This script reads player input and sends it to the player behaviors that handle it.
/// </summary>

[RequireComponent(typeof(PlayerAimMovement))]
public class PlayerInputScript : MonoBehaviour {
    private PlayerAimMovement movementScript;
    private ShootingBehavior shootingScript;

    private void Awake() {
        /// Here we set references.
        movementScript = GetComponent<PlayerAimMovement>();
        shootingScript = GetComponent<ShootingBehavior>();
    }

    private void Update() {
        /// We read player input to send to the player behavior script.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1")) {
            shootingScript.Shoot();
        } else if (Input.GetButtonUp("Fire1")) {
            shootingScript.StopShooting();
        }
        /// Pass all parameters to the character control script.
        movementScript.Move(h, v);
    }
}