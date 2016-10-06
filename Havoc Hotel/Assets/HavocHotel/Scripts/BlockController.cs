using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float m_fOverworldSpeed = 10.0f;
    public float m_fOverworldMaxSpeed = 50.0f;
    float m_fTimer = 0;
    public float m_fSpawnTimer = 1.0f;

    public GameObject[] m_LevelChunk;

    private int m_fCounter = 0;
    // Use this for initialization
    void Start()
    {
        m_fCounter = 0;
        Spawn();
        //m_fOverworldSpeed = 0.0f;

        //transform.position = transform.position - Vector3.up * /*fSpeed*/m_fOverworldSpeed;
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


    }

    void OnTriggerExit(Collider other)
    {

        Debug.Log(other.tag);
        if (other.tag == "Level")
        {
            Debug.Log("Level is true");
            Spawn();
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

        }
        else
        {
            go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
        }


      //  if (m_fCounter == 2)
      //  {
      //      int iRandIndex = Random.Range(0, m_LevelChunk.Length);
      //      GameObject go = Instantiate(m_LevelChunk[iRandIndex], transform.position, Quaternion.identity) as GameObject;
      //      go.GetComponent<MoveLevel>().refController = this;
      //      if (m_fOverworldSpeed <= m_fOverworldMaxSpeed)
      //      {
      //          go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
      //          go.GetComponent<MoveLevel>().refController = this;
      //      }
      //      else
      //      {
      //          go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
      //      }
      //  }
      //  else
      //  {
      //      GameObject go = Instantiate(m_LevelChunk[m_fCounter], transform.position, Quaternion.identity) as GameObject;
      //      go.GetComponent<MoveLevel>().refController = this;
      //      m_fCounter++;
      //      if (m_fOverworldSpeed <= m_fOverworldMaxSpeed)
      //      {
      //
      //          go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
      //          go.GetComponent<MoveLevel>().refController = this;
      //      }
      //      else
      //      {
      //          go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOverworldSpeed;
      //      }
      //  }
    }
}









