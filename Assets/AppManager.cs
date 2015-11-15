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
	void Start () {
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
	}

	void LoadData(){
		m_highScore = PlayerPrefs.GetInt ("HighScore");
		m_invert = PlayerPrefs.GetInt ("Invert");
		m_musicVol = PlayerPrefs.GetFloat ("MusicVol");
		m_fxVol = PlayerPrefs.GetFloat ("FXVol");
	}

	public int GetInvert(){
		return m_invert;
		Debug.Log ("Invert Got = " + m_invert);
	}
	public int GetHighScore(){
		return m_highScore;
	}
	public void SetHighScore(int newScore){
		m_highScore = newScore;
	}
	
	public void SetInvert(int newInvert){
		m_invert = newInvert;
		Debug.Log ("Invert  Set = " + m_invert);
	}
	public void LoadScene(string name){
		Application.LoadLevel (name);
		Debug.Log ("Invert When Loading = " + m_invert);
	}
	public void LoadScene(int index){
		Application.LoadLevel(index);
	}



}
