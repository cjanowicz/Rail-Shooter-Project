using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {
    

    public GameObject m_smallExpPrefab;
    private GameObject[] m_smallExpArray;
    private const int m_smallExpNum = 15;
    private int m_smallExpIter = 0;
    //public GameObject m_largeExplosionPrefab;

    public GameObject m_medExpPrefab;
    private GameObject[] m_medExpArray;
    private const int m_medExpNum = 10;
    private int m_medExpIter = 0;

    public GameObject m_playerMuzzleFlashPrefab;
    private GameObject[] m_playerMFlashArray;
    private const int m_pMFlashNum = 3;
	private int m_pMFlashIter = 0;
    
    public GameObject m_playerBulletPrefab;
    private GameObject[] m_playerBulletArray;
    private const int m_playerBulletNum = 30;
    private int m_playerBulletIter = 0;

    public GameObject m_enemyBulletPrefab;
    private GameObject[] m_enemyBulletArray;
    private const int m_enemyBulletNum = 30;
    private int m_enemyBulletIter = 0;

    // Use this for initialization
    void Awake () {
		InstantiateEffect(m_smallExpPrefab, ref m_smallExpArray, m_smallExpNum);
        InstantiateEffect(m_medExpPrefab, ref m_medExpArray, m_medExpNum);
        InstantiateEffect(m_playerMuzzleFlashPrefab, ref m_playerMFlashArray, m_pMFlashNum);
        InstantiateEffect(m_playerBulletPrefab, ref m_playerBulletArray, m_playerBulletNum);
        InstantiateEffect(m_enemyBulletPrefab, ref m_enemyBulletArray, m_enemyBulletNum);

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
		StartEffect(newPos, ref m_playerMFlashArray[m_pMFlashIter], 
		            ref m_pMFlashIter, m_playerMFlashArray.Length);
	}

    public void CallMediumExplosion(Vector3 newPos) {
        StartEffect(newPos, ref m_medExpArray[m_medExpIter],
                    ref m_medExpIter, m_medExpArray.Length);
    }

    public void CallPlayerBullet(Vector3 newPos, Quaternion newRotation, float bulletForce) {
        int currentIter = m_playerBulletIter;
        StartEffect(newPos, ref m_playerBulletArray[m_playerBulletIter],
                    ref m_playerBulletIter, m_playerBulletArray.Length);
        m_playerBulletArray[currentIter].transform.rotation = newRotation;
        m_playerBulletArray[currentIter].GetComponent<Rigidbody>().velocity = m_playerBulletArray[currentIter].transform.forward * bulletForce;
    }
    public void CallEnemyBullet(Vector3 newPos, Quaternion newRotation, float bulletForce) {
        int currentIter = m_enemyBulletIter;
        StartEffect(newPos, ref m_enemyBulletArray[m_enemyBulletIter],
                    ref m_enemyBulletIter, m_enemyBulletArray.Length);
        m_enemyBulletArray[currentIter].transform.rotation = newRotation;
        m_enemyBulletArray[currentIter].GetComponent<Rigidbody>().velocity = m_enemyBulletArray[currentIter].transform.forward * bulletForce;
    }


}
