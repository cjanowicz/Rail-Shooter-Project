using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

    public GameObject m_smallExplosionPrefab;
    private const int m_numOfSmallExplosions = 10;
    private int m_smallExpIter = 0;
    private GameObject[] m_smallExpArray;
    //public GameObject m_largeExplosionPrefab;

	
	public GameObject m_playerMuzzleFlash;
	private const int m_numPFlash = 3;
	private int m_pFlashIter = 0;
	private GameObject[] m_pFlashArray;

    // Use this for initialization
    void Awake () {
		InstantiateEffect(m_smallExplosionPrefab, ref m_smallExpArray, m_numOfSmallExplosions);
		InstantiateEffect(m_playerMuzzleFlash, ref m_pFlashArray, m_numPFlash);

    }
	void InstantiateEffect(GameObject prefab, ref GameObject[] newArray, int size){
		newArray = new GameObject[size];
		for(int i = 0; i < size; i++) {
			newArray[i] = GameObject.Instantiate(prefab);
			newArray[i].transform.SetParent(this.transform);
			newArray[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartEffect(Vector3 newPos, ref GameObject effectObject, ref int iterator, int max){
		effectObject.transform.position = newPos;
		effectObject.SetActive(true);
		iterator++;
		if(iterator >= max) {
			iterator = 0;
		}
	}

    void CallSmallExplosion(Vector3 newPos) {
		StartEffect(newPos, ref m_smallExpArray[m_smallExpIter], 
		            ref m_smallExpIter, m_smallExpArray.Length);
    }

	void CallPlayerMuzzleFlash(Vector3 newPos){
		StartEffect(newPos, ref m_pFlashArray[m_pFlashIter], 
		            ref m_pFlashIter, m_pFlashArray.Length);
	}


}
