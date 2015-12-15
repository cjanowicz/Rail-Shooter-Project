using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public GameObject enemy1Prefab;
    private GameObject[] enemy1Array;
    private const int enemy1Num = 5;
    private int enemy1Iter = 0;

    public GameObject skullPrefab;
    private GameObject[] skullArray;
    private const int skullNum = 2;
    private int skullIter = 0;

    private int totalEnemiesActive = 0;
    private int enemiesDestroyed = 0;
    private int bossesActive = 0;

    private GameManager gameManager;

    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InstantiateEnemy(enemy1Prefab, ref enemy1Array, enemy1Num);
        InstantiateEnemy(skullPrefab, ref skullArray, skullNum);
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
        for (int i = 0; i <= enemiesDestroyed; i++) {
            if (i < enemy1Num) {
                SpawnEnemy1();
                totalEnemiesActive++;
            }
        }
    }

    private void SpawnEnemy1() {
        int currentIter = enemy1Iter;
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 30,
            ref enemy1Array[currentIter], ref enemy1Iter, enemy1Num);
    }

    private void SpawnSkullBoss() {
        int currentIter = skullIter;
        bossesActive++;
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 40,
                   ref skullArray[currentIter], ref skullIter, skullNum);
    }

    private void EnemyDied(int scoreAmount) {
        enemiesDestroyed += scoreAmount;
        totalEnemiesActive--;
        gameManager.UpdateEnemiesKilled(enemiesDestroyed);
        if (totalEnemiesActive == 0) {
            Invoke("CreateEnemies", 2);
        }
    }

    private void BossDied(int scoreAmount) {
        bossesActive--;
        enemiesDestroyed += scoreAmount;
        gameManager.UpdateEnemiesKilled(enemiesDestroyed);

        if (bossesActive == 0) {
            Invoke("SpawnSkullBoss", 6);
            if (enemiesDestroyed > 50)
                Invoke("SpawnSkullBoss", 6);
        }
    }
}