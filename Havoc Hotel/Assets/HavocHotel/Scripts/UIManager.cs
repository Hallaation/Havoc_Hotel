using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public Canvas refPopUpPanel; // ANY panel canvas that pops up.
    public GameObject refCreditsPanel;
    public GameObject refQuitPanel;
    public GameObject refPausePanel;
    public GameObject refStartMenu;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            pauseButton();
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

    public void quitButton()
    {
        Debug.Log("quitpanel was called");
        refQuitPanel.SetActive(true);
    }
    public void declineQuitButton()
    {
        Debug.Log("quitpanel was called");
        refQuitPanel.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("quit was called");
    }

    //public void QuiteCancel()
    //{
    //    Debug.Log("cancel was called");
    //    refPopUpPanel.enabled = false;
    //}

    public void pauseButton()
    {
        Debug.Log("paused");
        refPausePanel.SetActive(true);
        Time.timeScale = 0;
 
    }

    public void playButton()
    {
        Debug.Log("played");
        Time.timeScale = 1;
        refPausePanel.SetActive(false);
    }

    public void Credits()
    {
        Debug.Log("cradits");
        refCreditsPanel.SetActive(true);
    }
    public void creditsBack()
    {
        Debug.Log("cradits back");
        refCreditsPanel.SetActive(false);
    }

    public void CloseMainMenu()
    {
        refStartMenu.SetActive(false);
    }

}

