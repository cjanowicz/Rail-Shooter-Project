using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour
{
	// The current segment spans from the starting path segment to the
	// ending path segment. Usually, startPath == endPath and
	// (startSegment + 1) == endSegment. However, when traversing
	// between paths in a path chain, these conditions may not hold.
	private Path startPath;
	private int startIndex;
	private Path endPath;
	private int endIndex;

	// Interpolation constant for the current segment. t in [0,1].
	private float t;
	// Distance between the current and next frames of reference, bearing
	// in mind that this is dependent on the interpolation method. Here
	// we assume linear interpolation. The current distance along this
	// segment is thus (t * distance).
	private float distance;

	public Quaternion CurrentOrientation() {
		return Quaternion.Slerp(startPath.Orientation(startIndex),
		                        endPath.Orientation(endIndex),
		                        t);
	}

	public Vector3 CurrentPosition() {
		return Vector3.Slerp(startPath.Position(startIndex),
							 endPath.Position(endIndex),
							 t);
	}

	public float CurrentDistance() {
		return distance;
	}

	private float ComputeDistance() {
		return Vector3.Distance(startPath.Position(startIndex),
								endPath.Position(endIndex));
	}
	
	// Move the follower by a signed Euclidian distance. Returns false if
	// the movement was clipped due to the termination of the path, and
	// true otherwise.
	public bool Move(float amount) {
		t += amount / distance;

		// Check if the segment must be advanced.
		if (t > 1.0f) {
			float remaining = (t - 1.0f) * distance;
			Path prevEndPath = endPath;
			int prevEndIndex = endIndex;

			if (endIndex + 1 == endPath.Size()) {  // End of current path
				if (endPath.next == null) {  // Terminus
					t = 1.0f;
					return false;
				} else {  // Move onto next path
					endPath = endPath.next;
					endIndex = 0;
				}
			} else {  // Move within current path
				endIndex++;
			}

			startPath = prevEndPath;
			startIndex = prevEndIndex;
			distance = ComputeDistance();
			return Move(remaining);
		}

		// Check if the segment must be recessed.
		if (t < 0.0f) {
			float remaining = t * distance;
			Path prevStartPath = startPath;
			int prevStartIndex = startIndex;

			if (startIndex == 0) {  // Start of current path
				if (startPath.previous == null) {  // Terminus
					t = 0.0f;
					return false;
				} else {  // Move onto previous path
					startPath = startPath.previous;
					startIndex = startPath.Size() - 1;
				}
			} else {  // Move within current path
				startIndex--;
			}

			endPath = prevStartPath;
			endIndex = prevStartIndex;
			distance = ComputeDistance();
			return Move(remaining);
		}

		// Movement was within the current segment.
		return true;
	}
}

