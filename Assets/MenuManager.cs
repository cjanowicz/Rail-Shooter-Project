using UnityEngine;

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
    private string[] UIMethod = new string[numUI];
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

    private void Awake() {
        selectCube = GameObject.Find("SelectCube").transform;
        selectOffset = selectCube.position - UIObj[0].position;
        selectRotateScript = selectCube.GetComponent<SlowlyRotate>();

        if (Application.loadedLevelName == "TitleScene") {
            invertCube = GameObject.Find("InvertCube").transform;
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
            invertStateText = GameObject.Find("InvertState").GetComponent<TextMesh>();
        }
        for (int i = 0; i < UIObj.Length; i++) {
            UIMethod[i] = UIObj[i].name;
        }
        GameObject tempAppManager = GameObject.Find("AppManager(Clone)");
        if (tempAppManager == null) {
            tempAppManager = Instantiate(appManagerPrefab);
        }
        appScript = tempAppManager.GetComponent<AppManager>();
        if (Application.loadedLevelName != "TitleScene") {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void OnEnable() {
        lastRealTime = Time.realtimeSinceStartup;
    }

    private void Start() {
        if (Application.loadedLevelName == "TitleScene") {
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

    // Update is called once per frame
    private void Update() {
        realTimeDelta = Time.realtimeSinceStartup - lastRealTime;
        lastRealTime = Time.realtimeSinceStartup;

        if (Input.GetAxisRaw("Vertical") <= -0.1) {
            //Go Down
            if (inputPressed == false) {
                UIIter = (UIIter + 1) % UIObj.Length;
                inputPressed = true;
            }
        } else if (Input.GetAxisRaw("Vertical") >= 0.1) {
            //Go up
            if (inputPressed == false) {
                UIIter = UIIter - 1;
                if (UIIter < 0) {
                    UIIter = UIObj.Length - 1;
                }
                inputPressed = true;
            }
        } else {
            inputPressed = false;
        }

        if (Input.GetButtonDown("Fire1")) {
            SendMessage(UIMethod[UIIter]);
        }

        selectCube.position = Vector3.Lerp(selectCube.position,
                              UIObj[UIIter].position + selectOffset,
                                              Mathf.Clamp01(realTimeDelta * selectSpeed));
    }

    private void ChangeColor(Transform target, Color newColor) {
        target.GetComponent<Renderer>().material.color = newColor;
    }

    private void StartText() {
        selectRotateScript.SetSpeed(newRotateSpeed);
        if (settingsChanged) {
            appScript.SaveData();
        }
        Invoke("DelayLoadLevel", 1f);
    }

    private void DelayLoadLevel() {
        appScript.LoadScene("TestScene");
    }

    private void InvertText() {
        selectRotateScript.SetSpeed(newRotateSpeed);
        invertCube.SendMessage("SetSpeed", newRotateSpeed);

        invert *= -1;
        appScript.SetInvert(invert);

        if (invert == -1) {
            ChangeColor(invertCube, Color.green);
            invertStateText.text = "On";
        } else {
            ChangeColor(invertCube, Color.red);
            invertStateText.text = "Off";
        }

        settingsChanged = true;
    }

    private void ResumeText() {
        gameManager.EndPause();
    }

    private void QuitText() {
        gameManager.QuitGame();
        gameManager.LoadTitle();
    }
}