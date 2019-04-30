using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is meant to handle all scene loading, transitions, and application functions
/// as well as handle the saving a loading of persistent data.
/// Methods to call:
///     SaveData(): saves the data to the UserPrefs.
///     LoadData(): loads data from the UserPrefs.
///     int GetInvert(): returns the current invert settings.
///     int GetHighscore(): returns the current high score.
///     SetHighScore(int newScore): sets the new high score.
///     SetInvert(int newInvert): sets the invert settings.
///     LoadScene(string name || int index): load the specified scene.
///     QuitGame(): Calls the necessary functions to close the application.
/// </summary>

public class AppManager : MonoBehaviour {
    private int highScore = 0;
    private int invert = -1;
    private float musicVol = 1f;
    private float fxVol = 1f;

    private void Awake() {
        /// On awake, we check if save data exists by looking for "HighScore"
        /// If so, we load data. If not, we save the default values.
        if (PlayerPrefs.HasKey("HighScore") == false) {
            SaveData();
        } else {
            LoadData();
        }
    }

    public void SaveData() {
        /// Save the existing persistent data into the PlayerPrefs.
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Invert", invert);
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("FXVol", fxVol);
        PlayerPrefs.Save();
    }

    private void LoadData() {
        /// Read the persistent data into the local variables.
        highScore = PlayerPrefs.GetInt("HighScore");
        invert = PlayerPrefs.GetInt("Invert");
        musicVol = PlayerPrefs.GetFloat("MusicVol");
        fxVol = PlayerPrefs.GetFloat("FXVol");
    }

    public int GetInvert() {
        /// Return the invert from the local variable.
        return invert;
    }

    public int GetHighScore() {
        /// Return the high score from the local variable.
        return highScore;
    }

    public void SetHighScore(int newScore) {
        /// Set the high score.
        highScore = newScore;
    }

    public void SetInvert(int newInvert) {
        /// Set the invert.
        invert = newInvert;
    }
    
    public void LoadScene(string name) {
        /// Loads the scene that has the same name as the string argument.
        SceneManager.LoadScene(name);
    }

    public void LoadScene(int index) {
        /// Overloaded version of the function above, with an int argument.
        SceneManager.LoadScene(index);
    }

    public void QuitGame() {
        /// Function is called to quit the game.
        Debug.Log("AppManager Asked to quit game");
        Application.Quit();
    }
}