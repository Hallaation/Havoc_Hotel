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


    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
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
        Debug.Log("quitpanel was called");
        refQuitPanel.SetActive(true);
    }
    public void declineQuitButton()
    {
        Debug.Log("decline quit was called");
        refQuitPanel.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("quit was called");
    }

    public void Restart()
    {
        Debug.Log("restart called");
    }

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
 
    }

    public void resumePlayButton()
    {
        Debug.Log("resumed");
        Time.timeScale = 1;
        refPausePanel.SetActive(false);
    }

    public void Credits()
    {
        Debug.Log("credits");
        refCreditsPanel.SetActive(true);
    }
    public void creditsBack()
    {
        Debug.Log("cradits back");
        refCreditsPanel.SetActive(false);
    }

    public void CloseMainMenu()
    {
       mainMenuPanel.SetActive(false);
    }

}

