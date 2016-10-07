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
        //check to see if it isnt the kick hitbox
        if (this.tag != "Kick")
        {
            if (other.tag == "Wall")
            {
                m_refMovement.m_cState = CStates.OnWall;
            }

            //if the thing im colliding with has the player tag, allow pushing.
            Push(other);
        }

    }

    //when leaving the wall, the on player is switched the "onfloor" state which allow further regular movement
    void OnTriggerExit(Collider a_collision)
    {
        //exit out of wall jumping state and into onfloor
        if (a_collision.tag == "Wall" && this.tag != "Kick")
        {
            m_refMovement.m_cState = CStates.OnFloor;
        }
        Push(a_collision);
    }

    //??
    void OnPlayerKick(Collider a_collision)
    {
        if (this.tag == "Player" && a_collision.tag == "Kick")
        {
            //a_collision.gameObject.GetComponent<LouisMovement>()
        }
    }


    /// <summary>
    /// while the trigger stays in a collider check if it is in another player, if so push them
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        Push(other);
    }

    //only called when tag is player, this pushes the other collider's charactercontroller.
    void Push(Collider a_collider)
    {

        //if the thing im colliding with has the player tag, allow pushing.
        if (a_collider.tag == "Player")
        {
            if (Input.GetButtonDown(m_refMovement.playerNumber + "_AltFire"))
            {
                a_collider.GetComponent<LouisMovement>().movementDirection.x += this.transform.forward.x * m_refMovement.m_fPushForce;
            }
        }
    }
}


