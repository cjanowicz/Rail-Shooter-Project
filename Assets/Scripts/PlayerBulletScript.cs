﻿using UnityEngine;
using System.Collections;

public class PlayerBulletScript : MonoBehaviour {

    private static GameObject m_fxManager;
    public int m_damage = 1;
    public float m_lifeTime = 2f;
    private AudioSource m_audioSource;
    private static CameraShake m_camShakeScript;
    public float m_shakeStrength = 0.2f;
    private bool firstActivation = true;
    private float m_timeSlowAmt = 0.25f;

    // Use this for initialization
    void Awake() {
        m_fxManager = GameObject.Find("FXManager");
        m_audioSource = GetComponent<AudioSource>();
        if (m_camShakeScript == null) {
            m_camShakeScript = GameObject.Find("CameraAnchor").GetComponent<CameraShake>();
        }
    }

    void OnEnable() {
        if(firstActivation == true) {
            firstActivation = false;
        } else {
            Invoke("Deactivate", m_lifeTime);
            m_audioSource.Play();
        }
    }

    void OnTriggerEnter(Collider other) {
		if (other.tag != "Powerup" && other.tag != "Player" && other.tag != "EnemyBullet") {
            other.SendMessage("ApplyDamage", m_damage, SendMessageOptions.DontRequireReceiver);
            DestroySelf();
            if(other.tag == "Enemy" || other.tag == "Boss") {
                Time.timeScale = m_timeSlowAmt;
                Invoke("ResetTimeSlow", 0.01f);
            }
        }
    }
    void ResetTimeSlow() {
        Time.timeScale = 1f;
    }

    void DestroySelf() {
        m_fxManager.SendMessage("CallSmallExplosion", this.transform.position);
        Deactivate();
        CancelInvoke("Deactivate");
    }

    void Deactivate() {
        this.gameObject.SetActive(false);
    }
}
