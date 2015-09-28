using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlaceTileScript))]
public class PlaceTileEditorScript : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		PlaceTileScript myScript = (PlaceTileScript)target;
		if(GUILayout.Button("Build Grid"))
		{
			myScript.BuildGrid();
		}
	}
}