using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    //public Canvas refPopUpPanel; // ANY panel canvas that pops up.
    public GameObject refCreditsPanel;
    public GameObject refQuitPanel;
    public GameObject refPausePanel;
    public GameObject mainMenuPanel;

    private BlockController ref_BlockController;
    bool openPauseMenu; //bool to determin weather or not quit() was called from the pause menu or not

    // Use this for initialization
    void Start()
    {
        ref_BlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
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
        mainMenuPanel.SetActive(false);
    }

    public void quitButton()
    {
        // Debug.Log("quitpanel was called");
        refQuitPanel.SetActive(true);

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

    public void quit()
    {
        Application.Quit();
        Debug.Log("quit was called");
    }

    public void Restart()
    {
        Debug.Log("restart called");
        SceneManager.LoadScene(0); //loads scene (change number depending on scene index that is playing game)
    }                               //find scene index in build settings

    //public void QuiteCancel()
    //{
    //    Debug.Log("cancel was called");
    //    refPopUpPanel.enabled = false;
    //}

    public void Pause()
    {
        Debug.Log("paused");
        refPausePanel.SetActive(true);
        Time.timeScale = 0;
        openPauseMenu = true;
    }
    public void resumePlayButton()
    {
        Debug.Log("resumed");
        Time.timeScale = 1;
        refPausePanel.SetActive(false);
        openPauseMenu = false;
    }

    public void Credits()
    {
        //  Debug.Log("credits");
        refCreditsPanel.SetActive(true);
    }
    public void creditsBack()
    {
        //  Debug.Log("cradits back");
        refCreditsPanel.SetActive(false);
    }

    public void CloseMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

}

