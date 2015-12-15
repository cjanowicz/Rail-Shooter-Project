using UnityEngine;

public class ExplosionScript : MonoBehaviour {
    /// <summary>
    /// This script is meant to be attached to all prefabs that are explosive one-shot effects.
    /// </summary>
    private ParticleSystem myParticleSystem;
    private float objectLifetime = 2f;
    private AudioSource audioSource;
    private static CameraShake camShakeScript;
    public float shakeStrength = 0.5f;
    private bool firstActivation = true;

    private void Awake() {
        ///On awake, we set up references that are local
        ///and also references that are static and shared by objects with this component.
        myParticleSystem = GetComponent<ParticleSystem>();
        objectLifetime = myParticleSystem.duration;
        audioSource = GetComponent<AudioSource>();
        if (camShakeScript == null) {
            camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

    private void OnEnable() {
        ///On Enable is called by Unity when the gameobject is set to active, 
        ///and the FX manager component activates the game object to trigger the effect.
        ///First we check if this is the first time the objects has been activated.
        ///If so, we check the flag and let nothing happen.
        ///If it is not, we play the effects that go with the object, and invoke its deactivation function for later on.
        ///We also make sure to stop the effects in case the object has been called again before completly finishing playing.
        if (firstActivation == false) {
            CancelInvoke();
            Invoke("DeactivateWithTimer", objectLifetime);
            if (audioSource != null) { 
                audioSource.Stop();
                audioSource.Play();
            }
            myParticleSystem.Stop();
            myParticleSystem.Play();
            camShakeScript.StartCameraShake(shakeStrength, this.transform.position);
        } else
            firstActivation = false;
    }

    private void DeactivateWithTimer() {
        ///Deactivate the game object, 
        this.gameObject.SetActive(false);
    }
}