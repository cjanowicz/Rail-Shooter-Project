using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {


	public GameObject m_enemy1Prefab;
	private GameObject[] m_enemy1Array;
    private const int m_enemy1Num = 5;
    private int m_enemy1Iter = 0;

	
	public GameObject m_skullPrefab;
	private GameObject[] m_skullArray;
	private const int m_skullNum = 2;
	private int m_skullIter = 0;
	
	private int m_totalEnemiesActive = 0;
    private int m_enemiesDestroyed = 0;
	private int m_bossesActive = 0;

	private GameManager m_gameManager;


    // Use this for initialization
    void Awake() {
		m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		InstantiateEnemy(m_enemy1Prefab, ref m_enemy1Array, m_enemy1Num);
		InstantiateEnemy(m_skullPrefab, ref m_skullArray, m_skullNum);
		
    }

    void Start() {
        //I'm moving the functions that start the game going until the Start function
        //so that the game manager can turn the script on in order to 
        Invoke("CreateEnemies", 1);
    }
    void InstantiateEnemy(GameObject prefab, ref GameObject[] newArray, int size) {
        newArray = new GameObject[size];
        for (int i = 0; i < size; i++) {
            newArray[i] = GameObject.Instantiate(prefab);
            newArray[i].transform.SetParent(this.transform);
            newArray[i].name = newArray[i].name + i;
            newArray[i].SetActive(false);
        }
    }

    void StartEnemy(Vector3 newPos, ref GameObject enemyObject, ref int iterator, int max) {
        enemyObject.transform.position = newPos;
        enemyObject.SetActive(true);
        iterator++;
        if (iterator >= max) {
            iterator = 0;
        }
    }

    void CreateEnemies() {
        for (int i = 0; i <= m_enemiesDestroyed; i++) {
            if (i < m_enemy1Num) {
                SpawnEnemy1();
                m_totalEnemiesActive++;
            }
        }
    }

    void SpawnEnemy1() {
        int currentIter = m_enemy1Iter;
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 30,
            ref m_enemy1Array[currentIter], ref m_enemy1Iter, m_enemy1Num);
     }
	void SpawnSkullBoss() {
		int currentIter = m_skullIter;
		m_bossesActive++;
		StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 40,
		           ref m_skullArray[currentIter], ref m_skullIter, m_skullNum);
	}
	
	void EnemyDied(int scoreAmount) {
        m_enemiesDestroyed+= scoreAmount;
        m_totalEnemiesActive--;
		m_gameManager.UpdateEnemiesKilled(m_enemiesDestroyed);
        if (m_totalEnemiesActive == 0) {
			/////
		/// Note: There was a bug where the count of "Alive enemies"
			/// was getting out of sync due to enemies being called 
			/// back into operation just before they turned themselves off.
            Invoke("CreateEnemies", 2);
        }
    }

	void BossDied(int scoreAmount) {
        Debug.Log("Enemy Manager called Boss Died");
        m_bossesActive--;
		m_enemiesDestroyed+= scoreAmount;
		m_gameManager.UpdateEnemiesKilled(m_enemiesDestroyed);
        //TEMP: Increase number of bosses if total is greater than 50;

		if(m_bossesActive == 0){
			Invoke("SpawnSkullBoss", 6);
            if (m_enemiesDestroyed > 50)
                Invoke("SpawnSkullBoss", 6);
        }
	}

    void Update() {
    }
}
