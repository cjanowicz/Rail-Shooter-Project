using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    enum State { Title, Normal, LevelTransition, BossFight, GameOver };
    State m_gameState;

    public GameObject groundObj;
	
	public TextMesh m_scoreText;
	private int m_enemiesDestroyed = 0;
	public Transform m_dirLight;
	public Transform m_nightOrientation;
	private GameObject m_enemyManager;
	// Use this for initialization
	void Awake () {
		if(m_scoreText == null){
			m_scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		}
		m_enemyManager = GameObject.Find("EnemyManager");
        //m_dirLight = GameObject.Find("DirectionalLight
        m_gameState = State.Normal;
	}
	
	// Update is called once per frame
	void Update () {
        switch (m_gameState) {
            case State.LevelTransition:
                m_dirLight.rotation = Quaternion.Slerp(
                m_dirLight.rotation, m_nightOrientation.rotation, 0.001f);
                break;
		}
	}

	public void UpdateEnemiesKilled(int amount){
		m_scoreText.text =  amount.ToString();
		m_enemiesDestroyed = amount;

		if(m_enemiesDestroyed > 15){
            if(m_gameState == State.Normal) {
                m_gameState = State.LevelTransition;
                Invoke("StopNightTransition", 15f);
            }
		}
	}
    

	public void StopNightTransition(){
		m_enemyManager.SendMessage("SpawnSkullBoss");
        m_gameState = State.BossFight;
        Debug.Log("Game Manager stopped night transition");

	}

    void StartGameOver() {
        m_gameState = State.GameOver;

    }
}
