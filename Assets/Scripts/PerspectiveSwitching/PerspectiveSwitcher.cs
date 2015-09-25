﻿
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour {
    private Camera myCamera;

    void Awake() {
        myCamera = GetComponent<Camera>();
    }

    private Matrix4x4   ortho,
                        perspective;
    public float        fov     = 60f,
                        near    = .3f,
                        far     = 1000f,
                        orthographicSize = 50f;
    private float       aspect;
    private MatrixBlender blender;
    private bool        orthoOn;
 
    void Start()
    {
        aspect = (float) Screen.width / (float) Screen.height;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        myCamera.projectionMatrix = perspective;
        orthoOn = false;
        blender = (MatrixBlender) GetComponent(typeof(MatrixBlender));
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            orthoOn = !orthoOn;
            if (orthoOn)
                blender.BlendToMatrix(ortho, 1f);
            else
                blender.BlendToMatrix(perspective, 1f);
        }
    }
}