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
        
        m_refMovement.m_cState = CStates.OnFloor;
    }

    //check to see if the collider entered something
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("I entered something");
    }

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
