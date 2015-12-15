using UnityEngine;

public class PlayerHealthScript : MonoBehaviour {
    private PlayerAimMovement playerMovement;
    private PlayerInputScript playerInput;
    private CameraFollowScript camFollowScript;
    private CameraShake camShakeScript;
    private Rigidbody rigidbody;
    private GameObject fXManager;
    private GameObject gameManager;

    [SerializeField]
    private int maxHealth = 5;

    [SerializeField]
    private int health;

    private AudioSource audioSource;
    public AudioClip hurtSound;
    private Animator animator;
    public TextMesh healthText;

    private float forceFloat = 500.0f;
    private bool gameOver = false;
    public float hurtShakeAmt = 1;
    public float timeSlowAmt = 0.25f;
    public float invulnLength = 2;
    public int invulnFlashes = 3;

    // Use this for initialization
    private void Awake() {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (healthText == null) {
            healthText = GameObject.Find("HealthText").GetComponent<TextMesh>();
            UpdateHealthText();
        } else {
            UpdateHealthText();
        }
        playerMovement = GetComponent<PlayerAimMovement>();
        playerInput = GetComponent<PlayerInputScript>();
        rigidbody = GetComponent<Rigidbody>();
        fXManager = GameObject.Find("FXManager");
        gameManager = GameObject.Find("GameManager");
        GameObject camReference = GameObject.Find("CameraAnchor");
        camFollowScript = camReference.GetComponent<CameraFollowScript>();
        camShakeScript = camReference.GetComponent<CameraShake>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "World") {
            ApplyDamage();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "World") {
            ApplyDamage();
        } else if (other.tag == "Enemy") {
            ApplyDamage();
        }
    }

    private void ApplyDamage() {
        if (health > 1) {
            health--;
            audioSource.PlayOneShot(hurtSound);
            animator.SetTrigger("GotHurt");
            UpdateHealthText();
            camShakeScript.StartCameraShake(hurtShakeAmt, this.transform.position);
            fXManager.SendMessage("CallEnemyHurt", this.transform.position);
        } else {
            if (gameOver == false) {
                health--;
                UpdateHealthText();
                gameOver = true;
                playerMovement.enabled = false;
                playerInput.enabled = false;
                camFollowScript.enabled = false;
                GoLimp();
                InvokeRepeating("ExplodeRepeat", 0, 0.2f);
                Invoke("RestartLevel", 4);
                gameManager.SendMessage("StartGameOver");
            }
        }
        Time.timeScale = timeSlowAmt;
        Invoke("ResetTimeSlow", 0.01f);
    }

    private void ResetTimeSlow() {
        Time.timeScale = 1;
    }

    private void RestartLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void ExplodeRepeat() {
        fXManager.SendMessage("CallMediumExplosion", this.transform.position);
    }

    private void UpdateHealthText() {
        CancelInvoke();
        healthText.text = health.ToString();

        int times = 3;
        for (int i = 0; i < times; i++) {
            //Invoke("BlinkHealth");
        }
    }

    private void GoLimp() {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
                                         Random.Range(0, forceFloat),
                                         Random.Range(0, forceFloat)));
        rigidbody.AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
                                                  Random.Range(-forceFloat, forceFloat),
                                                  Random.Range(-forceFloat, forceFloat)));
    }

    private void BlinkHealth() {
    }
}