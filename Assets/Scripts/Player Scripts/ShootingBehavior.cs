using UnityEngine;

/// <summary>
/// This behavior is meant to be attached to the player, 
/// and handle all the variables and logic for shooting.
/// The input script communicates with it, and it 
/// communicates with the FX manager to place and activate
/// player bullets.
/// This script also communicates with the reticle script to
/// trigger shooting feedback and trigger lockon effects.
/// Methods to call:
///     Shoot(): Called by the PlayerInputScript. The script eventually 
///         calls "CallPlayerBullet" from the FX manager after it runs through the
///         logic for rapid fire.
/// </summary>

public class ShootingBehavior : MonoBehaviour {

    public float shotVelocity = 80.0f;
    public Transform shotTransform;
    private FXManager fXManagerScript;
    [HideInInspector]
    public bool fireInput = false;
    private bool coolDown = false;
    public float shotDelay = 0.1f;
    public ReticleBehavior retScript;
    [Header("Accuracy Variables")]
    public float spreadPerShot = 1f;
    public float spreadRecoverRate = 1f;
    private float currentSpread = 0f;
    
    private void Awake() {
        /// Sets up the reference to the FX manager in the scene. 
        fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
    }
    
    private void Update() {
        /// Here we use two flags to use rapid fire.
        /// fireInput is whether the player is inputting the fire button
        /// coolDown is whether we just shot and are experiencing the shotDelay.
        if (fireInput == true && coolDown == false) {
            /// If ready to fire, set the coolDown and reset for the cooldown, then fire the bullet.
            coolDown = true;
            Invoke("ResetShooting", shotDelay);
            FireBullet();
        }

        /// This checks if an enemy is detected by a raycast from where the player shoots from.
        /// If so, it tells the script for the reticle to change colour.
        RaycastHit hit;
        if (Physics.Raycast(shotTransform.position, shotTransform.forward, out hit)) {
            if (hit.transform.tag == "Enemy") {
                retScript.LockOn();
            } else
                retScript.LockOff();
        }

        /// This decreases the current bullet spread by a function of delta time.
        currentSpread = Mathf.Lerp(currentSpread, 0, Mathf.Clamp01(Time.deltaTime * spreadRecoverRate));
    }

    public void Shoot() {
        /// When playerInput detects the fire button is pressed, this function is called.
        fireInput = true;
    }

    public void StopShooting() {
        /// When the fire button is released, this function is called.
        fireInput = false;
    }


    private void ResetShooting() {
        /// After the shotDelay elapses, this function resets the cooldown.
        coolDown = false;
    }

    private void FireBullet() {
        /// Handles all the logic for shooting.
        /// Starting with rotating the shootng transform a random amount according to the shot spread.
        shotTransform.Rotate(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0);
        /// Then activating a bullet at the shooting transform's position and rotation, with our speed variable.
        fXManagerScript.CallPlayerBullet(shotTransform.position, shotTransform.rotation, shotVelocity);
        /// Tell the reticle script that a shot was taken to trigger UI feedback.
        retScript.ShotTaken();
        /// Reset the shooting transform rotation.
        shotTransform.localRotation = Quaternion.identity;
        /// Finally, increment the amount of shot spread for greater inaccuracy the more the player shoots.
        currentSpread += spreadPerShot;
    }
}