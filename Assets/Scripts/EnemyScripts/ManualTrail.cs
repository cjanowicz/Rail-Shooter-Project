﻿using UnityEngine;

/// <summary>
/// This behavior is not written by me, I found it on http://answers.unity3d.com/questions/1032656/trail-renderer-on-a-non-moving-object.html
/// I used it to quickly solve the problem of creating a visible trail for the player's wingtips,
/// when the conventional trail renderer does not easily work with stationary objects.
/// To solve this I found an easy solution for free on the internet, and studied their implementation.
/// </summary>

public class ManualTrail : MonoBehaviour {
    public int trailResolution;
    private LineRenderer lineRenderer;

    private Vector3[] lineSegmentPositions;
    private Vector3[] lineSegmentVelocities;

    // This would be the distance between the individual points of the line renderer
    public float offset;

    private Vector3 facingDirection;

    public enum LocalDirections { XAxis, YAxis, ZAxis }

    public LocalDirections localDirectionToUse;

    // How far the points 'lag' behind each other in terms of position
    public float lagTime;

    private Vector3 GetDirection() {
        switch (localDirectionToUse) {
            case LocalDirections.XAxis:
                return transform.right;

            case LocalDirections.YAxis:
                return transform.up;

            case LocalDirections.ZAxis:
                return transform.forward;
        }

        Debug.LogError("The variable 'localDirectionToUse' on the 'ManualTrail' script, located on object " + name + ", was somehow invalid. Please investigate!");
        return Vector3.zero;
    }

    // Use this for initialization
    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetVertexCount(trailResolution);

        lineSegmentPositions = new Vector3[trailResolution];
        lineSegmentVelocities = new Vector3[trailResolution];

        facingDirection = GetDirection();

        // Initialize our positions
        for (int i = 0; i < lineSegmentPositions.Length; i++) {
            lineSegmentPositions[i] = new Vector3();
            lineSegmentVelocities[i] = new Vector3();

            if (i == 0) {
                // Set the first position to be at the base of the transform
                lineSegmentPositions[i] = transform.position;
            } else {
                // All subsequent positions would be an offset of the original position.
                lineSegmentPositions[i] = transform.position + (facingDirection * (offset * i));
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        facingDirection = GetDirection();

        for (int i = 0; i < lineSegmentPositions.Length; i++) {
            if (i == 0) {
                // We always want the first position to be exactly at the original position
                lineSegmentPositions[i] = transform.position;
            } else {
                // All others will follow the original with the offset that you set up
                lineSegmentPositions[i] = Vector3.SmoothDamp(lineSegmentPositions[i], lineSegmentPositions[i - 1] + (facingDirection * offset), ref lineSegmentVelocities[i], lagTime);
            }

            // Once we're done calculating where our position should be, set the line segment to be in its proper place
            lineRenderer.SetPosition(i, lineSegmentPositions[i]);
        }
    }
}