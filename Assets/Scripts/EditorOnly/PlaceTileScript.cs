using UnityEngine;
using System.Collections;

public class PlaceTileScript : MonoBehaviour {

	public GameObject tilePrefab;
	public Vector3 gridStart;
	
	
	public void BuildGrid()
	{
		GameObject newTile = Instantiate(tilePrefab, gridStart, Quaternion.identity) as GameObject;
		newTile.transform.parent = this.transform;
	}
}
