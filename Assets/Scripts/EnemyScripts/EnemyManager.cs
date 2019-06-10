using UnityEngine;

/// <summary>
/// This script is a manager that keeps track of how many enemies are alive,
/// then handles them respawning in waves.
/// It also passes on news of enemy death to the game manager,
/// which increments score and also changes game state.
/// </summary>

public class EnemyManager : MonoBehaviour {

    public GameObject enemy1Prefab;
    private GameObject[] enemy1Array;
    private const int enemy1Num = 10;
    private int enemy1Iter = 0;

    public GameObject skullPrefab;
    private GameObject[] skullArray;
    private const int skullNum = 3;
    private int skullIter = 0;

    private int totalEnemiesActive = 0;
    private int enemiesDestroyed = 0;
    private int bossesActive = 0;

    public LevelManager myLevelManager;

    private GameManager gameManager;

    private void Awake() {
        /// set up references, and call functions to instantiate arrays of all enemy types.
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InstantiateEnemy(enemy1Prefab, ref enemy1Array, enemy1Num);
        InstantiateEnemy(skullPrefab, ref skullArray, skullNum);
    }
    private void InstantiateEnemy(GameObject prefab, ref GameObject[] newArray, int size) {
        /// This function creates a new array filled with the gameObject prefab we specify,
        /// sets all the element's parents to the enemyManager, renames them, then sets them all to false.
        newArray = new GameObject[size];
        for (int i = 0; i < size; i++) {
            newArray[i] = GameObject.Instantiate(prefab);
            newArray[i].transform.SetParent(this.transform);
            newArray[i].name = newArray[i].name + i;
            newArray[i].SetActive(false);
        }
    }

    private void Start() {
        /// obsoleting this line for now since the LevelManager will handle when what get spawned.
        /// On start, we call the function to start spawning the first waves of enemies.
        /// 
        ///
        //Invoke("CreateEnemies", 1);
    }

    public void CreateEnemies() {
        /// According to how many enemies have been destroyed, we spawn more enemies.
        for (int i = 0; i <= enemiesDestroyed; i++) {
            if (i < enemy1Num) {
                SpawnEnemy1();
            }
        }
    }

    public void SpawnEnemy1() {
        /// We call the function to spawn the enemy using the array we
        /// instantiated, and the iterator we were using.
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 30,
            ref enemy1Array[enemy1Iter], ref enemy1Iter, enemy1Num);
        totalEnemiesActive++;
    }

    private void StartEnemy(Vector3 newPos, ref GameObject enemyObject, ref int iterator, int max) {
        /// Here we place the enemy in the position specified, set it to active, and then increase the iterator.
        /// The iterator is passed as a reference, so it modifies the original value.
        /// This function is re-usable for all types of enemies we want to spawn.
        enemyObject.transform.position = newPos;
        enemyObject.SetActive(true);
        iterator++;
        if (iterator >= max) {
            iterator = 0;
        }
    }

    public void SpawnSkullBoss() {
        /// Same as the  SpawnEnemy1 function, but for the skull boss enemy prefab and variables.
        bossesActive++;
        StartEnemy(Vector3.back * 20 + Vector3.right * Random.Range(-20, 20) + Vector3.up * 40,
                   ref skullArray[skullIter], ref skullIter, skullNum);
    }

    private void EnemyDied(int scoreAmount) {
        /// When an Enemy1 dies, it increments the amount of enemies killed
        /// both for the enemy manager and the game manager.
        /// If all enemies are destroyed, it spawns another wave.
        enemiesDestroyed += scoreAmount;
        totalEnemiesActive--;
        gameManager.UpdateEnemiesKilled(enemiesDestroyed);
        if (totalEnemiesActive == 0) {
            //Invoke("CreateEnemies", 2);
            myLevelManager.DroneWaveDestroyed();
        }
    }

    private void BossDied(int scoreAmount) {
        /// Same as for the Enemy1, but for the skull boss, and using slightly different respawning logic.
        bossesActive--;
        enemiesDestroyed += scoreAmount;
        gameManager.UpdateEnemiesKilled(enemiesDestroyed);
        myLevelManager.BossDestroyed();
        /*if (bossesActive == 0) {
            Invoke("SpawnSkullBoss", 6);
            if (enemiesDestroyed > 50)
                Invoke("SpawnSkullBoss", 6);
        }*/
    }
}