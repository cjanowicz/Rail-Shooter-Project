﻿using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform m_camTransform;

    // How long the object should shake for.
    public float m_shakeAmt = 0f;
    public float m_decreaseFactor = 1.0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float m_shakeStr = 0.7f;
    public float m_maxDist = 20f;

    Vector3 originalPos;

    void Awake() {
        m_camTransform = GetComponent<Transform>();
    }

    void OnEnable() {
        originalPos = m_camTransform.localPosition;
    }

    void Update() {
        if (m_shakeAmt > 0) {
            m_camTransform.localPosition = originalPos + Random.insideUnitSphere * m_shakeStr * m_shakeAmt;

            m_shakeAmt -= Time.deltaTime * m_decreaseFactor;
        } else {
            m_shakeAmt = 0f;
            m_camTransform.localPosition = originalPos;
        }
    }

	public void StartCameraShake(float incomingAmt, Vector3 origin){
        Vector3 relativeVec = origin - m_camTransform.position;
        float newAmt = Mathf.Clamp01((m_maxDist - relativeVec.magnitude) / m_maxDist) * incomingAmt;
        if (newAmt > m_shakeAmt) {
            m_shakeAmt = newAmt;
        }
	}
}