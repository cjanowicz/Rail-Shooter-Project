using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is for managing events relevant to the game state in the main gameplay scene,
/// and for communicating with the AppManager.
/// </summary>

public class GameManager : MonoBehaviour {

    public GameObject groundObj;
    public Transform dirLight;
    public Transform nightOrientation;
    public GameObject PauseMenuManager;
    public TextMesh scoreText;
    public FadeOutManager FadeOutScript;

    [SerializeField]
    private GameObject appManagerPrefab;

    private AppManager appScript;
    private AudioSource audioSource;
    private GameObject enemyManager;
    private bool gamePaused = false;

    private State gameState;

    private float lerpSpeed = 0.3f;
    private int score = 0;
    public int scoreToSpawnBoss = 15;

    private enum State { Title, Normal, LevelTransition, BossFight, GameOver };



    private void Awake() {
        /// In the Awake script, we set up our references.
        /// If the AppManager hasn't been spawned already, 
        /// we spawn a copy of our own that will remain persistent as long as the application is running.
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

    private void Start() {
        /// On the first frame, after the references are set up,
        /// we tell the fade-out manager to fade in the game.
        FadeOutScript.StartFadeOut();
    }

    private void Update() {
        /// Here, depending on the gameState variable,
        /// we use it to trigger running the code that rotates the directional light
        /// For the purpose of making the scene transition into night.
        switch (gameState) {
            case State.LevelTransition:
                dirLight.rotation = Quaternion.Slerp(
                dirLight.rotation, nightOrientation.rotation, Mathf.Clamp01(Time.deltaTime * lerpSpeed));
                break;
        }
        /// If we press the Escape or Start button, we pause the game.
        if (Input.GetButtonDown("Cancel")) {
            if (gamePaused == false) {
                StartPause();
            } else {
                EndPause();
            }
        }
    }

    private void StartPause() {
        /// Here we set the variables relevant to pausing the game.
        gamePaused = true;
        Time.timeScale = 0;
        PauseMenuManager.SetActive(true);
        audioSource.Pause();
    }

    public void EndPause() {
        /// Here we resume the gameplay 
        Time.timeScale = 1;
        PauseMenuManager.SetActive(false);
        audioSource.Play();
        gamePaused = false;
    }

    public void LoadTitle() {
        /// We call this function to tell the appManager to load the title scene.
        EndPause();
        appScript.LoadScene("TitleScene");
    }

    public void QuitGame() {
        //We call this function to quit the game.
        EndPause();
        appScript.QuitGame();
    }

    public void StopNightTransition() {
        /// At the end of the night transition, we tell the enemy manager to start spawning the skullBoss enemy.
        enemyManager.SendMessage("SpawnSkullBoss");
        gameState = State.BossFight;
    }

    public void UpdateEnemiesKilled(int amount) {
        /// Every time an enemy dies, it calls this function on the game manager,
        /// which increments the score counter.
        scoreText.text = amount.ToString();
        score = amount;

        /// If the score is greater than 15, we start a transition
        /// that will change the scene to night, and eventually tell the enemy manager
        /// to start spawning skull bosses.
        if (score > scoreToSpawnBoss) {
            if (gameState == State.Normal) {
                gameState = State.LevelTransition;
                Invoke("StopNightTransition", 15f);
            }
        }
    }

    private void RestartLevel() {
        /// Here we tell the app manager to restart the game at the current scene.
        EndPause();
        appScript.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void StartGameOver() {
        /// Here we check if the highscore needs to be overwritten, then do so if needed.
        if (score > appScript.GetHighScore()) {
            appScript.SetHighScore(score);
            appScript.SaveData();
        }

        /// After that, we change the game state, tell the fade-out to happen, then load the title scene in 4 seconds.
        gameState = State.GameOver;
        FadeOutScript.StartFadeIn();
        Invoke("LoadTitle", 4);
    }

    
}