using UnityEngine;
using System.Collections;

public class MoveLevel : MonoBehaviour
{
    public float m_fLevelSpeed = 1.0f; //-modifiable value for designers (modifiable in unity)- initial speed of level
    

    public BlockController refController; // refrance BlockController


    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (refController.m_bRunning) 
        {
            transform.Translate(Vector3.down * m_fLevelSpeed * Time.deltaTime); // move level
            m_fLevelSpeed = refController.m_fOverworldSpeed; // makes levelSpeed OBEY worldSpeed
        }
    }

    void OnTriggerEnter(Collider other)// when colliding with box collider at bottom  
    {
        if (other.tag == "Finish") // 
        {
            Destroy(this.gameObject); // destroy level blocks
        }

    }

   

}
