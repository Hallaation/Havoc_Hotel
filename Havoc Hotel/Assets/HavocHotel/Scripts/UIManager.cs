using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public Canvas popupPanel;
    public Canvas credits;

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
        popupPanel.enabled = true;
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("quit was called");
    }

    public void donDoIt()
    {
        Debug.Log("cancel was called");
        popupPanel.enabled = false;
    }

    public void pauseButton()
    {
        Debug.Log("paused");
        Time.timeScale = 0;
        popupPanel.enabled = true;
    }

    public void playButton()
    {
        Debug.Log("played");
        Time.timeScale = 1;
        popupPanel.enabled = false;
    }

    public void test()
    {
        Debug.Log("test active");
    }

    public void Credits()
    {
        Debug.Log("cradits");
        credits.enabled = true;
    }
    public void creditsBack()
    {
        Debug.Log("cradits back");
        credits.enabled = false;
    }
}

