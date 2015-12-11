using UnityEngine;

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

    private void Awake() {
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InstantiateEnemy(m_enemy1Prefab, ref m_enemy1Array, m_enemy1Num);
        InstantiateEnemy(m_skullPrefab, ref m_skullArray, m_skullNum);
    }

    private void Start() {
        Invoke("CreateEnemies", 1);
    }

    private void InstantiateEnemy(GameObject prefab, ref GameObject[] newArray, int size) {
        newArray = new GameObject[size];
        for (int i = 0; i < size; i++) {
            newArray[i] = GameObject.Instantiate(prefab);
            newArray[i].transform.SetParent(this.transform);
            newArray[i].name = newArray[i].name + i;
            newArray[i].SetActive(false);
        }
    }

    private void StartEnemy(Vector3 newPos, ref GameObject enemyObject, ref int iterator, int max) {
        enemyObject.transform.position = newPos;
        enemyObject.SetActive(true);
        iterator++;
        if (iterator >= max) {
            iterator = 0;
        }
    }

    private void CreateEnemies() {
        for (int i = 0; i <= m_enemiesDestroyed; i++) {
            if (i < m_enemy1Num) {
                SpawnEnemy1();
                m_totalEnemiesActive++;
            }
        }
    }

    private void SpawnEnemy1() {
        int currentIter = m_enemy1Iter;
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 30,
            ref m_enemy1Array[currentIter], ref m_enemy1Iter, m_enemy1Num);
    }

    private void SpawnSkullBoss() {
        int currentIter = m_skullIter;
        m_bossesActive++;
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 40,
                   ref m_skullArray[currentIter], ref m_skullIter, m_skullNum);
    }

    private void EnemyDied(int scoreAmount) {
        m_enemiesDestroyed += scoreAmount;
        m_totalEnemiesActive--;
        m_gameManager.UpdateEnemiesKilled(m_enemiesDestroyed);
        if (m_totalEnemiesActive == 0) {
            Invoke("CreateEnemies", 2);
        }
    }

    private void BossDied(int scoreAmount) {
        m_bossesActive--;
        m_enemiesDestroyed += scoreAmount;
        m_gameManager.UpdateEnemiesKilled(m_enemiesDestroyed);

        if (m_bossesActive == 0) {
            Invoke("SpawnSkullBoss", 6);
            if (m_enemiesDestroyed > 50)
                Invoke("SpawnSkullBoss", 6);
        }
    }
}