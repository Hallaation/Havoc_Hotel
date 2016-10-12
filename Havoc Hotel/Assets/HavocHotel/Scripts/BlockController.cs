using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float m_fOverworldSpeed = 5.0f; //start speed of levels
    public float m_fOverworldMaxSpeed = 50.0f; //max speed overworld can go (for countering too high acceleration)
    public float m_fSpeedIncrease = 1.0f; //-modifiable value to change rate at which level increases in speed
    public float m_fTimeStart = 0.0f; //time 'til level starts moving

    //float m_fTimer = 0; //meant to be used to help with block spawn problems 
    //public float m_fSpawnTimer = 1.0f;

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
        /*
                m_fOverworldSpeed += Time.deltaTime;

                m_fSpawnTimer /= 1.59f;
                m_fTimer has to account for level speed change
                //   m_fTimer += Time.deltaTime;

                m_fTimer += Time.deltaTime;
                if (m_fTimer >= m_fSpawnTimer)
                {
                    m_fTimer -= m_fSpawnTimer;
                    Debug.Log("Spawn something");



                    int iRandIndex = Random.Range(0, m_LevelChunk.Length);
                    GameObject go = Instantiate(m_LevelChunk[iRandIndex], transform.position, Quaternion.identity) as GameObject;
                    //m_fOverworldSpeed += go.GetComponent<MoveLevel>().m_fSpeedIncrease;
                    go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
                    go.GetComponent<MoveLevel>().refController = this;
                }
                */
                

        m_fTimeStart -= Time.deltaTime;//use delta time to have seconds until bRunning is true
        if (m_fTimeStart < 0)
        {
            m_bRunning = true;
        }


        // if (Input.GetKey(KeyCode.Return))
        // {
        //     m_bRunning = true;
        // }

        if (Input.GetKey(KeyCode.W))    // test power up stuff
        {                               // test power up stuff
            --m_fOverworldSpeed;        // test power up stuff
        }                               // test power up stuff
        if (Input.GetKey(KeyCode.S))    // test power up stuff
        {                               // test power up stuff
            ++m_fOverworldSpeed;        // test power up stuff
        }                               // test power up stuff
    }

    void OnTriggerExit(Collider other) // when level block leave box collider activate
    {
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
        GameObject go = Instantiate(m_LevelChunk[iRandIndex], transform.position, Quaternion.identity) as GameObject; //pick a level "chunk" from rand list
        go.GetComponent<MoveLevel>().refController = this; //refrance MoveLevel???????????????????????

        if (m_fOverworldSpeed < m_fOverworldMaxSpeed) //if state to make max speed posible
        {
            go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
            m_fOverworldSpeed += m_fSpeedIncrease;
        }
        else if(m_fOverworldSpeed > m_fOverworldMaxSpeed)
        {
            go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldMaxSpeed;//iff world speed more than max speed; make world speed max speed
        }
    }
}









