using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float m_fOverworldSpeed = 5.0f; //all speed increase except acceleration 
    public float m_fOverworldMaxSpeed = 50.0f; //max speed overworld can go (for countering too high acceleration)
    public float m_fSpeedIncrease = 1.0f; //-modifiable value to change rate at which level increases in speed

    //float m_fTimer = 0; //meant to be used to help with block spawn problems 
    //public float m_fSpawnTimer = 1.0f;

    public GameObject[] m_LevelChunk;

    public bool m_bRunning;


    // Use this for initialization
    void Start()
    {
        m_bRunning = false;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {

        //m_fOverworldSpeed += Time.deltaTime;

        //m_fSpawnTimer /= 1.59f;
        //m_fTimer has to account for level speed change
        //   m_fTimer += Time.deltaTime;
        /*
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
        if (Input.GetKey(KeyCode.Return))
        {
            m_bRunning = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (this.tag == "Spawner")
        {
            if (other.tag == "Level")
            {
                Debug.Log(this.tag);
                Debug.Log("Spawn level");
                Spawn();
            }
        }

    }



    void Spawn()
    {

        int iRandIndex = Random.Range(0, m_LevelChunk.Length);
        GameObject go = Instantiate(m_LevelChunk[iRandIndex], transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<MoveLevel>().refController = this;
        if (m_fOverworldSpeed <= m_fOverworldMaxSpeed)
        {
            go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
            m_fOverworldSpeed += m_fSpeedIncrease;

        }
        else
        {
            go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldMaxSpeed;
        }


        // if (m_fCounter >= 2)
        // {
        //     int iRandIndex = Random.Range(0, m_LevelChunk.Length);
        //     GameObject go = Instantiate(m_LevelChunk[iRandIndex], transform.position, Quaternion.identity) as GameObject;
        //     go.GetComponent<MoveLevel>().refController = this;
        //     if (m_fOverworldSpeed <= m_fOverworldMaxSpeed)
        //     {
        //         go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
        //        // go.GetComponent<MoveLevel>().refController = this;
        //     }
        //     else
        //     {
        //         go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldMaxSpeed;
        //     }
        // }
        // else if (m_fCounter >= 2)
        // {
        //     GameObject go = Instantiate(m_LevelChunk[m_fCounter], transform.position, Quaternion.identity) as GameObject;
        //     go.GetComponent<MoveLevel>().refController = this;
        //     m_fCounter++;
        //     if (m_fOverworldSpeed <= m_fOverworldMaxSpeed)
        //     {
        //         go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
        //        // go.GetComponent<MoveLevel>().refController = this;
        //     }
        //     else
        //     {
        //         go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldMaxSpeed;
        //     }
        // }
    }
}









