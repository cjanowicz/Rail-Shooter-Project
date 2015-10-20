using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour
{
	public Vector3[] positions;
	public Quaternion[] orientations;

	public Path next;
	public Path previous;

	public int Size() {
		return positions.Length;
	}

	public Quaternion Orientation(int index) {
		return orientations[index];
	}

	public Vector3 Position(int index) {
		return positions[index];
	}
}

