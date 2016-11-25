using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIManager : MonoBehaviour
{
    //public Canvas refPopUpPanel; // ANY panel canvas that pops up.
    public GameObject refCreditsPanel;
    public GameObject refQuitPanel;
    public GameObject refPausePanel;
    public GameObject mainMenuPanel;
    public GameObject refCountDownTimer;
    public GameObject refWinPanel;
    private GameObject refCheatManger;
    public PlayerEnabler refPlayerEnabler;
    private BlockController ref_BlockController;
    private GameObject m_gLastSelctedObject;
    private List<GameObject> m_playerList = new List<GameObject>();

    private bool openPauseMenu; //bool to determin weather or not quit() was called from the pause menu or not

    private GameObject EventSystem;

    private bool m_bGameStarted = false;

    private bool m_PlayersInGameScene = false;

    private bool m_bResumeGame;

    private bool m_bIsGameFinished;

    public float m_fWaitTime = 5.0f;

    private float m_fTimer;


    public bool GameStarted { get { return m_bGameStarted; } set { m_bGameStarted = value; } }

    public bool PlayersInScene { get { return m_PlayersInGameScene; } set { m_PlayersInGameScene = value; } }

    public bool GameFinished { get { return m_bIsGameFinished; } set { m_bIsGameFinished = value; } }
    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        GameFinished = false;
        EventSystem = GameObject.Find("EventSystem");
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            ref_BlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            GameFinished = true;
        }
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            m_gLastSelctedObject = GameObject.Find("PlayButton");
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            m_playerList.Add(item);
        }

        if (GameObject.Find("CheatManager"))
        {
            refCheatManger = GameObject.Find("CheatManager");
        }

    }

    void Awake()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            ref_BlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();
            resumePlayButton();
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            GameFinished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject == null)
        {
            EventSetSelected(GameObject.Find("EventSystem"), m_gLastSelctedObject);
        }
        else
        {
            m_gLastSelctedObject = (GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject);
        }
        if (GameStarted && m_PlayersInGameScene)
        {
            if (Input.GetButtonDown("Cancel"))
            //if(Input.GetKeyDown(KeyCode.Escape))
            {
                //need to check if the game has finished or not.
                if (!openPauseMenu && !GameFinished)
                {
                    Pause();
                }
            }
            if (m_bResumeGame)
            {
                #region
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
                #endregion
            }
        }
        else
        {
            //If the player hasn't pressed start yet, make them "enabled" otherwise if an enabled player presses start the original start menu opens up again and disables all movement.
            #region 

            foreach (GameObject item in m_playerList)
            {
                //go through each player
                //check if player if they have started the game or not.
                //if they have allow them to open the start menu, otherwise dont bother.
                if (item)
                {
                    Movement tempMovement = item.GetComponent<Movement>();
                    if (m_bGameStarted)
                    {
                        if (Input.GetButtonDown(tempMovement.playerNumber + "_Start") && tempMovement.IsPlaying)
                        {
                            //open up the menu.
                            m_gLastSelctedObject = GameObject.Find("PlayButton");
                            mainMenuPanel.SetActive(true);
                            GameObject go = GameObject.Find("EventSystem");
                            GameObject button = GameObject.Find("PlayButton");
                            EventSetSelected(go, button);
                            refPlayerEnabler.DisablePlayers();
                        }
                        else if (Input.GetButtonDown(tempMovement.playerNumber + "_Start") && !tempMovement.IsPlaying)
                        {
                            tempMovement.IsPlaying = true;
                            tempMovement.refPlayerStartText.SetActive(false);
                        }
                    }
                }
            }

            #endregion
        }


        if (GameFinished)
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer >= m_fWaitTime)
            {
                refWinPanel.SetActive(true);
                EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("RestartButton"));
                GameFinished = false;
            }
        }

    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForEndOfFrame();
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(0); // loads scene with index of zero (can find index of scene with the build settings)
    }


    //goes through the playerlist and deletes their game object.
    //Also looks for any gameobjects with player tag. This is to look if there any any left over. 
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

    /// <summary>Does different stuff depending on what scene it is in.
    ///If in the second scene which is the game scene, the quit button will close up the pause menu.
    ///if in the Win scene, the win panel is closed up. Otherwise the functionality of both the game scene and win scene are the same.
    ///Otherwise if it's quit is pressed in any other scene, the main menu will be de activated.
    ///</summary>
    public void quitButton()
    {
        //if this is the game scene, make the quit text asking to return to main menu, otherwise run regular stuff which is to end the game.
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //If this is the game scene, load up the quit panel. close up the pause panel.
            refQuitPanel.SetActive(true);
            GameObject.Find("QuitText").GetComponent<Text>().text = "Are you sure you \nwant to return to main menu ? ";
            m_gLastSelctedObject = GameObject.Find("RealQuitButton");
            EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("RealQuitButton"));
            refPausePanel.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            refQuitPanel.SetActive(true);
            refWinPanel.SetActive(false);
            GameObject.Find("QuitText").GetComponent<Text>().text = "Are you sure you \nwant to return to main menu ? ";
            m_gLastSelctedObject = GameObject.Find("RealQuitButton");
            EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("RealQuitButton"));

        }
        else
        {
            //otherwise use the regular quit
            refQuitPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            m_gLastSelctedObject = GameObject.Find("RealQuitButton");
            EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("RealQuitButton"));
        }
        if (refCheatManger)
        {
            DestroyImmediate(refCheatManger);
        }
    }
    /// <summary>
    /// Similiar to quit button above. but instead of de activated a specific item from each scene, it re activates it.
    /// so main menu shows up the main menu
    /// game scene shows the pause menu
    /// win scene shows the winning menu.
    /// </summary>
    public void declineQuitButton()
    {
        //if the current scene is the game scene, return the pause menu.
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            refQuitPanel.SetActive(false);
            refPausePanel.SetActive(true);
            m_gLastSelctedObject = GameObject.Find("ResumeButton");
            EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("ResumeButton"));
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            refQuitPanel.SetActive(false);
            refWinPanel.SetActive(true);
            StartCoroutine(WaitTime());
            m_gLastSelctedObject = GameObject.Find("RestartButton");
            EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("RestartButton"));
        }
        else
        {
            refQuitPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            m_gLastSelctedObject = GameObject.Find("PlayButton");
            EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("PlayButton"));
        }

    }

    /// <summary>
    /// This is called whenever the confirmation button is pressed after any quit button is pressed.
    /// If in any scene other than the start menu, the game will return to the main menu
    /// otherwise the Game will close.
    /// </summary>
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

    /// <summary>
    /// Not sure if this is still used. probably isnt
    /// </summary>
    public void Restart()
    {
        GameObject.Find("PlayerController").GetComponent<PlayerTextController>().GameFinished = true;
        GameObject.Find("PlayerController").GetComponent<PlayerTextController>().Timer = 50000;
        //resumePlayButton();
    }

    /// <summary>
    /// Restart Game is called whenever the restart button is pressed in the pause menu or in the win scene
    /// It looks for all players and sets their state to not dead. turns them to active if they were de activated and then reload into the game scene again.
    /// </summary>
    public void RestartGame()
    {
        GameObject[] PlayerGos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in PlayerGos)
        {
            player.transform.position = new Vector3(-50, -50, -50);
            player.GetComponent<Movement>().m_bIsDead = false;
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<Movement>().ResetPlayer();
            player.gameObject.SetActive(true);
        }
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// This pauses the game. A pause menu will show up when in the game scene.
    /// </summary>
    public void Pause()
    {
        refPausePanel.SetActive(true);
        m_gLastSelctedObject = GameObject.Find("ResumeButton");
        EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("ResumeButton"));
        openPauseMenu = true;
        GameObject.Find("Music").GetComponent<AudioSource>().Pause();
        ref_BlockController.m_bIsPaused = false;

        foreach (GameObject item in m_playerList)
        {
            item.GetComponent<Movement>().m_bGameRunning = false;
        }
        Time.timeScale = 0;
    }

    /// <summary>
    /// This resumes the game after it is pressed in the pause menu.
    /// </summary>
    public void resumePlayButton()
    {
        Time.timeScale = 1;
        refPausePanel.SetActive(false);
        openPauseMenu = false;

        m_bResumeGame = true;



    }

    /// <summary>
    /// might be used. not 100% sure. keeping it around in case it breaks something
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitTime()
    {
        yield return new WaitForSecondsRealtime(5);
    }
    /// <summary>
    /// this brings up the credits panel when the credits button is pressed in the main menu
    /// </summary>
    public void Credits()
    {
        //  Debug.Log("credits");
        refCreditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        m_gLastSelctedObject = GameObject.Find("CreditsBackButton");
        EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("CreditsBackButton"));
    }
    /// <summary>
    /// used to return to main menu
    /// </summary>
    public void creditsBack()
    {
        //  Debug.Log("cradits back");
        refCreditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        m_gLastSelctedObject = GameObject.Find("PlayButton");
        EventSetSelected(GameObject.Find("EventSystem"), GameObject.Find("PlayButton"));
    }

    /// <summary>
    /// once the play button is pressed in the start menu, the game will start and the menu will close
    /// it sends an argument to show the "Press Start" above the players or not.
    /// </summary>
    public void CloseMainMenu()
    {
        mainMenuPanel.SetActive(false);
        if (!m_bGameStarted)
        {
            refPlayerEnabler.EnablePlayers(m_bGameStarted);
            m_bGameStarted = true;
        }
        else
        {
            refPlayerEnabler.EnablePlayers(m_bGameStarted);
        }
    }

    /// <summary>
    /// Returns to the main menu. deletes players from a list
    /// </summary>
    public void ReturnToMainMenu()
    {
        for (int i = 0; i < m_playerList.Count; ++i)
        {
            DestroyImmediate(m_playerList[i]);
        }
        SceneManager.LoadScene(1);
    }

    void EventSetSelected(GameObject a_eventSystem, GameObject a_selected)
    {
        a_eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        StartCoroutine(WaitTime());
        a_eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(a_selected);
    }

}

