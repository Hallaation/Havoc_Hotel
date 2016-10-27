using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerEnabler : MonoBehaviour
{
    public List<GameObject> m_playerList = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        m_playerList.Add(GameObject.Find("Player_001"));
        m_playerList.Add(GameObject.Find("Player_002"));
        m_playerList.Add(GameObject.Find("Player_003"));
        m_playerList.Add(GameObject.Find("Player_004"));
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void EnablePlayers()
    {
        foreach (GameObject item in m_playerList)
        {
            item.GetComponent<Movement>().refPlayerStartText.SetActive(true);
            item.GetComponent<Movement>().m_bGameRunning = true;
        }

    }
}
