using UnityEngine;
using System.Collections;

public class EnemyScript_FollowPath : MonoBehaviour {


    enum State { Chasing, Engaged, Dodging, Dead };
    State m_state;

    public GameObject shipModel;
    private Rigidbody m_rigidbody;
    private GameObject m_fXManager;

    //Moving
    private float timer = 0.0f;
    private float movingTimeMax = 5.0f;

    //Shooting
    public float shootTimerMax = 1.0f;
    public Transform enemyTransform;
    public Rigidbody bullet;
    public float velocity = 40.0f;
    public Transform bulletOrigin;
    private bool canShoot = false;

    //Health
    public int healthMax = 3;
    private int health;

    private float forceFloat = 500.0f;

    void Awake() {
        health = healthMax;
        m_state = State.Chasing;
        m_rigidbody = GetComponent<Rigidbody>();
        m_fXManager = GameObject.Find("FXManager");
    }

    // Update is called once per frame
    void Update() {

        switch (m_state) {
            case State.Chasing:
                

                break;
        }
        if (canShoot == true) {
            FireOnTarget(enemyTransform);
            canShoot = false;
        }

    }

    void OnCollisionEnter(Collision collision) {
        KillObject();
    }

    void FireOnTarget(Transform target) {
        bulletOrigin.LookAt(target.position);
        Rigidbody newBullet = Instantiate(bullet, bulletOrigin.position, bulletOrigin.rotation) as Rigidbody;
        newBullet.AddForce(newBullet.transform.forward * velocity, ForceMode.VelocityChange);
    }

    void ApplyDamage(int damage) {
        health -= damage;

        //if the enemy is destroyed, I want it to explode, then spew smoke and tumble out of the sky. 
        //Then when it hits another object, I want it to explode. 
        if (health <= 0) {
            if(m_state != State.Dead) {
                DeathSequence();

            } else {
                KillObject();
            }

        }
    }
    //have it tumble through the sky, remove constraints.
    void DeathSequence() {
        InvokeRepeating("FXExplode", 0, 0.5f);
        m_state = State.Dead;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
                                               Random.Range(0, forceFloat),
                                               Random.Range(-forceFloat, forceFloat)));
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
                                                Random.Range(-forceFloat, forceFloat),
                                                Random.Range(-forceFloat, forceFloat)));
    }

    void KillObject() {
        m_fXManager.SendMessage("CallMediumExplosion", this.transform.position);
        CancelInvoke();
        this.gameObject.SetActive(false);
    }

    void FXExplode() {
        m_fXManager.SendMessage("CallSmallExplosion", this.transform.position);
    }


    void OnEnable() {
        //CancelInvoke
        m_state = State.Chasing;
    }

    //Make it invisible, disable collision box.
    void MakeInvisible() {
        m_state = State.Dead;
        shipModel.GetComponent<Renderer>().enabled = false;
        timer = 0;

        //this.attachedRigidbody.useGravity = true;
    }
    //Re-enable collision box, put constraints back on.
    void MakeVisible() {
        shipModel.GetComponent<Renderer>().enabled = true;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        transform.position = Vector3.zero;
    }
}
