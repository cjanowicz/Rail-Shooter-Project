using UnityEngine;
using System.Collections;

public class EnemyMovementScript : MonoBehaviour {

    enum State { Chasing, Engaged, Dodging, Dead };
    State m_state;

    private float m_movementDamper = 0.02f;
    private Vector3 m_randomOffset;
    public float m_randomX =10;
    public float m_randomY = 5;
    public float m_randomZ = 5;
    public float m_speed = 10;

    public GameObject shipModel;
    private Rigidbody m_rigidbody;
    private GameObject m_fXManager;
    private float m_shootTimer = 3;

    private EnemyShootingScript m_shootingScript;

    public int healthMax = 3;
    private int health;

    private float forceFloat = 500.0f;

    void Awake() {

        m_rigidbody = GetComponent<Rigidbody>();
        m_fXManager = GameObject.Find("FXManager");
        m_shootingScript = GetComponent<EnemyShootingScript>();

        health = healthMax;
        m_state = State.Chasing;
    }
    void OnEnable() {
        //CancelInvoke
        health = healthMax;
        m_state = State.Chasing;
        InvokeRepeating("SetNewOffset", 0, Random.Range(2, 6));
        ResetRigidbody();
    }

    void SetNewOffset() {
        m_randomOffset = new Vector3(Random.Range(-m_randomX, m_randomX),
            Random.Range(-m_randomY, m_randomY),
            Random.Range(-m_randomZ, m_randomZ) + 30);

    }

    void Update() {

        switch (m_state) {
            case State.Engaged:
                m_shootTimer -= Time.deltaTime;
                if (m_shootTimer <= 0) {
                    FireAtPlayer();
                    m_shootTimer = Random.Range(3, 5);
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
        transform.position = Vector3.Lerp(transform.position, m_randomOffset, m_movementDamper);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((m_randomOffset + Vector3.forward*3) - transform.position), m_movementDamper);
        //m_rigidbody.AddRelativeForce(Vector3.forward * m_speed);
    }

    void FireAtPlayer() {
        m_shootingScript.FireAtPlayer();
    }

    void OnCollisionEnter(Collision collision) {
        if(m_state == State.Dead) {
            KillObject();
        }
    }

    void ApplyDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            if (m_state != State.Dead) {
                DeathSequence();
            } else {
                KillObject();
            }

        }
    }
    void DeathSequence() {
        InvokeRepeating("FXExplode", 0, 0.5f);
        FallDown();
        m_state = State.Dead;
        //This invoke is to stop the dead enemy from holding up the game if he dies but is unable to hit the ground
        Invoke("KillObject", 3);
    }

    void FallDown() {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
                                               Random.Range(0, forceFloat),
                                               Random.Range(-forceFloat, forceFloat)));
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
                                                Random.Range(-forceFloat, forceFloat),
                                                Random.Range(-forceFloat, forceFloat)));
    }

    void ResetRigidbody() {
        GetComponent<Rigidbody>().useGravity = false;
    }

    void KillObject() {
        m_fXManager.SendMessage("CallMediumExplosion", this.transform.position);
        transform.parent.SendMessage("EnemyDied");
        CancelInvoke();
        this.gameObject.SetActive(false);
    }

    void FXExplode() {
        m_fXManager.SendMessage("CallSmallExplosion", this.transform.position);
    }
    
}
