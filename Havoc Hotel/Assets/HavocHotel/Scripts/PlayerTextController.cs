using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTextController : MonoBehaviour
{
    public List<LouisMovement> refPlayers;
    private List<LouisMovement> m_mDeadPlayers;


    public GameObject refWinMessage;
    // Use this for initialization
    void Start()
    {
        m_mDeadPlayers = new List<LouisMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < refPlayers.Count; ++i)
        {

            if (refPlayers[i].m_bIsDead && refPlayers[i] != null)
            {
                m_mDeadPlayers.Add(refPlayers[i]);
                refPlayers.RemoveAt(i);
            }
            if (refPlayers.Count <= 1 && m_mDeadPlayers.Count != 0)
            {
                refWinMessage.SetActive(true);
                refWinMessage.GetComponent<UnityEngine.UI.Text>().text = "Player " + (refPlayers[0].playerNumber + 1) + " has won!";
            }
        }


    }
}
