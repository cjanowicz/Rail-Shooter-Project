using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject groundObj;
	
	public TextMesh m_scoreText;
	private int m_enemiesDestroyed = 0;
	public Transform m_dirLight;
	public Transform m_nightOrientation;
	bool m_nightTransition = false;
	private GameObject m_enemyManager;
	// Use this for initialization
	void Awake () {
		if(m_scoreText == null){
			m_scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		}
		m_enemyManager = GameObject.Find("EnemyManager");
		//m_dirLight = GameObject.Find("DirectionalLight

	}
	
	// Update is called once per frame
	void Update () {
		if(m_nightTransition == true){
			m_dirLight.rotation = Quaternion.Slerp(
			m_dirLight.rotation, m_nightOrientation.rotation, 0.001f);

		}
	}

	public void UpdateEnemiesKilled(int amount){
		m_scoreText.text =  amount.ToString();
		m_enemiesDestroyed = amount;

		if(m_enemiesDestroyed > 15){
			if(m_nightTransition == false){
			m_nightTransition = true;
			Invoke("StopNightTransition",15f);
			}
		}
	}

	public void StopNightTransition(){
		m_nightTransition = false;
		m_enemyManager.SendMessage("SpawnSkullBoss");

	}
}
