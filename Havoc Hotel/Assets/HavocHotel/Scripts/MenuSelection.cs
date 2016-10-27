using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour
{


    public GameObject[] m_panelArray;
    //Main Menu - 1
    //Quit Menu - 2
    //Credits -  3
    //Puase panel - 4
    // Use this for initialization

    GameObject EventSystem;
    public GameObject currentlySelectedObject;

    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        //EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(currentlySelectedObject);
    }
}
