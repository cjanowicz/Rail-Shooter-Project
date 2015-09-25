using UnityEngine;
using System.Collections;

//This line means that the script will execute even when in the editor
[ExecuteInEditMode]


public class VertexColors : MonoBehaviour {

	public Color objectColor = Color.white;
	int i = 0;

	// Update is called once per frame
	void Update () {
		//Get Mesh Data:
		//If you have objects inheriting a prefab, then you use sharedMesh
		//if you have a single object ( I imagine a single instance of the mesh) then you say ".mesh"
		Mesh objectMesh = GetComponent<MeshFilter>().mesh;

		//Create an array for colors
		Color[] colorsArray = new Color[objectMesh.vertices.Length];

		i = 0;
		while(i < objectMesh.vertices.Length) 
		{
			//sets new vertex color
			colorsArray[i] = objectColor;
			i++;
		}
		objectMesh.colors = colorsArray;

	}
}

/*
using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {
	void Start() {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Color[] colors = new Color[vertices.Length];
		int i = 0;
		while (i < vertices.Length) {
			colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
			i++;
		}
		mesh.colors = colors;
	}
}
*/