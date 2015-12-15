using UnityEngine;

public class ShootingBehavior : MonoBehaviour {
    public float shotVelocity = 80.0f;
    public Transform shotTransform;

    private FXManager fXManagerScript;

    [HideInInspector]
    public bool bufferedShot = false;

    private bool shooting = false;
    public int burstShots = 3;
    public float burstShotDelay = 0.1f;
    public ReticleBehavior retScript;

    [Header("Accuracy Variables")]
    public float spreadPerShot = 1f;

    public float spreadRecoverRate = 1f;
    private float currentSpread = 0f;

    // Use this for initialization
    private void Awake() {
        fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
    }

    // Update is called once per frame
    private void Update() {
        if (bufferedShot == true && shooting == false) {
            shooting = true;
            //bufferedShot = false;
            for (int i = 0; i < burstShots; i++) {
                Invoke("FireBullet", burstShotDelay * i);
            }
            Invoke("ResetShooting", burstShotDelay * burstShots);
        }

        RaycastHit hit;

        if (Physics.Raycast(shotTransform.position, shotTransform.forward, out hit)) {
            if (hit.transform.tag == "Enemy") {
                retScript.LockOn();
            } else
                retScript.LockOff();
        }

        currentSpread = Mathf.Lerp(currentSpread, 0, Mathf.Clamp01(Time.deltaTime * spreadRecoverRate));
    }

    public void Shoot() {
        bufferedShot = true;
    }

    public bool GetBufferedShot() {
        return bufferedShot;
    }

    private void ResetShooting() {
        shooting = false;
        bufferedShot = false;
    }

    private void FireBullet() {
        /*
        Rigidbody newBullet = Instantiate(bullet, shotTransform.position, shotTransform.rotation) as Rigidbody;
        newBullet.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
        */
        //Use Accuracy Here:
        shotTransform.Rotate(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0);
        fXManagerScript.CallPlayerBullet(shotTransform.position, shotTransform.rotation, shotVelocity);
        retScript.ShotTaken();
        shotTransform.localRotation = Quaternion.identity;
        currentSpread += spreadPerShot;
    }
}