using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class ReadyToPlay : MonoBehaviour
{

    public GameObject refOneMorePlayer;
    public GameObject refreadyToPlay;
    public GameObject refEnterInstruction;
    public GameObject refArrowsText;
    public GameObject[] refPlayers;
    private List<GameObject> playerList = new List<GameObject>();
    private UIManager refUiManager;
    private int m_iCounter = 0;

    // Use this for initialization
    void Start()
    {
        refUiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        m_iCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_iCounter >= 2 || (Debug.isDebugBuild && m_iCounter >= 1))
        {
            refArrowsText.SetActive(false);
            refOneMorePlayer.SetActive(false);
            refEnterInstruction.SetActive(false);
            refreadyToPlay.SetActive(true);
            foreach (GameObject player in playerList)
            {
                if (Input.GetButtonDown(player.GetComponent<Movement>().playerNumber + "_Start"))
                {
                    foreach (GameObject item in playerList)
                    {
                        item.GetComponent<Movement>().DontDestroyOnLoad();
                    }
                    SceneManager.LoadScene(3);
                }

            }

        }
        //one more player needed
        else if (m_iCounter == 1)
        {
            // refArrowsText.transform.position = new Vector3(refArrowsText.transform.position.x , 7, refArrowsText.transform.position.z);
            refArrowsText.SetActive(false);
            refOneMorePlayer.SetActive(true);
            refEnterInstruction.SetActive(false);
            refreadyToPlay.SetActive(false);
        }
        else
        {
            refArrowsText.SetActive(true);
            refEnterInstruction.SetActive(true);
            refOneMorePlayer.SetActive(false);
            refreadyToPlay.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider a_collision)
    {
        if (a_collision.tag == "Head")
        {
            if (!(playerList.Contains(a_collision.transform.parent.parent.gameObject)))
            {
                ++m_iCounter;
                playerList.Add(a_collision.transform.parent.parent.gameObject);
                a_collision.GetComponentInParent<Movement>().DestroyOnLoad = true;
            }
        }
    }

    /// <summary>
    /// Ontrigger exit, will look for a player's head collider. If it moves outside of the play box, it is no longer in play.
    /// </summary>
    /// <param name="a_collision"></param>
    void OnTriggerExit(Collider a_collision)
    {
        if (a_collision.tag == "Head")
        {
            if (playerList.Contains(a_collision.transform.parent.parent.gameObject))
            {
                --m_iCounter;
                playerList.Remove(a_collision.transform.parent.parent.gameObject);
                a_collision.GetComponentInParent<Movement>().DestroyOnLoad = false;
            }
        }
    }




}
