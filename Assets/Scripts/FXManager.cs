using UnityEngine;

/// <summary>
/// This script has variable groups for all different kinds of objects that need to be spawned and 
/// re-used quickly, such as explosions and bullets. The functions calls are unique to what is being called,
/// and the functions that instantiate and spawn the objects are all re-usable.
/// </summary>

public class FXManager : MonoBehaviour {
    public GameObject smallExpPrefab;
    private GameObject[] smallExpArray;
    private const int smallExpNum = 15;
    private int smallExpIter = 0;

    public GameObject medExpPrefab;
    private GameObject[] medExpArray;
    private const int medExpNum = 10;
    private int medExpIter = 0;

    public GameObject enemyHurtPrefab;
    private GameObject[] enemyHurtArray;
    private const int enemyHurtNum = 10;
    private int enemyHurtIter = 0;

    public GameObject playerMuzzleFlashPrefab;
    private GameObject[] playerMFlashArray;
    private const int pMFlashNum = 10;
    private int pMFlashIter = 0;

    public GameObject enemyMuzzleFlashPrefab;
    private GameObject[] enemyMFlashArray;
    private const int eMFlashNum = 10;
    private int eMFlashIter = 0;

    public GameObject playerBulletPrefab;
    private GameObject[] playerBulletArray;
    private const int playerBulletNum = 30;
    private int playerBulletIter = 0;

    public GameObject enemyBulletPrefab;
    private GameObject[] enemyBulletArray;
    private const int enemyBulletNum = 50;
    private int enemyBulletIter = 0;

    private void Awake() {
        /// Instantiate all the prefabs and related variables.
        InstantiateEffect(smallExpPrefab, ref smallExpArray, smallExpNum);
        InstantiateEffect(medExpPrefab, ref medExpArray, medExpNum);
        InstantiateEffect(enemyHurtPrefab, ref enemyHurtArray, enemyHurtNum);
        InstantiateEffect(playerMuzzleFlashPrefab, ref playerMFlashArray, pMFlashNum);
        InstantiateEffect(enemyMuzzleFlashPrefab, ref enemyMFlashArray, eMFlashNum);
        InstantiateEffect(playerBulletPrefab, ref playerBulletArray, playerBulletNum);
        InstantiateEffect(enemyBulletPrefab, ref enemyBulletArray, enemyBulletNum);
    }

    private void InstantiateEffect(GameObject prefab, ref GameObject[] newArray, int size) {
        /// Similar to the enemy manager, this instantiates an array of game objects
        /// and sets their parent transforms to this object.
        newArray = new GameObject[size];
        for (int i = 0; i < size; i++) {
            newArray[i] = GameObject.Instantiate(prefab);
            newArray[i].transform.SetParent(this.transform);
            newArray[i].SetActive(false);
        }
    }

    private void StartEffect(Vector3 newPos, ref GameObject effectObject, ref int iterator, int max) {
        /// this function is re-usable, and places a game object while incrementing the array iterator.
        effectObject.transform.position = newPos;
        effectObject.SetActive(true);
        iterator++;
        if (iterator >= max) {
            iterator = 0;
        }
    }

    public void CallSmallExplosion(Vector3 newPos) {
        StartEffect(newPos, ref smallExpArray[smallExpIter],
                    ref smallExpIter, smallExpArray.Length);
    }

    public void CallPlayerMuzzleFlash(Vector3 newPos, Quaternion newRotation) {
        StartEffect(newPos, ref playerMFlashArray[pMFlashIter],
                    ref pMFlashIter, playerMFlashArray.Length);
        playerMFlashArray[pMFlashIter].transform.rotation = newRotation;
    }

    public void CallEnemyMuzzleFlash(Vector3 newPos, Quaternion newRotation) {
        StartEffect(newPos, ref enemyMFlashArray[eMFlashIter],
                    ref eMFlashIter, enemyMFlashArray.Length);
        enemyMFlashArray[eMFlashIter].transform.rotation = newRotation;
    }

    public void CallMediumExplosion(Vector3 newPos) {
        StartEffect(newPos, ref medExpArray[medExpIter],
                    ref medExpIter, medExpArray.Length);
    }

    public void CallEnemyHurt(Vector3 newPos) {
        StartEffect(newPos, ref enemyHurtArray[enemyHurtIter],
                    ref enemyHurtIter, enemyHurtArray.Length);
    }

    public void CallPlayerBullet(Vector3 newPos, Quaternion newRotation, float bulletForce) {
        CallPlayerMuzzleFlash(newPos, newRotation);
        int currentIter = playerBulletIter;
        StartEffect(newPos, ref playerBulletArray[playerBulletIter],
                    ref playerBulletIter, playerBulletArray.Length);
        playerBulletArray[currentIter].transform.rotation = newRotation;
        playerBulletArray[currentIter].GetComponent<Rigidbody>().velocity = playerBulletArray[currentIter].transform.forward * bulletForce;
    }

    public void CallEnemyBullet(Vector3 newPos, Quaternion newRotation, float bulletForce) {
        CallEnemyMuzzleFlash(newPos, newRotation);
        int currentIter = enemyBulletIter;
        StartEffect(newPos, ref enemyBulletArray[enemyBulletIter],
                    ref enemyBulletIter, enemyBulletArray.Length);
        enemyBulletArray[currentIter].transform.rotation = newRotation;
        enemyBulletArray[currentIter].GetComponent<Rigidbody>().velocity = enemyBulletArray[currentIter].transform.forward * bulletForce;
    }
}