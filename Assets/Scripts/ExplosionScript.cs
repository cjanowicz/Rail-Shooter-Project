using UnityEngine;

public class ExplosionScript : MonoBehaviour {
    private ParticleSystem particleSystem;
    private float objectLifetime = 2f;
    private AudioSource audioSource;
    private static CameraShake camShakeScript;
    public float shakeStrength = 0.5f;
    private bool firstActivation = true;

    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
        objectLifetime = particleSystem.duration;
        audioSource = GetComponent<AudioSource>();
        if (camShakeScript == null) {
            camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

    private void OnEnable() {
        if (firstActivation == false) {
            CancelInvoke();
            particleSystem.Stop();
            if (audioSource != null)
                audioSource.Stop();
            Invoke("DeactivateWithTimer", objectLifetime);
            if (audioSource != null)
                audioSource.Play();
            camShakeScript.StartCameraShake(shakeStrength, this.transform.position);
            particleSystem.Play();
        } else
            firstActivation = false;
    }

    private void DeactivateWithTimer() {
        particleSystem.Stop();
        if (audioSource != null)
            audioSource.Stop();
        this.gameObject.SetActive(false);
    }
}