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
    public BlockController ref_BlockController;
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
        WinnerCheck();
    }
    void WinnerCheck()
    {
        //constantly check if all players are dead.
        if (!m_bIsFinished)
        {
            for (int i = 0; i < refPlayers.Count; ++i)
            {
                if (refPlayers[i].m_bIsDead && refPlayers[i] != null)
                {
                    m_mDeadPlayers.Add(refPlayers[i]);
                    m_mDeadPlayers[m_mDeadPlayers.Count - 1].EmitDeathParticle();
                    refPlayers.RemoveAt(i);

                }
                if (refPlayers.Count <= 1 && m_mDeadPlayers.Count != 0)
                {
                    refWinMessage.SetActive(true);
                    refWinMessage.GetComponent<UnityEngine.UI.Text>().text = "Player " + (refPlayers[0].playerNumber + 1) + " has won!";
                    ref_BlockController.m_bIsPaused = true;
                    m_bIsFinished = true;
                    Time.timeScale = 0.7f;
                }
            }
        }
        else
        {
            //once finished this is where the player x wins message is shown. And then players are reset to certain positions. and reset their death status
            Time.timeScale -= Time.deltaTime;
            //once finished this is where the player x wins message is shown. And then players are reset to certain positions. and reset their death status
            if (Time.timeScale <= 0.1f)
            {
                //go through alive players (this is one player that is alive and won) and reset their position to a spawn point
                foreach (Movement player in refPlayers)
                {
                    player.m_bIsDead = false;
                }

                //reset the dead players and set their death status to false. this will allow the scene to automatically add them to the right list again. dont need this, do it at winner scene
                //foreach (Movement player in m_mDeadPlayers)
                //{
                //    if (player.GetComponent<Movement>().playerNumber > 3)
                //    {
                //        //player.transform.position = GameObject.Find("Player4_Spawn").transform.position;
                //        //Debug.Log("Reset player" + player.GetComponent<Movement>().playerNumber);
                //        player.GetComponent<Movement>().m_bIsDead = false;
                //    }
                //    else
                //    {
                //        //player.transform.position = GameObject.Find("Player" + (player.GetComponent<Movement>().playerNumber + 1) + "_Spawn").transform.position;
                //        //Debug.Log("Reset player" + player.GetComponent<Movement>().playerNumber);
                //        player.GetComponent<Movement>().m_bIsDead = false;
                //    }
                //}
                //PlayerPrefs.SetInt("Winner", refPlayers[0].playerNumber);

                SceneManager.LoadScene(4);
            }
        }
    }
}


