using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject groundObj;
    public Transform dirLight;
    public Transform nightOrientation;
    public GameObject PauseMenuManager;
    public TextMesh scoreText;
    public UIManager UIManagerScript;

    [SerializeField]
    private GameObject appManagerPrefab;

    private AppManager appScript;
    private AudioSource audioSource;
    private GameObject enemyManager;
    private bool gamePaused = false;

    private State gameState;

    private float lerpSpeed = 0.3f;
    private int score = 0;

    private enum State { Title, Normal, LevelTransition, BossFight, GameOver };

    public void EndPause() {
        Time.timeScale = 1;
        PauseMenuManager.SetActive(false);
        audioSource.Play();
        gamePaused = false;
    }

    public void LoadTitle() {
        EndPause();
        appScript.LoadScene("TitleScene");
    }

    public void QuitGame() {
        EndPause();
        appScript.QuitGame();
    }

    public void StopNightTransition() {
        enemyManager.SendMessage("SpawnSkullBoss");
        gameState = State.BossFight;
    }

    public void UpdateEnemiesKilled(int amount) {
        scoreText.text = amount.ToString();
        score = amount;

        if (score > 15) {
            if (gameState == State.Normal) {
                gameState = State.LevelTransition;
                Invoke("StopNightTransition", 15f);
            }
        }
    }

    private void Awake() {
        GameObject tempAppManager = GameObject.Find("AppManager(Clone)");
        if (tempAppManager == null) {
            tempAppManager = Instantiate(appManagerPrefab);
        }
        appScript = tempAppManager.GetComponent<AppManager>();

        if (scoreText == null) {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
        }
        enemyManager = GameObject.Find("EnemyManager");
        gameState = State.Normal;
        audioSource = GetComponent<AudioSource>();
    }

    private void RestartLevel() {
        EndPause();
        appScript.LoadScene(Application.loadedLevel);
    }

    private void Start() {
        UIManagerScript.StartFadeOut();
    }

    private void StartGameOver() {
        if (score > appScript.GetHighScore()) {
            appScript.SetHighScore(score);
            appScript.SaveData();
        }
        gameState = State.GameOver;
        UIManagerScript.StartFadeIn();
        Invoke("LoadTitle", 4);
    }

    private void StartPause() {
        gamePaused = true;
        Time.timeScale = 0;
        PauseMenuManager.SetActive(true);
        audioSource.Pause();
    }

    private void Update() {
        switch (gameState) {
            case State.LevelTransition:
                dirLight.rotation = Quaternion.Slerp(
                dirLight.rotation, nightOrientation.rotation, Mathf.Clamp01(Time.deltaTime * lerpSpeed));
                break;
        }

        if (Input.GetButtonDown("Cancel")) {
            if (gamePaused == false) {
                StartPause();
            } else {
                EndPause();
            }
        }
    }
}