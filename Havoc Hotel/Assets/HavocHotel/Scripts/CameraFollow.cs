using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject m_gObjectToFollow;
    [Range(0f , 50.0f)]
    public float m_fDistanceSlider;

    [Range(-10.0f , 10.0f)]
    public float m_fUpDistance;

    [Range(-10.0f , 10.0f)]
    public float m_fHorizontalDistance;

    [Range(-180 , 180)]
    public float m_fXTilt;


    [Range(-180, 180)]
    public float m_fYTilt;
    public bool m_bLookAt;
    private GameObject oldPosition;
    // Use this for initialization
    void Start()
    {
        oldPosition = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gObjectToFollow)
        {
            if (m_bLookAt)
            {
                this.transform.LookAt(m_gObjectToFollow.transform);
                this.transform.position = new Vector3(oldPosition.transform.position.x , oldPosition.transform.position.y , m_fDistanceSlider);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0 , 180 , 0));
                this.transform.position = new Vector3(m_gObjectToFollow.transform.position.x + m_fHorizontalDistance , m_gObjectToFollow.transform.position.y + m_fUpDistance , m_gObjectToFollow.transform.position.z + m_fDistanceSlider);
                this.transform.localRotation = Quaternion.Euler(new Vector3(m_fXTilt , m_fYTilt));
            }
        }
        else
        {
            this.transform.position = oldPosition.transform.position;
        }
    }
}
