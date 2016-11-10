using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    //public Canvas refPopUpPanel; // ANY panel canvas that pops up.
    public GameObject refCreditsPanel;
    public GameObject refQuitPanel;
    public GameObject refPausePanel;
    public GameObject mainMenuPanel;
    public GameObject refCountDownTimer;
    public GameObject refWinPanel;
    private BlockController ref_BlockController;

    private List<GameObject> m_playerList = new List<GameObject>();

    private bool openPauseMenu; //bool to determin weather or not quit() was called from the pause menu or not

    private GameObject EventSystem;

    private bool m_bGameStarted = false;

    private bool m_PlayersInGameScene = false;

    private bool m_bResumeGame;

    private float m_fWaitTime = 5.0f;

    private float m_fTimer;


    public bool GameStarted { get { return m_bGameStarted; } set { m_bGameStarted = value; } }

    public bool PlayersInScene { get { return m_PlayersInGameScene; } set { m_PlayersInGameScene = value; } }
    // Use this for initialization
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            ref_BlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();
        }
        else
        {
            refWinPanel = GameObject.Find("WinPanel");
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            m_playerList.Add(item);
        }

    }

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            ref_BlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();
            resumePlayButton();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStarted && m_PlayersInGameScene)
        {
            if (Input.GetButtonDown("Cancel"))
            //if(Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }

            if (m_bResumeGame)
            {

                refCountDownTimer.GetComponent<Text>().text = ((int)m_fTimer).ToString();
                m_fTimer += Time.deltaTime;
                ref_BlockController.m_bIsPaused = false;
                GameObject.Find("Music").GetComponent<AudioSource>().UnPause();
                WaitTime();
                foreach (GameObject item in m_playerList)
                {
                    item.GetComponent<Movement>().m_bGameRunning = true;
                }
                m_bResumeGame = false;
                refCountDownTimer.SetActive(false);
            }
        }

    }


    public void LoadLevel()
    {
        SceneManager.LoadScene(0); // loads scene with index of zero (can find index of scene with the build settings)
    }


    public void LoadMainMenu()
    {
        foreach (GameObject player in m_playerList)
        {
            Destroy(player.GetComponent<Movement>());
            DestroyImmediate(player);
        }
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < go.Length; ++i)
        {
            DestroyImmediate(go[i]);
        }
        SceneManager.LoadScene(1);
    }



    public void play()
    {
        m_bGameStarted = true;
        mainMenuPanel.SetActive(false);
    }

    public void quitButton()
    {
        //if this is the game scene, make the quit text asking to return to main menu, otherwise run regular stuff which is to end the game.
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //If this is the game scene, load up the quit panel. close up the pause panel.
            refQuitPanel.SetActive(true);
            GameObject.Find("QuitText").GetComponent<Text>().text = "Are you sure you \nwant to return to main menu ? ";
            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("RealQuitButton"));
            refPausePanel.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            refQuitPanel.SetActive(true);
            GameObject.Find("QuitText").GetComponent<Text>().text = "Are you sure you \nwant to return to main menu ? ";
            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("RealQuitButton"));
            refWinPanel.SetActive(false);
        }
        else
        {
            //otherwise use the regular quit
            refQuitPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("RealQuitButton"));
        }
    }
    //if the player decides to press no on the quit confirmation
    public void declineQuitButton()
    {
        //if the current scene is the game scene, return the pause menu.
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            refQuitPanel.SetActive(false);
            refPausePanel.SetActive(true);
            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ResumeButton"));
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            refQuitPanel.SetActive(false);
            refWinPanel.SetActive(true);
            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("RestartButton"));
        }
        else
        {
            refQuitPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
        }

    }

    public void Quit()
    {
        //if the current scene is the game scene, load the main menu, otherwise quit the game.
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            LoadMainMenu();
        }
        else
        {
            Application.Quit();
        }

    }

    public void Restart()
    {
        GameObject.Find("PlayerController").GetComponent<PlayerTextController>().GameFinished = true;
        GameObject.Find("PlayerController").GetComponent<PlayerTextController>().Timer = 50000;
        //resumePlayButton();
    }

    public void RestartGame()
    {
        GameObject[] PlayerGos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in PlayerGos)
        {
            player.GetComponent<Movement>().m_bIsDead = false;
        }
        SceneManager.LoadScene(2);
    }

    public void Pause()
    {
        refPausePanel.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ResumeButton"));
        openPauseMenu = true;
        GameObject.Find("Music").GetComponent<AudioSource>().Pause();
        ref_BlockController.m_bIsPaused = false;

        foreach (GameObject item in m_playerList)
        {
            item.GetComponent<Movement>().m_bGameRunning = false;
        }
        Time.timeScale = 0;
    }

    public void resumePlayButton()
    {
        Time.timeScale = 1;
        refPausePanel.SetActive(false);
        openPauseMenu = false;

        m_bResumeGame = true;



    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSecondsRealtime(5);
    }
    public void Credits()
    {
        //  Debug.Log("credits");
        refCreditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("CreditsBackButton"));
    }
    public void creditsBack()
    {
        //  Debug.Log("cradits back");
        refCreditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    public void CloseMainMenu()
    {
        m_bGameStarted = true;
        mainMenuPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        for (int i = 0; i < m_playerList.Count; ++i)
        {
            DestroyImmediate(m_playerList[i]);
        }
        SceneManager.LoadScene(1);
    }
}

