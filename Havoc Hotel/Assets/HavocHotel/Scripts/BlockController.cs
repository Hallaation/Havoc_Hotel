using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float m_fOverworldSpeed = 5.0f; //start speed of levels
    public float m_fOverworldMaxSpeed = 50.0f; //max speed overworld can go (for countering too high acceleration)
    public float m_fSpeedIncrease = 1.0f; //-modifiable value to change rate at which level increases in speed
    public float m_fTimeStart = 0.0f; //time 'til level starts moving

    public GameObject go;

    public GameObject[] m_LevelChunk; // array to have list of modular level pieces

    public bool m_bRunning; // bool to control leel movent
    
    // Use this for initialization
    void Start()
    {
        m_bRunning = false;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("0_LB"))
        {
            m_bRunning = false;
            Debug.Log("Pressed");
        }
        else if (Input.GetButtonDown("0_RB"))
        {
            m_bRunning = true;
            Debug.Log("Pressed");
        }

        //m_fTimeStart -= Time.deltaTime;//use delta time to have seconds until bRunning is true
        //
        //if (m_fTimeStart <= 0)
        //{
        //    m_bRunning = true;
        //}
        if (m_bRunning)
        {
            if (m_fOverworldSpeed < m_fOverworldMaxSpeed) //if state to make max speed posible
            {

                m_fOverworldSpeed += m_fSpeedIncrease * Time.deltaTime;
            }
        }

    }

    void OnTriggerExit(Collider other) // when level block leave box collider activate
    {
        if (this.tag == "Player")
        {
            Debug.Log("Player is trying something");
        }
        if (this.tag == "Spawner") // talking to box directly with "Spawner" tag
        {
            if (other.tag == "Level") // talking level block with "Level" tag
            {
                Debug.Log(this.tag);       // things to see in console if this is activating
                Debug.Log("Spawn level");  // things to see in console if this is activating
                Spawn(); // activate Spawn() funct 
            }
        }

    }



    void Spawn()
    {
        int iRandIndex = Random.Range(0, m_LevelChunk.Length); // rand list to choose level block or in this case "chunk"

        m_LevelChunk[iRandIndex].GetComponent<MoveLevel>().refController = this; //Make it reference this.

        go = Instantiate(m_LevelChunk[iRandIndex], go.transform.position + Vector3.up * 8, Quaternion.identity) as GameObject; //pick a level "chunk" from rand list


    }
}







