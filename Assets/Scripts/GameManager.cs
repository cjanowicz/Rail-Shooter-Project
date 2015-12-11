using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject groundObj;
    public Transform m_dirLight;
    public Transform m_nightOrientation;
    public GameObject m_PauseMenuManager;
    public TextMesh m_scoreText;
    public UIManager m_UIManagerScript;

    [SerializeField]
    private GameObject m_appManagerPrefab;

    private AppManager m_appScript;
    private AudioSource m_audioSource;
    private GameObject m_enemyManager;
    private bool m_gamePaused = false;

    private State m_gameState;

    private float m_lerpSpeed = 0.3f;
    private int m_score = 0;

    private enum State { Title, Normal, LevelTransition, BossFight, GameOver };

    public void EndPause() {
        Time.timeScale = 1;
        m_PauseMenuManager.SetActive(false);
        m_audioSource.Play();
        m_gamePaused = false;
    }

    public void LoadTitle() {
        EndPause();
        m_appScript.LoadScene("TitleScene");
    }

    public void QuitGame() {
        EndPause();
        m_appScript.QuitGame();
    }

    public void StopNightTransition() {
        m_enemyManager.SendMessage("SpawnSkullBoss");
        m_gameState = State.BossFight;
    }

    public void UpdateEnemiesKilled(int amount) {
        m_scoreText.text = amount.ToString();
        m_score = amount;

        if (m_score > 15) {
            if (m_gameState == State.Normal) {
                m_gameState = State.LevelTransition;
                Invoke("StopNightTransition", 15f);
            }
        }
    }

    private void Awake() {
        GameObject tempAppManager = GameObject.Find("AppManager(Clone)");
        if (tempAppManager == null) {
            tempAppManager = Instantiate(m_appManagerPrefab);
        }
        m_appScript = tempAppManager.GetComponent<AppManager>();

        if (m_scoreText == null) {
            m_scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
        }
        m_enemyManager = GameObject.Find("EnemyManager");
        m_gameState = State.Normal;
        m_audioSource = GetComponent<AudioSource>();
    }

    private void RestartLevel() {
        EndPause();
        m_appScript.LoadScene(Application.loadedLevel);
    }

    private void Start() {
        m_UIManagerScript.StartFadeOut();
    }

    private void StartGameOver() {
        if (m_score > m_appScript.GetHighScore()) {
            m_appScript.SetHighScore(m_score);
            m_appScript.SaveData();
        }
        m_gameState = State.GameOver;
        m_UIManagerScript.StartFadeIn();
        Invoke("LoadTitle", 4);
    }

    private void StartPause() {
        m_gamePaused = true;
        Time.timeScale = 0;
        m_PauseMenuManager.SetActive(true);
        m_audioSource.Pause();
    }

    private void Update() {
        switch (m_gameState) {
            case State.LevelTransition:
                m_dirLight.rotation = Quaternion.Slerp(
                m_dirLight.rotation, m_nightOrientation.rotation, Mathf.Clamp01(Time.deltaTime * m_lerpSpeed));
                break;
        }

        if (Input.GetButtonDown("Cancel")) {
            if (m_gamePaused == false) {
                StartPause();
            } else {
                EndPause();
            }
        }
    }
}