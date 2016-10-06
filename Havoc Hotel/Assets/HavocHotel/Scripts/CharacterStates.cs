﻿using UnityEngine;
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
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown(m_refMovement.playerNumber + "_AltFire"))
            {
                Push(other);
            }
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

    /// <summary>
    /// while the trigger stays in a collider check if it is in another player, if so push them
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown(m_refMovement.playerNumber + "_AltFire"))
            {
                Push(other);
            }
        }
    }

    void Push(Collider a_collider)
    {
        //a_collider.GetComponent<CharacterController>().Move(this.transform.forward * m_refMovement.m_fPushForce * Time.deltaTime) ;
        a_collider.GetComponent<LouisMovement>().movementDirection.x += this.transform.forward.x * m_refMovement.m_fPushForce;
    }

}
