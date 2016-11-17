using UnityEngine;
using System.Collections;

public class CharacterStates : MonoBehaviour
{

    //reference to a movement script
    public Movement m_refMovement;

    private bool m_bIsInWall;
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
        //else
        //{
        //    m_refMovement.m_cState = CStates.OnFloor;
        //}
        //if(!WallCheck())
        //{
        //    m_refMovement.m_cState = CStates.OnFloor;
        //}
        if (this.tag == "Head")
        {
            HeadCheck();
        }
    }


    //changes character state to wall jumping/sliding 
    //when your character kicks another player, its downward movementt is set to 0 and a bounce force is added
    void OnTriggerEnter(Collider other)
    {
        if (this.tag == "Kick")
        {
            if (other.tag == "Head")
            {
                Movement hitTemp = other.GetComponent<Movement>();
                m_refMovement.movementDirection.y = 0;
                m_refMovement.movementDirection.y += m_refMovement.m_fHeadBounceForce;
                m_refMovement.transform.FindChild("Dive_Kick_Particle").GetComponent<ParticleSystem>().Stop();
                other.GetComponentInParent<Movement>().m_cState = CStates.Stunned;
                m_refMovement.m_bIsKicking = false;
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
        //if(a_collision.tag == "Head")
        //{
        //    this.gameObject.SetActive(false);
        //}
    }


    /// <summary>
    /// while the trigger stays in a collider check if it is in another player, if so push them
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (this.tag == "WallCheck")
        {
            if (other.tag == "Wall" && this.tag == "WallCheck")
            {
                m_bIsInWall = true;
                m_refMovement.m_cState = CStates.OnWall;
            }
        }

    }

    bool WallCheck()
    {
        #region
        if (this.tag == "WallCheck")
        {
            RaycastHit hit;
            Debug.DrawRay(this.transform.position + this.transform.forward, Vector3.right, Color.red, 1);
            if (Physics.Raycast(this.transform.position + this.transform.forward, Vector3.right, out hit, 0.2f))
            {
                if (hit.transform.tag == "Wall")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
        #endregion
    }

    void HeadCheck()
    {
        #region
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, Vector3.up, Color.yellow, 1);
        if (Physics.Raycast(this.transform.position, Vector3.up, out hit, 0.4f))
        {
            if (hit.transform.tag == "Wall")
            {
                Debug.Log("Head is htting something");
                m_refMovement.m_cState = CStates.OnFloor;
                //m_cCharacterController.Move(Vector3.down * 0.2f);
                m_refMovement.movementDirection.y = 0 - m_refMovement.refBlockController.m_fOverworldSpeed;
                // m_bIsKicking = false;
            }
        }
        #endregion
    }
}
