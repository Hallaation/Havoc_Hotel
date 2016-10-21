using UnityEngine;
using System.Collections;

public class MoveLevel : MonoBehaviour
{   

    public BlockController refController;


    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (refController.m_bRunning)
        {
            transform.Translate(Vector3.down * refController.m_fOverworldSpeed * Time.deltaTime);
        }
    }


   

}
