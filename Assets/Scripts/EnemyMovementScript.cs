using UnityEngine;
using System.Collections;

public class EnemyMovementScript : MonoBehaviour {

    enum State { Chasing, Engaged, Dodging, Dead };
    State m_state;

    private float m_movResponsive = 8f;
    private Vector3 m_randomOffset;
    public float m_randomX = 10;
    public float m_randomY = 5;
    public float m_randomZ = 5;
    public float m_speed = 10;
    public float m_zOffset = 30;

    public GameObject shipModel;
    private Rigidbody m_rigidbody;
    private GameObject m_fXManager;
    private float m_shootTimer = 3;
    public float m_shootMin = 3f;
    public float m_shootMax = 5f;
    public int m_scoreWorth = 1;
    public bool m_isBoss = false;
    public float m_burstDelay = 0.1f;
    public int m_burstNum = 10;

    private EnemyShootingScript m_shootingScript;

    private float forceFloat = 500.0f;

    void Awake() {

        m_rigidbody = GetComponent<Rigidbody>();
        m_fXManager = GameObject.Find("FXManager");
        m_shootingScript = GetComponent<EnemyShootingScript>();

        m_state = State.Chasing;
    }
    void OnEnable() {
        //CancelInvoke
        m_state = State.Chasing;
        InvokeRepeating("SetNewOffset", 0, Random.Range(2, 6));
        ResetRigidbody();
    }

    void SetNewOffset() {
        m_randomOffset = new Vector3(Random.Range(-m_randomX, m_randomX),
            Random.Range(-m_randomY, m_randomY),
            Random.Range(-m_randomZ, m_randomZ) + m_zOffset);

    }

    void Update() {

        switch (m_state) {
            case State.Engaged:
                m_shootTimer -= Time.deltaTime;
                if (m_shootTimer <= 0) {
                    if (m_isBoss == true) {
                        for (int i = 0; i < m_burstNum; i++) {
                            Invoke("BroadcastFire", i * m_burstDelay);
                        }

                    } else {
                        BroadcastFire();
                    }
                    m_shootTimer = Random.Range(m_shootMin, m_shootMax);
                }
                Move();

                break;
            case State.Chasing:

                Move();
                if (transform.position.z > m_randomOffset.z - 3) {
                    m_state = State.Engaged;
                }
                break;
        }
    }

    void Move() {
        transform.position = Vector3.Lerp(transform.position, m_randomOffset, Mathf.Clamp01(Time.deltaTime * m_movResponsive));
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((m_randomOffset + Vector3.forward * 10) - transform.position), Mathf.Clamp01(Time.deltaTime * m_movResponsive) );
        //m_rigidbody.AddRelativeForce(Vector3.forward * m_speed);
    }

    void BroadcastFire() {
        this.gameObject.BroadcastMessage("FireAtPlayer");
    }

    void OnCollisionEnter(Collision collision) {
        if (m_state == State.Dead) {
            KillObject();
        }
    }
    public void DeathSequence() {
        if (m_state != State.Dead) {
            InvokeRepeating("FXExplode", 0, 0.5f);
            FallDown();
            m_state = State.Dead;
            //This invoke is to stop the dead enemy from holding up the game if he dies but is unable to hit the ground
            Invoke("KillObject", 3);
        } else if (m_state == State.Dead) {
            KillObject();
        }
    }

    void FallDown() {
        m_rigidbody.useGravity = true;
        m_rigidbody.AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
                                               Random.Range(0, forceFloat),
                                               Random.Range(-forceFloat, forceFloat)));
        m_rigidbody.AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
                                                Random.Range(-forceFloat, forceFloat),
                                                Random.Range(-forceFloat, forceFloat)));
    }

    void ResetRigidbody() {
        GetComponent<Rigidbody>().useGravity = false;
    }

    void KillObject() {
        m_fXManager.SendMessage("CallMediumExplosion", this.transform.position);
        if (m_isBoss == false) {
            transform.parent.SendMessage("EnemyDied", m_scoreWorth);
        } else {

            transform.parent.SendMessage("BossDied", m_scoreWorth);
        }
        CancelInvoke();
        this.gameObject.SetActive(false);
    }

    void FXExplode() {
        m_fXManager.SendMessage("CallSmallExplosion", this.transform.position);
    }

}
