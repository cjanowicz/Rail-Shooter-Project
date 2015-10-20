using UnityEngine;
using System.Collections;

public class GroundScroll : MonoBehaviour {

	public static float groundSpeed = 200;
    public static float xSpeed = 1;
    [Range(0.0f, 1.0f)]
    public static float xDamper = 0.65f;
    delegate void MyDelegate();
    MyDelegate myDelegate;
    ScrollGridScript myScrollGrid;

    void Start() {
        if (transform.name == "GameManager") {
            myDelegate = UpdateX;
            myScrollGrid = GetComponent<ScrollGridScript>();
        } else {
            myDelegate = MoveSelf;
        }
    }

	// Update is called once per frame
	void Update () {
        myDelegate();
	}

    void MoveSelf() {
        transform.position += new Vector3(-xSpeed * xDamper, 0, -groundSpeed * Time.deltaTime);
        if (transform.position.z <= -30) 
            transform.position = new Vector3(Random.Range(-500, 500), 0, Random.Range(1200, 2000));
       
    }
    void UpdateX() {
        xSpeed = myScrollGrid.xSpeed;
    }
}
