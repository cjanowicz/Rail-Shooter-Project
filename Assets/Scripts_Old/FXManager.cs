using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

    public GameObject m_smallExplosionPrefab;
    [SerializeField]
    private const int m_numOfSmallExplosions = 10;
    private int m_smallExpIter = 0;
    private GameObject[] m_smallExpArray;
    //public GameObject m_largeExplosionPrefab;

    // Use this for initialization
    void Awake () {
        m_smallExpArray = new GameObject[m_numOfSmallExplosions];
        for(int i = 0; i < m_numOfSmallExplosions; i++) {
            m_smallExpArray[i] = GameObject.Instantiate(m_smallExplosionPrefab);
            m_smallExpArray[i].transform.SetParent(this.transform);
            m_smallExpArray[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CallSmallExplosion(Vector3 newPos) {
        m_smallExpArray[m_smallExpIter].transform.position = newPos;
        m_smallExpArray[m_smallExpIter].SetActive(true);
        m_smallExpIter++;
        if(m_smallExpIter >= m_smallExpArray.Length) {
            m_smallExpIter = 0;
        }
    }
}
