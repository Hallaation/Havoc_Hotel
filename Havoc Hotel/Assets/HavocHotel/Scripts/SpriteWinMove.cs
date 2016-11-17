using UnityEngine;
using System.Collections;

public class SpriteWinMove : MonoBehaviour
{


    private Vector3 m_vMoveTowards;

    public float m_fSpeed;
    // Use this for initialization
    void Start()
    {
        m_vMoveTowards = new Vector3(0.194444444f, 0.05f, 0.31f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localPosition.x <= m_vMoveTowards.x)
        {
            Debug.Log("true");

        }
        else
        {
            this.transform.localPosition -= new Vector3(m_fSpeed * Time.deltaTime, 0, 0);
        }
    }
}

