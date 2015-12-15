using UnityEngine;

public class EnemyMovementScript : MonoBehaviour {

    private enum State { Chasing, Engaged, Dodging, Dead };

    private State state;

    private float movResponsive = 8f;
    private Vector3 randomOffset;
    public float randomX = 10;
    public float randomY = 5;
    public float randomZ = 5;
    public float speed = 10;
    public float zOffset = 30;

    public GameObject shipModel;
    private Rigidbody myRigidbody;
    private GameObject fXManager;
    private float shootTimer = 3;
    public float shootMin = 3f;
    public float shootMax = 5f;
    public int scoreWorth = 1;
    public bool isBoss = false;
    public float burstDelay = 0.1f;
    public int burstNum = 10;

    private EnemyShootingScript shootingScript;

    private float forceFloat = 500.0f;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody>();
        fXManager = GameObject.Find("FXManager");
        shootingScript = GetComponent<EnemyShootingScript>();

        state = State.Chasing;
    }

    private void OnEnable() {
        //CancelInvoke
        state = State.Chasing;
        InvokeRepeating("SetNewOffset", 0, Random.Range(2, 6));
        ResetRigidbody();
    }

    private void SetNewOffset() {
        randomOffset = new Vector3(Random.Range(-randomX, randomX),
            Random.Range(-randomY, randomY),
            Random.Range(-randomZ, randomZ) + zOffset);
    }

    private void Update() {
        switch (state) {
            case State.Engaged:
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0) {
                    if (isBoss == true) {
                        for (int i = 0; i < burstNum; i++) {
                            Invoke("BroadcastFire", i * burstDelay);
                        }
                    } else {
                        BroadcastFire();
                    }
                    shootTimer = Random.Range(shootMin, shootMax);
                }
                Move();

                break;

            case State.Chasing:

                Move();
                if (transform.position.z > randomOffset.z - 3) {
                    state = State.Engaged;
                }
                break;
        }
    }

    private void Move() {
        transform.position = Vector3.Lerp(transform.position, randomOffset, Mathf.Clamp01(Time.deltaTime * movResponsive));
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((randomOffset + Vector3.forward * 10) - transform.position), Mathf.Clamp01(Time.deltaTime * movResponsive));
        //myRigidbody.AddRelativeForce(Vector3.forward * speed);
    }

    private void BroadcastFire() {
        this.gameObject.BroadcastMessage("FireAtPlayer");
    }

    private void OnCollisionEnter(Collision collision) {
        if (state == State.Dead) {
            KillObject();
        }
    }

    public void DeathSequence() {
        if (state != State.Dead) {
            Invoke("FXExplode", 0.5f);
            FallDown();
            state = State.Dead;
            //This invoke is to stop the dead enemy from holding up the game if he dies but is unable to hit the ground
            //Invoke("KillObject", 3);
        } else if (state == State.Dead) {
            KillObject();
        }
    }

    private void FallDown() {
        myRigidbody.useGravity = true;
        PositionPunch();
        RotationPunch();
    }

    public void RotationPunch() {
        myRigidbody.AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
                                                  Random.Range(-forceFloat, forceFloat),
                                                  Random.Range(-forceFloat, forceFloat)));
    }

    public void PositionPunch() {
        myRigidbody.AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
                                         Random.Range(0, forceFloat),
                                         Random.Range(-forceFloat, forceFloat)));
    }

    private void ResetRigidbody() {
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void KillObject() {
        if (gameObject.activeSelf) {
            fXManager.SendMessage("CallMediumExplosion", this.transform.position);
            if (isBoss == false) {
                transform.parent.SendMessage("EnemyDied", scoreWorth);
            } else {
                transform.parent.SendMessage("BossDied", scoreWorth);
            }
            this.gameObject.SetActive(false);
        }
    }

    private void FXExplode() {
        if (gameObject.activeSelf) {
            fXManager.SendMessage("CallSmallExplosion", this.transform.position);
            Invoke("FXExplode", 0.5f);
        }
    }
}