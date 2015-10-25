using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {


    public GameObject m_enemy1Prefab;
    private GameObject[] m_enemy1Array;
    private const int m_enemy1Num = 5;
    private int m_enemy1Iter = 0;

    private int m_totalEnemiesActive = 0;
    private int m_enemiesDestroyed = 0;

    // Use this for initialization
    void Awake() {
        InstantiateEnemy(m_enemy1Prefab, ref m_enemy1Array, m_enemy1Num);

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
        Debug.Log("Enemy spawned, name = " + m_enemy1Array[currentIter].name + ", total = " + m_totalEnemiesActive);
    }

    void EnemyDied() {
        m_enemiesDestroyed++;
        m_totalEnemiesActive--;
        Debug.Log("EnemyDied Called, destroyed = " + m_enemiesDestroyed + ", total = " + m_totalEnemiesActive);
        if (m_totalEnemiesActive == 0) {
            Invoke("CreateEnemies", 2);
        }
    }

    void Update() {
        if (Input.GetKeyDown("k")) {
            Debug.Log("LOG: destroyed = " + m_enemiesDestroyed + ", total = " + m_totalEnemiesActive);
        }
    }


}
