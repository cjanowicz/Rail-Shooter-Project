using UnityEngine;

/// <summary>
/// This script handles enemy movement with a state machine.
/// It communicates with the enemy manager, the enemy shooting behavior, and the FX manager.
/// Methods to call:
///     DeathSequence(): Called by the EnemyHealthScript, to begin the deathsequence effects.
/// </summary>

public class DroneMovement : MonoBehaviour {

    public enum EnemyType { Drone, Tank, Boss };
    private enum State { Chasing, Engaged, Dodging, Dead };

    public EnemyType type;
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

    public float horizontalSpeed = 5;
    public float forwardSpeed = 5;
    public float deathEffectRepeatTime = 0.1f;
    public GameObject smokeEmitterObject;


    public float forceFloat = 500.0f;
    //public float deathRotationForce = 50.0f;

    private void Awake() {
        /// Set up references. 
        myRigidbody = GetComponent<Rigidbody>();
        fXManager = GameObject.Find("FXManager");
        shootingScript = GetComponent<EnemyShootingScript>();

        state = State.Chasing;
    }

    /// When enabled, the state and rigidbody both reset,
    /// and it starts a repeating function that determines where it places itself
    private void OnEnable() {
        state = State.Chasing;
        InvokeRepeating("SetNewOffset", 0, Random.Range(2, 6));
        ResetRigidbody();
    }

    private void SetNewOffset() {
        /// Determine a new location to randomly go to within bounds.
        randomOffset = new Vector3(Random.Range(-randomX, randomX),
            Random.Range(-randomY, randomY),
            Random.Range(-randomZ, randomZ) + zOffset);
    }

    /// According to the state machine, either...
    private void Update() {
        switch (type) {
            case EnemyType.Drone:
                switch (state) {
                    case State.Engaged:
                        /// Shoot according to a timer and move.
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
                        /// Or move from the start position to in front of the player,
                        /// then transition when we are in front of the player.
                        Move();
                        if (transform.position.z > randomOffset.z - 3) {
                            state = State.Engaged;
                        }
                        break;
                }
                break;
            case EnemyType.Tank:

                /*switch (state) {
                    case State.Engaged:
                        /// Shoot according to a timer and move.
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
                        /// Or move from the start position to in front of the player,
                        /// then transition when we are in front of the player.
                        Move();
                        if (transform.position.z > randomOffset.z - 3) {
                            state = State.Engaged;
                        }
                        break;
                }*/
                break;
        }
    }

    /// Here we move the enemy using a Lerp and interpolate its rotation.
    private void Move() {
        switch (type) {
            case EnemyType.Drone:
                transform.position = Vector3.Lerp(transform.position, randomOffset, Mathf.Clamp01(Time.deltaTime * movResponsive));
                transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((randomOffset + Vector3.forward * 10) - transform.position), Mathf.Clamp01(Time.deltaTime * movResponsive));
                break;
            case EnemyType.Tank:
                transform.position = Vector3.Lerp(transform.position, randomOffset, Mathf.Clamp01(Time.deltaTime * movResponsive));
                transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((randomOffset + Vector3.forward * 10) - transform.position), Mathf.Clamp01(Time.deltaTime * movResponsive));
                break;
        }
    }

   

    /// Send the "FireAtPlayer" message to all "EnemyShootingBehavior" components on the enemy character.
    /// This works for characters with more than one copy of the "EnemyShootingBehavior".
    private void BroadcastFire() {
        this.gameObject.BroadcastMessage("FireAtPlayer");
    }

    public void DeathSequence() {
        /// The health behavior calls this function, which first starts the character death sequence
        /// by making it play effects and call a function, then set the state. If this function is called
        /// while the state is dead, it immediately kills the object.
        if (state != State.Dead) {
            Invoke("FXExplode", deathEffectRepeatTime);
            FallDown();
            state = State.Dead;
        } else if (state == State.Dead) {
            KillObject();
        }
    }

    private void FXExplode() {
        /// This explosion effect continually calls a small explosion effect while the enemy is dying
        /// but stops repeating once the enemy is dead. 
        if (gameObject.activeSelf) {
            fXManager.SendMessage("CallSmallExplosion", this.transform.position);
            Invoke("FXExplode", deathEffectRepeatTime);
        }
    }

    private void FallDown() {
        /// This sets the character to go limp like a ragdoll and fly through the air. 
        myRigidbody.useGravity = true;
        //PositionPunch();
        RotationPunch();
        smokeEmitterObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision) {
        /// If the character is already dead, a collision immediately kills the object.
        if (state == State.Dead) {
            KillObject();
        }
    }

    public void RotationPunch() {
        /// Add torque to the rigidbody for damage feedback.
        myRigidbody.AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
                                                  Random.Range(-forceFloat, forceFloat),
                                                  Random.Range(-forceFloat, forceFloat)));
    }

    public void PositionPunch() {
        /// Adds a positional punch to the character for damage feedback.
        myRigidbody.AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
                                         Random.Range(0, forceFloat),
                                         Random.Range(-forceFloat, forceFloat)));
    }

    private void ResetRigidbody() {
        /// We reset values that were changed on this character's rigidbody when it last went through the deathSequence.
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void KillObject() {
        /// This function calls a bigger explosion from the FX manager
        /// as well as informs the enemy manager (its parents) that it died, then deactivates itself.
        if (gameObject.activeSelf) {
            fXManager.SendMessage("CallMediumExplosion", this.transform.position);
            if (isBoss == false) {
                transform.parent.SendMessage("EnemyDied", scoreWorth);
            } else {
                transform.parent.SendMessage("BossDied", scoreWorth);
            }
            smokeEmitterObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

}