using UnityEngine;
using System.Collections;

public class CharacterStates : MonoBehaviour
{
    //reference to a movement script
    public LouisMovement m_refMovement;

    // Use this for initialization
    void Start()
    {
        //returns to dev if the script is properly being made/instanced

    }
    //changes character state to wall jumping/sliding
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            m_refMovement.m_cState = CStates.OnWall;
        }
    }

    void OnTriggerExit(Collider a_collision)
    {
        //exit out of wall jumping state and into onfloor
        if (this.tag == "Wall")
        {
            m_refMovement.m_cState = CStates.OnFloor;
        }
    }
    void OnPlayerKick(Collider a_collision)
    {
        if(this.tag == "Player" && a_collision.tag == "Kick")
        {
            //a_collision.gameObject.GetComponent<LouisMovement>()
        }
    }
    //check to see if the collider entered something
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("I entered something");
    }
}
