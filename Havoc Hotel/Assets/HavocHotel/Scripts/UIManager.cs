using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    //public Canvas refPopUpPanel; // ANY panel canvas that pops up.
    public GameObject refCreditsPanel;
    public GameObject refQuitPanel;
    public GameObject refPausePanel;
    public GameObject mainMenuPanel;

    private BlockController ref_BlockController;

    private List<GameObject> m_playerList = new List<GameObject>();

    private bool openPauseMenu; //bool to determin weather or not quit() was called from the pause menu or not

    private GameObject EventSystem;

    private bool m_bGameStarted = false;

    private bool m_PlayersInGameScene = false;

    public bool GameStarted { get { return m_bGameStarted; } set { m_bGameStarted = value; } }

    public bool PlayersInScene { get { return m_PlayersInGameScene; } set { m_PlayersInGameScene = value; } }
    // Use this for initialization
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        ref_BlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            m_playerList.Add(item);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStarted && m_PlayersInGameScene)
        {
            if (Input.GetButtonUp("Cancel"))
            //if(Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }


    public void LoadLevel()
    {
        SceneManager.LoadScene(0); // loads scene with index of zero (can find index of scene with the build settings)
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }



    public void play()
    {
        m_bGameStarted = true;
        mainMenuPanel.SetActive(false);
    }

    public void quitButton()
    {
        // Debug.Log("quitpanel was called");
        refQuitPanel.SetActive(true);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("RealQuitButton"));
        //resume game code to make sure you can quit game
        //Debug.Log("resumed");
        //Time.timeScale = 1;
        refPausePanel.SetActive(false);
    }
    public void declineQuitButton()
    {
        // Debug.Log("decline quit was called");
        refQuitPanel.SetActive(false);

        if (openPauseMenu == true)
        {
            refPausePanel.SetActive(true);
        }

    }

    public void MainMenuQuit()
    {
        // Debug.Log("quitpanel was called");
        refQuitPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("RealQuitButton"));
        //resume game code to make sure you can quit game
        //Debug.Log("resumed");
        //Time.timeScale = 1;
    }
    public void MainMenuDeclineQuitButton()
    {
        // Debug.Log("decline quit was called");
        refQuitPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));

    }
    public void quit()
    {
        Application.Quit();

    }

    public void Restart()
    {
        //get every current player's movement script and reset their position to the spawn points in the map
        //Movement temp;
        //foreach (GameObject player in m_playerList)
        //{
        //    temp = player.GetComponent<Movement>();
        //    temp.transform.position = GameObject.Find("Player" + (player.GetComponent<Movement>().playerNumber + 1) + "_Spawn").transform.position;
        //}
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameObject.Find("PlayerController").GetComponent<UIManager>().Restart();
    }


    public void Pause()
    {
        Debug.Log("paused");
        refPausePanel.SetActive(true);
        Time.timeScale = 0;
        openPauseMenu = true;
        ref_BlockController.m_bIsPaused = false;

        foreach (GameObject item in m_playerList)
        {
            item.GetComponent<Movement>().m_bGameRunning = false;
        }
    }
    public void resumePlayButton()
    {
        Debug.Log("resumed");
        Time.timeScale = 1;
        refPausePanel.SetActive(false);
        openPauseMenu = false;
        ref_BlockController.m_bIsPaused = false;
        foreach (GameObject item in m_playerList)
        {
            item.GetComponent<Movement>().m_bGameRunning = true;
        }
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

}

