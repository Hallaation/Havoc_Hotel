using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ReadyToPlay : MonoBehaviour
{

    public GameObject refOneMorePlayer;
    public GameObject refreadyToPlay;

    private List<GameObject> playerList = new List<GameObject>();
    private int m_iCounter = 0;
    // Use this for initialization
    void Start()
    {
        m_iCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_iCounter >= 2)
        {
            refOneMorePlayer.SetActive(false);
            refreadyToPlay.SetActive(true);
            //joystick button 7
            if (Input.GetKeyDown("joystick button"))
            {
                foreach (GameObject item in playerList)
                {
                    DontDestroyOnLoad(item);
                }
            }

        }
        else if (m_iCounter == 1)
        {
            refOneMorePlayer.SetActive(true);
            refreadyToPlay.SetActive(false);
        }
        else
        {
            refOneMorePlayer.SetActive(false);
            refreadyToPlay.SetActive(false);
        }

    }

    void OnTriggerStay(Collider a_collision)
    {
        if (a_collision.tag == "Head")
        {
            ++m_iCounter;
            playerList.Add(a_collision.gameObject);
        }
    }

    void OnTriggerExit(Collider a_collision)
    {
        if (a_collision.tag == "Head")
        {
            --m_iCounter;
            if(playerList.Contains(a_collision.gameObject))
            {
                playerList.Remove(a_collision.gameObject);
            }
        }
    }



}
