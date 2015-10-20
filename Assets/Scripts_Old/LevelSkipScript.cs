using UnityEngine;
using System.Collections;

public class LevelSkipScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("h"))
		{
			int level = Application.loadedLevel +1;
			if(level >= Application.levelCount)
				level = 0;
			Physics.gravity = new Vector3(0,-9.81f,0);
			Application.LoadLevel(level);
		}

	}
}
