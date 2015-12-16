using UnityEngine;

/// <summary>
/// This simple behavior is attached to the child colliders on the player object.
/// When a collision is detected with a world object or an enemy, it sends a message
/// updates to the health script to take damage.
/// </summary>

public class PlayerColliderScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        /// If the collider hits an object that is an enemy or a level geometry/obstacle/world object...
        if (other.tag == "World" || other.tag == "Enemy") {
            DamageUpwardsObject();
        }
    }

    private void DamageUpwardsObject() {
        /// ... then we send a message to the parent object to decrease player health.
        SendMessageUpwards("ApplyDamage", SendMessageOptions.DontRequireReceiver);
    }
}