using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class PlayerTextController : MonoBehaviour
{ 
    public List<Movement> refPlayers;
    private List<Movement> m_mDeadPlayers;

    private float m_fTimer;
    private float m_fWaitTime;
    public GameObject refWinMessage;

    private bool m_bIsFinished;

    public bool GameFinished { get { return m_bIsFinished; } set { m_bIsFinished = value; } }
    public float Timer { get { return m_fTimer; } set { m_fTimer = value; } }
    // Use this for initialization
    void Start()
    {
        m_bIsFinished = false;
        m_fWaitTime = 5.0f;
        m_mDeadPlayers = new List<Movement>();
    }

    void Awake()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            refPlayers.Add(item.GetComponent<Movement>());
        }
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
                foreach (Movement player in refPlayers)
                {
                    player.m_bIsDead = false;
                    player.transform.position = GameObject.Find("Player" + (player.GetComponent<Movement>().playerNumber + 1) + "_Spawn").transform.position;
                }
                foreach (Movement player in m_mDeadPlayers)
                {
                    player.m_bIsDead = false;
                    player.transform.position = GameObject.Find("Player" + (player.GetComponent<Movement>().playerNumber + 1) + "_Spawn").transform.position;
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

}

