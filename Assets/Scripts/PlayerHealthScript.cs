using UnityEngine;
using System.Collections;

public class PlayerHealthScript : MonoBehaviour {

    [SerializeField]
    private int m_maxHealth = 5;
    [SerializeField]
    private int m_health;
    private AudioSource m_audioSource;
    public AudioClip m_hurtSound;
    private Animator m_animator;
	public TextMesh m_healthText;

	private PlayerMovementScript m_playerMovement;
	private PlayerInputScript m_playerInput;
	public CameraFollowScript m_cameraScript;
	private Rigidbody m_rigidbody;
	private GameObject m_fXManager;
	private float forceFloat = 500.0f;



	// Use this for initialization
	void Awake () {
        m_health = m_maxHealth;
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
		if(m_healthText == null){
			m_healthText = GameObject.Find("HealthText").GetComponent<TextMesh>();
			UpdateHealtText();
		} else{
			UpdateHealtText();
		}
		m_playerMovement = GetComponent<PlayerMovementScript>();
		m_playerInput = GetComponent<PlayerInputScript>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_fXManager = GameObject.Find("FXManager");

    }

	void OnCollisionEnter(Collision collision) {
		if(collision.transform.tag == "World"){
			ApplyDamage();
		}
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "World") {
            ApplyDamage();
        } else if (other.tag == "Powerup") {
            ApplyDamage();
        } else if (other.tag == "Enemy") {
            ApplyDamage();
        }
    }

    void ApplyDamage() {
        m_health--;
        m_audioSource.PlayOneShot(m_hurtSound);
		m_animator.SetTrigger("GotHurt");
		UpdateHealtText();

		if(m_health <= 0){
			m_playerMovement.enabled = false;
			m_playerInput.enabled = false;
			m_cameraScript.enabled = false;
				GoLimp();
			InvokeRepeating("ExplodeRepeat",0,0.2f);
			Invoke("RestartLevel",3);
		} else if (m_health <= -15) {
			
			Application.LoadLevel(Application.loadedLevel);
		}
    }
	void RestartLevel(){
		Application.LoadLevel(Application.loadedLevel);
	}

	void ExplodeRepeat(){
		m_fXManager.SendMessage("CallMediumExplosion", this.transform.position);
	}

	void UpdateHealtText(){
		CancelInvoke();
		m_healthText.text = m_health.ToString();

		int times = 3;
		for(int i =0; i < times; i++){
			//Invoke("BlinkHealth");
		}
	}

	void GoLimp(){
		m_rigidbody.isKinematic = false;
			m_rigidbody.useGravity = true;
			m_rigidbody.AddForce(new Vector3(Random.Range(-forceFloat, forceFloat),
			                                 Random.Range(0, forceFloat),
			                                 Random.Range(0, forceFloat)));
			m_rigidbody.AddRelativeTorque(new Vector3(Random.Range(-forceFloat, forceFloat),
			                                          Random.Range(-forceFloat, forceFloat),
			                                          Random.Range(-forceFloat, forceFloat)));
	}

	void BlinkHealth(){

	}
}