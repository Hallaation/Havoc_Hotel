using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathCameraShake : MonoBehaviour
{

    // Transform of the camera to shake. Grabs the gameObject's transform, a camera' transform reference.
    // if null.
    public Transform ref_camTransform;

    // How long the object should shake for.
    public float m_fShakeDuration;
    private float m_fTimer;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float m_fShakeAmount = 0.7f;
    public float m_fDecreaseFactor = 1.0f;

    private bool m_bShakeCamera;
    Vector3 m_vOriginalPos;

    void Start()
    {
        m_fTimer = 0;
    }
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
            m_fTimer += Time.deltaTime;
            if (m_fTimer < m_fShakeDuration)
            {
                ref_camTransform.localPosition = m_vOriginalPos + Random.insideUnitSphere * m_fShakeAmount * Time.deltaTime;
            }
            else
            {
                m_bShakeCamera = false;
                ref_camTransform.localPosition = m_vOriginalPos;
                m_fTimer = 0;
            }
        }
        else
        {
            ref_camTransform.localPosition = m_vOriginalPos;
        }

    }

    void CameraShake()
    {

    }

    void OnTriggerEnter(Collider a_collider)
    {
        if (a_collider.tag == "Player")
        {
            a_collider.GetComponent<CharacterController>().enabled = false;
            m_bShakeCamera = true;
        }

    }

}





