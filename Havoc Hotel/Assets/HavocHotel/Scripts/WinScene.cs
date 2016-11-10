using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class WinScene : MonoBehaviour
{
    private int temp;
    public List<Movement> refPlayers;
    // Use this for initialization
    void Start()
    {
      temp = PlayerPrefs.GetInt("Winner");
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            refPlayers.Add(item.GetComponent<Movement>());
            
        }
        for(int i = 0; i < refPlayers.Count; ++i)
        {
           if (refPlayers[i].playerNumber != temp)
            {
                refPlayers[i].m_bIsDead = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       


    }

    void OnTriggerEnter(Collider a_collision)
    {
       
    }

    /// <summary>
    /// Ontrigger exit, will look for a player's head collider. If it moves outside of the play box, it is no longer in play.
    /// </summary>
    /// <param name="a_collision"></param>
    void OnTriggerExit(Collider a_collision)
    {

    }




}
