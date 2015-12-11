using UnityEngine;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    private GameObject
        m_appManagerPrefab;

    private AppManager m_appScript;
    private Transform m_selectCube;
    private Transform m_invertCube;
    private TextMesh m_scoreText;
    private const int m_numUI = 2;
    public Transform[] m_UIObj = new Transform[m_numUI];
    private string[] m_UIMethod = new string[m_numUI];
    private Vector3 m_selectOffset;
    public float m_selectSpeed = 10;
    private int m_UIIter = 0;
    private bool m_inputPressed;
    private SlowlyRotate m_selectRotateScript;
    public float m_newRotateSpeed = 20f;
    private TextMesh m_invertStateText;
    private bool m_settingsChanged = false;
    private int m_invert = -1;
    private int m_highScore = 0;
    private float m_lastRealTime = 0f;
    private float m_realTimeDelta = 0f;
    private GameManager m_gameManager;

    private void Awake() {
        m_selectCube = GameObject.Find("SelectCube").transform;
        m_selectOffset = m_selectCube.position - m_UIObj[0].position;
        m_selectRotateScript = m_selectCube.GetComponent<SlowlyRotate>();

        if (Application.loadedLevelName == "TitleScene") {
            m_invertCube = GameObject.Find("InvertCube").transform;
            m_scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
            m_invertStateText = GameObject.Find("InvertState").GetComponent<TextMesh>();
        }
        for (int i = 0; i < m_UIObj.Length; i++) {
            m_UIMethod[i] = m_UIObj[i].name;
        }
        GameObject tempAppManager = GameObject.Find("AppManager(Clone)");
        if (tempAppManager == null) {
            tempAppManager = Instantiate(m_appManagerPrefab);
        }
        m_appScript = tempAppManager.GetComponent<AppManager>();
        if (Application.loadedLevelName != "TitleScene") {
            m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void OnEnable() {
        m_lastRealTime = Time.realtimeSinceStartup;
    }

    private void Start() {
        if (Application.loadedLevelName == "TitleScene") {
            m_invert = m_appScript.GetInvert();
            m_highScore = m_appScript.GetHighScore();
            m_scoreText.text = m_highScore.ToString();

            if (m_invert == -1) {
                ChangeColor(m_invertCube, Color.green);
                m_invertStateText.text = "On";
            } else {
                ChangeColor(m_invertCube, Color.red);
                m_invertStateText.text = "Off";
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        m_realTimeDelta = Time.realtimeSinceStartup - m_lastRealTime;
        m_lastRealTime = Time.realtimeSinceStartup;

        if (Input.GetAxisRaw("Vertical") <= -0.1) {
            //Go Down
            if (m_inputPressed == false) {
                m_UIIter = (m_UIIter + 1) % m_UIObj.Length;
                m_inputPressed = true;
            }
        } else if (Input.GetAxisRaw("Vertical") >= 0.1) {
            //Go up
            if (m_inputPressed == false) {
                m_UIIter = m_UIIter - 1;
                if (m_UIIter < 0) {
                    m_UIIter = m_UIObj.Length - 1;
                }
                m_inputPressed = true;
            }
        } else {
            m_inputPressed = false;
        }

        if (Input.GetButtonDown("Fire1")) {
            SendMessage(m_UIMethod[m_UIIter]);
        }

        m_selectCube.position = Vector3.Lerp(m_selectCube.position,
                              m_UIObj[m_UIIter].position + m_selectOffset,
                                              Mathf.Clamp01(m_realTimeDelta * m_selectSpeed));
    }

    private void ChangeColor(Transform target, Color newColor) {
        target.GetComponent<Renderer>().material.color = newColor;
    }

    private void StartText() {
        m_selectRotateScript.SetSpeed(m_newRotateSpeed);
        if (m_settingsChanged) {
            m_appScript.SaveData();
        }
        Invoke("DelayLoadLevel", 1f);
    }

    private void DelayLoadLevel() {
        m_appScript.LoadScene("TestScene");
    }

    private void InvertText() {
        m_selectRotateScript.SetSpeed(m_newRotateSpeed);
        m_invertCube.SendMessage("SetSpeed", m_newRotateSpeed);

        m_invert *= -1;
        m_appScript.SetInvert(m_invert);

        if (m_invert == -1) {
            ChangeColor(m_invertCube, Color.green);
            m_invertStateText.text = "On";
        } else {
            ChangeColor(m_invertCube, Color.red);
            m_invertStateText.text = "Off";
        }

        m_settingsChanged = true;
    }

    private void ResumeText() {
        m_gameManager.EndPause();
    }

    private void QuitText() {
        m_gameManager.QuitGame();
        m_gameManager.LoadTitle();
    }
}