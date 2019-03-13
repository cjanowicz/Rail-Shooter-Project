using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script acts as a start menu in the title scene, 
/// and acts as a pause menu in other scenes.
/// </summary>

public class MenuManager : MonoBehaviour {

    [SerializeField]
    private GameObject
        appManagerPrefab;

    private AppManager appScript;
    private Transform selectCube;
    private Transform invertCube;
    private TextMesh scoreText;
    private const int numUI = 2;
    public Transform[] UIObj = new Transform[numUI];
    private Vector3 selectOffset;
    public float selectSpeed = 10;
    private int UIIter = 0;
    private bool inputPressed;
    private SlowlyRotate selectRotateScript;
    public float newRotateSpeed = 20f;
    private TextMesh invertStateText;
    private bool settingsChanged = false;
    private int invert = -1;
    private int highScore = 0;
    private float lastRealTime = 0f;
    private float realTimeDelta = 0f;
    private GameManager gameManager;

    public AudioClip selectionSound;
    public AudioClip selectionNotValid;
    public AudioClip selectionConfirm;
    public AudioClip startSound;
    public AudioSource myAudioSource;


    private void Awake() {
        /// Here we set up references to existing objects in scenes,
        /// and depending on the scene name, we look for different game objects.
        selectCube = GameObject.Find("SelectCube").transform;
        selectOffset = selectCube.position - UIObj[0].position;
        selectRotateScript = selectCube.GetComponent<SlowlyRotate>();

        if (SceneManager.GetActiveScene().name == "TitleScene") {
            invertCube = GameObject.Find("InvertCube").transform;
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
            invertStateText = GameObject.Find("InvertState").GetComponent<TextMesh>();
        } else {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        
        /// We make a refernece to the appManager script in this sectino.
        /// If there is no appManager object in the scene, we make one and keep a reference to it.
        GameObject tempAppManager = GameObject.Find("AppManager(Clone)");
        if (tempAppManager == null) {
            tempAppManager = Instantiate(appManagerPrefab);
        }
        appScript = tempAppManager.GetComponent<AppManager>();
    }

    private void OnEnable() {
        /// When the game object is enabled, it starts recording its own time,
        /// because Time.DeltaTime is affected by Time.TimeScale, which is used to pause the game.
        lastRealTime = Time.realtimeSinceStartup;
    }

    private void Start() {
        /// After the awake function, if we are in the title scene,
        /// we set the values of the settings according to existing data, 
        /// which we get from the AppManager script reference.
        if (SceneManager.GetActiveScene().name == "TitleScene") {
            invert = appScript.GetInvert();
            highScore = appScript.GetHighScore();
            scoreText.text = highScore.ToString();

            if (invert == -1) {
                ChangeColor(invertCube, Color.green);
                invertStateText.text = "On";
            } else {
                ChangeColor(invertCube, Color.red);
                invertStateText.text = "Off";
            }
        }
    }

    private void Update() {
        /// Ever frame, we increment our time varaible,
        realTimeDelta = Time.realtimeSinceStartup - lastRealTime;
        lastRealTime = Time.realtimeSinceStartup;

        /// We check for player input, and increment the UIIterator.
        /// We also make sure that one press of the input axis 
        /// corellates to only one button press.
        if (Input.GetAxisRaw("Vertical") <= -0.1) {
            //Go Down
            if (inputPressed == false) {
                UIIter = (UIIter + 1) % UIObj.Length;
                inputPressed = true;
                myAudioSource.clip = selectionSound;
                myAudioSource.Play();
            }
        } else if (Input.GetAxisRaw("Vertical") >= 0.1) {
            //Go up
            if (inputPressed == false) {
                UIIter = UIIter - 1;
                if (UIIter < 0) {
                    UIIter = UIObj.Length - 1;
                }
                inputPressed = true;
                myAudioSource.clip = selectionSound;
                myAudioSource.Play();
            }
        } else {
            inputPressed = false;
        }

        /// If the player presses the fire button, we use SendMessage to call a method on this gameObject
        /// that has the same name as the game object that is in the array.
        if (Input.GetButtonDown("Fire1")) {
            SendMessage(UIObj[UIIter].name);
        }

        /// After we've checked for player input, we interpolate the selectCube game object's
        /// position to the location of the UI element we are seleting, plus an offset.
        selectCube.position = Vector3.Lerp(selectCube.position,
                              UIObj[UIIter].position + selectOffset,
                                              Mathf.Clamp01(realTimeDelta * selectSpeed));
    }

    private void ChangeColor(Transform target, Color newColor) {
        /// This function changes the color of the target object to a specified color.
        target.GetComponent<Renderer>().material.color = newColor;
    }

    private void StartText() {
        /// This function is called when we select the "StartText" option.
        /// We make the select cube spin, save the settings if we changed anything, and load the level after a short delay.
        selectRotateScript.SetSpeed(newRotateSpeed);


        /// Play Sound Effect:
        myAudioSource.clip = startSound;
        myAudioSource.Play();


        if (settingsChanged) {
            appScript.SaveData();
        }
        Invoke("DelayLoadLevel", 1f);
    }

    private void DelayLoadLevel() {
        /// After a short delay, we call this to tell the AppManager to load the main game scene.
        appScript.LoadScene("TestScene");
    }

    private void InvertText() {
        /// This function is called when we select the UI option for inverting player controls.
        /// First we set the speed on two objects in the scene.
        selectRotateScript.SetSpeed(newRotateSpeed);
        invertCube.SendMessage("SetSpeed", newRotateSpeed);

        /// Play Sound Effect:
        myAudioSource.clip = selectionConfirm;
        myAudioSource.Play();

        /// Then we change our invert settings by multiplying by -1,
        /// then set the AppManager's saved invert settings.
        invert *= -1;
        appScript.SetInvert(invert);

        /// According to what settings we use, we change the displayed feedback for the invert settings.
        if (invert == -1) {
            ChangeColor(invertCube, Color.green);
            invertStateText.text = "On";
        } else {
            ChangeColor(invertCube, Color.red);
            invertStateText.text = "Off";
        }

        /// Finally, we set the settings changed flag.
        settingsChanged = true;
    }

    private void ResumeText() {
        /// If we selected Resume as a UI option, we tell the GameManager object to unpause the game.
        gameManager.EndPause();
    }

    private void QuitText() {
        /// Quitting the game calls that function on the Game Manager from the main game scene.

        /// First we set the speed on two objects in the scene.
        selectRotateScript.SetSpeed(newRotateSpeed);
        invertCube.SendMessage("SetSpeed", newRotateSpeed);
        
        gameManager.QuitGame();
    }

    private void QuitGame()
    {

    }
}