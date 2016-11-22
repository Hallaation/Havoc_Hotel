using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class CheatCodes : MonoBehaviour
{

    private Text m_tButtonPressed; //replace with the effect you want.
    public AudioClip m_aCheatAudio;
    public AudioClip m_aOriginalAudio;
    public string[] CheatCode = { "W" , "i" , "l" , "l" }; //the cheat code you want. make this more robust
    private int m_iCheatIndex;

    private bool m_bChangeAudio;

    // Use this for initialization

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        m_iCheatIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //okay lets actually break it down.
        //every frame look for an input.
        //If the input matches the cheat index. move forward.
        //Once the counter has reached the end. Activate cheat code and disable everything else.
        //Button press
        // If correct index ++
        //otherwise set to 0
        // if the index is cheatcode length. which means they have sucessfully reached it. Activate cheat.
        if (Input.anyKeyDown)
        {
            if (m_iCheatIndex != (CheatCode.Length - 1))
            {
                if (Input.inputString.ToLower() == CheatCode[m_iCheatIndex])
                {
                    m_iCheatIndex++;
                }
                else
                {
                    m_iCheatIndex = 0;
                }
            }
            else
            {
                //this is where the cheat activates
                m_bChangeAudio = true;
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += LookForObjects;
    }

    private void LookForObjects(Scene a_scene , LoadSceneMode a_sceneMode)
    {
        if (a_scene.buildIndex == 2)
        {
            if (m_bChangeAudio)
            {
                GameObject.Find("Music").GetComponent<AudioSource>().clip = m_aCheatAudio;
                GameObject.Find("Music").GetComponent<AudioSource>().Play();
            }
            else
            {
                GameObject.Find("Music").GetComponent<AudioSource>().clip = m_aOriginalAudio;
                GameObject.Find("Music").GetComponent<AudioSource>().Play();
            }
        }
    }
}