using UnityEngine;

public class ShootingBehavior : MonoBehaviour {
    public float m_shotVelocity = 80.0f;
    public Transform shotTransform;

    private FXManager m_fXManagerScript;

    [HideInInspector]
    public bool bufferedShot = false;

    private bool shooting = false;
    public int burstShots = 3;
    public float burstShotDelay = 0.1f;
    public ReticleBehavior m_retScript;

    [Header("Accuracy Variables")]
    public float m_spreadPerShot = 1f;

    public float m_spreadRecoverRate = 1f;
    private float m_currentSpread = 0f;

    // Use this for initialization
    private void Awake() {
        m_fXManagerScript = GameObject.Find("FXManager").GetComponent<FXManager>();
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
                m_retScript.LockOn();
            } else
                m_retScript.LockOff();
        }

        m_currentSpread = Mathf.Lerp(m_currentSpread, 0, Mathf.Clamp01(Time.deltaTime * m_spreadRecoverRate));
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
        shotTransform.Rotate(Random.Range(-m_currentSpread, m_currentSpread), Random.Range(-m_currentSpread, m_currentSpread), 0);
        m_fXManagerScript.CallPlayerBullet(shotTransform.position, shotTransform.rotation, m_shotVelocity);
        m_retScript.ShotTaken();
        shotTransform.localRotation = Quaternion.identity;
        m_currentSpread += m_spreadPerShot;
    }
}