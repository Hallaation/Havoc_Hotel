using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class PlayerTextController : MonoBehaviour
{
    public List<LouisMovement> refPlayers;
    private List<LouisMovement> m_mDeadPlayers;

    private float m_fTimer;
    private float m_fWaitTime;
    public GameObject refWinMessage;

    private bool m_bIsFinished;
    // Use this for initialization
    void Start()
    {
        m_bIsFinished = false;
        m_fWaitTime = 5.0f;
        m_mDeadPlayers = new List<LouisMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bIsFinished)
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

                    m_bIsFinished = true;
                }
            }
        }
        else
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer > m_fWaitTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}

