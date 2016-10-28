using UnityEngine;
using System.Collections;

public class CharacterStates : MonoBehaviour
{

    //reference to a movement script
    public Movement m_refMovement;

    // Use this for initialization
    void Start()
    {
        //returns to dev if the script is properly being made/instanced
        //looks for a movement script in the parents
        m_refMovement = transform.parent.GetComponentInParent<Movement>();

    }


    void Update()
    {
        //Debug.Log(this.transform.forward);

        //RaycastHit hit;
        //if (Input.GetButtonDown(m_refMovement.playerNumber + "_AltFire"))
        //{
        //    Vector3 rayOrigin = this.transform.position + new Vector3(0f, 0f, 0f);
        //    Debug.DrawLine(rayOrigin, rayOrigin + this.transform.forward);
        //    if (Physics.Raycast(rayOrigin, this.transform.forward, out hit, 0.9f))
        //    {
        //        if (hit.transform.tag == "Player")
        //        {
        //            hit.transform.gameObject.GetComponent<LouisMovement>().movementDirection.x += this.transform.forward.x * m_refMovement.m_fPushForce;
        //        }
        //    }
        //}
    }


    //changes character state to wall jumping/sliding
    void OnTriggerEnter(Collider other)
    {
        if (this.tag == "Kick")
        {
            if (other.tag == "Head")
            {
                Movement hitTemp = other.GetComponent<Movement>();
                m_refMovement.movementDirection.y += m_refMovement.m_fHeadBounceForce;
                //GetComponentInParent<LouisMovement>().movementDirection.y += GetComponentInParent<LouisMovement>().m_fHeadBounceForce;
                other.GetComponentInParent<Movement>().m_cState = CStates.Stunned;

                //other.GetComponent<LouisMovement>().m_cState = CStates.Stunned;
                //other.GetComponent<LouisMovement>().m_cState = CStates.Stunned;
                m_refMovement.m_cState = CStates.OnFloor;

            }
        }
    }

    void OnTriggerExit(Collider a_collision)
    {
        //exit out of wall jumping state and into onfloor
        if (a_collision.tag == "Wall" && this.tag != "Kick")
        {
            m_refMovement.m_cState = CStates.OnFloor;
        }
    }


    /// <summary>
    /// while the trigger stays in a collider check if it is in another player, if so push them
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
  
        
           if (other.tag == "Wall")
            {

                m_refMovement.m_cState = CStates.OnWall;


            }

        
    }


    //void Push(Collider a_collider)
    //{
    //    //a_collider.GetComponent<CharacterController>().Move(this.transform.forward * m_refMovement.m_fPushForce * Time.deltaTime) ;
    //    a_collider.GetComponent<LouisMovement>().movementDirection.x += this.transform.forward.x * m_refMovement.m_fPushForce;
    //}

}
