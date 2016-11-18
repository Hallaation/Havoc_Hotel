using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class TrailDestroyer : MonoBehaviour
{

    TrailRenderer m_tRenderer;
    // Use this for initialization
    void Start()
    {
        m_tRenderer = GetComponent<TrailRenderer>();
        Debug.Log("Trailer destroyer was made");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount > 0)
        {
            //TrailRenderer temp = transform.FindChild("Dive_Kick_Trail(Clone)").GetComponent<TrailRenderer>();
            //Debug.Log(temp);
            Destroy(this.transform.gameObject, transform.FindChild("Dive_Kick_Trail(Clone)").GetComponent<TrailRenderer>().time);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }
}
