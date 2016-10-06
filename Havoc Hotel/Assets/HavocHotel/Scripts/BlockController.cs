using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float m_fOtherSpeed;
    public float m_fOverworldSpeed;
    float m_fTimer = 0;
    public float m_fSpawnTimer = 1.0f;

    public GameObject[] m_LevelChunk;

    // Use this for initialization
    void Start()
    {

        //m_fOverworldSpeed = 0.0f;

        //transform.position = transform.position - Vector3.up * /*fSpeed*/m_fOverworldSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        m_fOverworldSpeed += Time.deltaTime;
  
        //m_fSpawnTimer /= 1.59f;
        //m_fTimer has to account for level speed change
        //   m_fTimer += Time.deltaTime;
        m_fTimer += Time.deltaTime;
        if (m_fTimer >= m_fSpawnTimer)
        {
            m_fTimer -= m_fSpawnTimer;
            Debug.Log("Spawn something");



            int iRandIndex = Random.Range(0, m_LevelChunk.Length - 1);
            GameObject go = Instantiate(m_LevelChunk[iRandIndex], transform.position, Quaternion.identity) as GameObject;
            //m_fOverworldSpeed += go.GetComponent<MoveLevel>().m_fSpeedIncrease;
            go.GetComponent<MoveLevel>().m_fLevelSpeed = m_fOtherSpeed;
            go.GetComponent<MoveLevel>().refController = this;
        }

    }
}









