using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPurposeEnemyMovement : MonoBehaviour {

    public GameObject shipModel;
    private Rigidbody myRigidbody;
    private GameObject fXManager;
    private EnemyShootingScript shootingScript;

    public enum State { Positioning, Attacking, Dodging, Dead };
    public State myState;

    public float speed = 10;
    public float zOffset = 30;

    private float shootTimer = 3;
    public float shootMin = 3f;
    public float shootMax = 5f;
    public int scoreWorth = 1;
    public bool isBoss = false;
    public float burstDelay = 0.1f;
    public int burstNum = 10;

    public float maximumXSteeringSpeed = 3;

    // Use this for initialization
    void Start () {

        /// Set up references. 
        myRigidbody = GetComponent<Rigidbody>();
        fXManager = GameObject.Find("FXManager");
        shootingScript = GetComponent<EnemyShootingScript>();

        myState = State.Positioning;
    }

    private void OnEnable()
    {
        myState = State.Positioning;
        InvokeRepeating("SetNewOffset", 0, Random.Range(2, 6));
        ResetRigidbody();
    }

    private void ResetRigidbody()
    {
        /// We reset values that were changed on this character's rigidbody when it last went through the deathSequence.
        GetComponent<Rigidbody>().useGravity = false;
    }
    

    // Update is called once per frame
    void Update () {
		
	}
}
