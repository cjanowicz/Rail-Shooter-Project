using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField]
	GameObject m_appManagerPrefab;
	private AppManager m_appScript;
	private bool m_gamePaused = false;

    enum State { Title, Normal, LevelTransition, BossFight, GameOver };
    State m_gameState;

    public GameObject groundObj;
	private AudioSource m_audioSource;
	
	public TextMesh m_scoreText;
	private int m_score = 0;
	public Transform m_dirLight;
	public Transform m_nightOrientation;
	private GameObject m_enemyManager;
	public UIManager m_UIManagerScript;
	public GameObject m_PauseMenuManager;
    // Use this for initialization
    private float m_lerpSpeed = 0.3f;
	void Awake () {
		GameObject tempAppManager = GameObject.Find ("AppManager(Clone)");
		if (tempAppManager == null) {
			tempAppManager = Instantiate(m_appManagerPrefab);
		}
		m_appScript = tempAppManager.GetComponent<AppManager>();

		if(m_scoreText == null){
			m_scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		}
		m_enemyManager = GameObject.Find("EnemyManager");
        //m_dirLight = GameObject.Find("DirectionalLight
        m_gameState = State.Normal;
		m_audioSource = GetComponent<AudioSource> ();
	}

	void Start(){
		m_UIManagerScript.StartFadeOut ();
	}
	
	// Update is called once per frame
	void Update () {
        switch (m_gameState) {
            case State.LevelTransition:
                m_dirLight.rotation = Quaternion.Slerp(
                m_dirLight.rotation, m_nightOrientation.rotation, Mathf.Clamp01(Time.deltaTime * m_lerpSpeed));
                break;
		}

		if(Input.GetButtonDown("Cancel")){
			m_gamePaused = !m_gamePaused;
			if(m_gamePaused){
				StartPause();
			} else{
				EndPause();
			}
		}
		
		if(m_gamePaused){
			
		}
	}

	public void UpdateEnemiesKilled(int amount){
		m_scoreText.text =  amount.ToString();
		m_score = amount;

		if(m_score > 15){
            if(m_gameState == State.Normal) {
                m_gameState = State.LevelTransition;
                Invoke("StopNightTransition", 15f);
            }
		}
	}


	public void StopNightTransition(){
		m_enemyManager.SendMessage("SpawnSkullBoss");
        m_gameState = State.BossFight;

	}

    void StartGameOver() {
        Debug.Log("m_Score == " + m_score);
		if (m_score > m_appScript.GetHighScore ()) {
			m_appScript.SetHighScore(m_score);
			m_appScript.SaveData();
        }
        Debug.Log("Set score == " + m_appScript.GetHighScore() + ", playerPrefs score = " + PlayerPrefs.GetInt("HighScore"));
        m_gameState = State.GameOver;
		m_UIManagerScript.StartFadeIn ();
		Invoke("LoadTitle", 4);

	}
	void LoadTitle(){
		m_appScript.LoadScene("TitleScene");
	}
	void RestartLevel() {
		m_appScript.LoadScene(Application.loadedLevel);
	}
	public void QuitGame(){
		m_appScript.QuitGame ();
	}

	
	void StartPause(){
		Time.timeScale = 0;
		m_PauseMenuManager.SetActive (true);
		m_audioSource.Pause ();

	}
	
	public void EndPause(){
		Debug.Log ("EndPause called");
		Time.timeScale = 1;
		m_PauseMenuManager.SetActive (false);
		m_audioSource.Play ();
	}
}
