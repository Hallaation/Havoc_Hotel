using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheatCodes : MonoBehaviour
{

    private Text m_tButtonPressed; //replace with the effect you want.

    public string[] CheatCode = { "W" , "i" , "l" , "l" }; //the cheat code you want. make this more robust
    private int m_iCheatIndex;
    // Use this for initialization

    void Start()
    {
        m_tButtonPressed = GameObject.Find("Cheat_Activated").GetComponent<Text>();
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
                m_tButtonPressed.text = "Cheat Activated";
            }
        }
    }
}