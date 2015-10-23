using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {
    

    public GameObject m_smallExplosionPrefab;
    private GameObject[] m_smallExpArray;
    private const int m_numOfSmallExplosions = 15;
    private int m_smallExpIter = 0;
    //public GameObject m_largeExplosionPrefab;

    public GameObject m_medExplosionPrefab;
    private GameObject[] m_medExpArray;
    private const int m_numOfMedExp = 10;
    private int m_medExpIter = 0;

    public GameObject m_playerMuzzleFlash;
    private GameObject[] m_pFlashArray;
    private const int m_numPFlash = 3;
	private int m_pFlashIter = 0;
    
    public GameObject m_playerBullet;
    private GameObject[] m_pBulletArray;
    private const int m_numPBullet = 30;
    private int m_pBulletIter = 0;

    // Use this for initialization
    void Awake () {
		InstantiateEffect(m_smallExplosionPrefab, ref m_smallExpArray, m_numOfSmallExplosions);
        InstantiateEffect(m_medExplosionPrefab, ref m_medExpArray, m_numOfMedExp);
        InstantiateEffect(m_playerMuzzleFlash, ref m_pFlashArray, m_numPFlash);
        InstantiateEffect(m_playerBullet, ref m_pBulletArray, m_numPBullet);

    }
	void InstantiateEffect(GameObject prefab, ref GameObject[] newArray, int size){
		newArray = new GameObject[size];
		for(int i = 0; i < size; i++) {
			newArray[i] = GameObject.Instantiate(prefab);
			newArray[i].transform.SetParent(this.transform);
			newArray[i].SetActive(false);
		}
	}

	void StartEffect(Vector3 newPos, ref GameObject effectObject, ref int iterator, int max){
		effectObject.transform.position = newPos;
		effectObject.SetActive(true);
		iterator++;
		if(iterator >= max) {
			iterator = 0;
		}
	}

    public void CallSmallExplosion(Vector3 newPos) {
		StartEffect(newPos, ref m_smallExpArray[m_smallExpIter], 
		            ref m_smallExpIter, m_smallExpArray.Length);
    }

    public void CallPlayerMuzzleFlash(Vector3 newPos){
		StartEffect(newPos, ref m_pFlashArray[m_pFlashIter], 
		            ref m_pFlashIter, m_pFlashArray.Length);
	}

    public void CallMediumExplosion(Vector3 newPos) {
        StartEffect(newPos, ref m_medExpArray[m_medExpIter],
                    ref m_medExpIter, m_medExpArray.Length);
    }

    public void CallPlayerBullet(Vector3 newPos, Quaternion newRotation, float bulletForce) {
        int currentIter = m_pBulletIter;
        StartEffect(newPos, ref m_pBulletArray[m_pBulletIter],
                    ref m_pBulletIter, m_pBulletArray.Length);
        m_pBulletArray[currentIter].transform.rotation = newRotation;
        m_pBulletArray[currentIter].GetComponent<Rigidbody>().velocity = m_pBulletArray[currentIter].transform.forward * bulletForce;
    }


}
