using UnityEngine;
using System.Collections;

public class SpriteWinMove : MonoBehaviour
{


    public Vector3 m_vMoveTowards;
    public float m_fSpeed;
    [Range(0.1f, 5.0f)]
    public float m_fSpeedReduction;
    // Use this for initialization
    void Start()
    {
		m_vMoveTowards = new Vector3(2.0f, 0.05f, 0.31f);
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-290, 0), ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.transform.localPosition.x <= m_vMoveTowards.x)
        //{
        //    Debug.Log("true");
        //
        //}
        //else
        //{
        //    if (m_fSpeed > 0.2f)
        //    {
        //        m_fSpeed -= m_fSpeedReduction * Time.deltaTime;
        //    }
        //    else if (m_fSpeed <= 0.2f)
        //    {
        //        m_fSpeed = 0.2f;
        //    }
        //    this.transform.localPosition -= new Vector3(m_fSpeed * Time.deltaTime, 0, 0);
        //}

        if (this.transform.localPosition.x <= m_vMoveTowards.x)
        {
            this.transform.localPosition = new Vector3(m_vMoveTowards.x, this.transform.localPosition.y, this.transform.localPosition.z);
        }
    }
}

