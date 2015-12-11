using UnityEngine;

public class PlayerColliderScript : MonoBehaviour {
    /*void OnCollisionEnter(Collision collision) {
		if(collision.transform.tag == "World"){
			DamageUpwardsObject();
		} else if (collision.transform.tag == "Enemy") {
			DamageUpwardsObject();
		}
	}
*/

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "World") {
            DamageUpwardsObject();
        } else if (other.tag == "Enemy") {
            DamageUpwardsObject();
        }
    }

    private void DamageUpwardsObject() {
        SendMessageUpwards("ApplyDamage", SendMessageOptions.DontRequireReceiver);
    }
}