using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour {
    private int highScore = 0;
    private int invert = -1;
    private float musicVol = 1f;
    private float fxVol = 1f;

    private void Awake() {
        if (PlayerPrefs.HasKey("HighScore") == false) {
            SaveData();
        } else {
            LoadData();
        }
    }

    public void SaveData() {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Invert", invert);
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("FXVol", fxVol);
        PlayerPrefs.Save();
    }

    private void LoadData() {
        highScore = PlayerPrefs.GetInt("HighScore");
        invert = PlayerPrefs.GetInt("Invert");
        musicVol = PlayerPrefs.GetFloat("MusicVol");
        fxVol = PlayerPrefs.GetFloat("FXVol");
    }

    public int GetInvert() {
        return invert;
    }

    public int GetHighScore() {
        return highScore;
    }

    public void SetHighScore(int newScore) {
        highScore = newScore;
    }

    public void SetInvert(int newInvert) {
        invert = newInvert;
    }

    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }

    public void QuitGame() {
        Application.Quit();
    }
}