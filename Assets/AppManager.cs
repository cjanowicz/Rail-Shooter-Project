using UnityEngine;
using System.Collections;

public class AppManager : MonoBehaviour {
	///This script will handle scene transitions
	///and saving/loading data

	private int m_highScore = 0;
	private int m_invert = -1;
	private float m_musicVol = 1f;
	private float m_fxVol = 1f;

	// Use this for initialization
	void Awake () {
		if (Application.loadedLevelName == "InitScene") {
			LoadScene("TitleScene");
		}
		
		if (PlayerPrefs.HasKey ("HighScore") == false) {
			SaveData();
		} else {
			LoadData();
		}
	}

	public void SaveData(){
		PlayerPrefs.SetInt ("HighScore", m_highScore);
		PlayerPrefs.SetInt ("Invert", m_invert);
		PlayerPrefs.SetFloat ("MusicVol", m_musicVol);
		PlayerPrefs.SetFloat ("FXVol", m_fxVol);
		PlayerPrefs.Save ();
        Debug.Log("New High Score is set to " + PlayerPrefs.GetInt("HighScore"));
	}

	void LoadData(){
        Debug.Log("Data Loaded");
		m_highScore = PlayerPrefs.GetInt ("HighScore");
		m_invert = PlayerPrefs.GetInt ("Invert");
		m_musicVol = PlayerPrefs.GetFloat ("MusicVol");
		m_fxVol = PlayerPrefs.GetFloat ("FXVol");
	}

	public int GetInvert(){
		return m_invert;
	}
	public int GetHighScore(){
		return m_highScore;
	}
	public void SetHighScore(int newScore){
		m_highScore = newScore;
	}
	
	public void SetInvert(int newInvert){
		m_invert = newInvert;
	}
	public void LoadScene(string name){
		Application.LoadLevel (name);
	}
	public void LoadScene(int index){
		Application.LoadLevel(index);
	}

	public void QuitGame(){
		Application.Quit ();
	}



}
