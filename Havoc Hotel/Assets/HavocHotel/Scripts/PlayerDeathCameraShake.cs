﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerDeathCameraShake : MonoBehaviour
{

    // Transform of the camera to shake. Grabs the gameObject's transform, a camera' transform reference.
    // if null.
    public Transform ref_camTransform;

    // How long the object should shake for.
    public float m_fShakeDuration = 0f;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float m_fShakeAmount = 0.7f;
    public float m_fDecreaseFactor = 1.0f;

    private bool m_bShakeCamera;
    Vector3 m_vOriginalPos;

    void Awake()
    {
        if (ref_camTransform == null)
        {
            ref_camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        m_vOriginalPos = ref_camTransform.localPosition;
    }



    // Update is called once per frame
    void Update()
    {

        if (m_bShakeCamera)
        {
            CameraShake();
        }
        else
        {
            ref_camTransform.localPosition = m_vOriginalPos;
        }
    }

    void CameraShake()
    {
        if (m_fShakeDuration > 0)
        {
            ref_camTransform.localPosition = m_vOriginalPos + Random.insideUnitSphere * m_fShakeAmount * Time.deltaTime;

            m_fShakeDuration -= Time.deltaTime * m_fDecreaseFactor;
        }
        else
        {
            m_fShakeDuration = 0f;
            ref_camTransform.localPosition = m_vOriginalPos;
        }
    }

    void OnTriggerEnter(Collider a_collider)
    {
        if (a_collider.tag == "Player")
        {
            m_bShakeCamera = true;
        }
    }

}





