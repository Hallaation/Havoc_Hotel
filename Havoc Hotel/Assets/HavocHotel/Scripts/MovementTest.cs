using UnityEngine;
using System.Collections;

public class MovementTest : MonoBehaviour
{
    public float m_fMoveSpeed = 10.0f;
    const int _ROTATION_SPEED = 20; // Not used yet.

    public float m_fJumpSpeed = 20.0f;
    public float m_fDoublem_fMoveSpeed = 10;
    public float m_fGravity = 2.0f;
    public Transform lookAt;
    bool wentThrough = false;
    bool disable = true;

    //private Vector3 movementDirection = Vector3.zero;

    public GameObject platformController;

    //update every frame
    //void Update()
    //{
    //    CharacterController temp = GetComponent<CharacterController>();
    //    if (temp.isGrounded || disable)
    //    {
    //        this.transform.position -= new Vector3(Input.GetAxis(PlayerNumber +"_Horizontal") * Time.deltaTime * m_fm_fMoveSpeed , 0);
    //        if (temp.isGrounded)
    //        {
    //            if (Input.GetButton(PlayerNumber + "_Fire") && Input.GetAxis(PlayerNumber + "_Vertical") >= 0)
    //            {
    //                transform.position += new Vector3(0 , m_fJumpSpeed * Time.deltaTime);
    //                //movementDirection.y = m_fJumpSpeed;
    //                //Debug.Log("Jumping");
    //            }
    //        }
    //    }
    //    //timer += Time.deltaTime;
    //    
    //    movementDirection.y -= m_fGravity * Time.deltaTime;
    //    temp.Move(movementDirection * Time.deltaTime);
    //
    //    //if (wentThrough)
    //    //{
    //    //    foreach (Collider i in platformController.GetComponentsInChildren<Collider>())
    //    //    {
    //    //        Physics.IgnoreCollision(temp , i , false);
    //    //        Debug.Log(i.name);
    //    //    }
    //    //}
    //
    //    if (Input.GetButton(PlayerNumber + "_Fire") && Input.GetAxis(PlayerNumber + "_Vertical") < 0)
    //    {
    //        //JumpDown();
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        CharacterController temp = GetComponent<CharacterController>();

        Debug.Log("TRIGGERED");
        Physics.IgnoreCollision(temp, other.GetComponentInParent<Collider>());
        Debug.Log(other.name);
        Debug.Log(GetComponent<Collider>().name);

    }
    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        CharacterController temp = GetComponent<CharacterController>();
        Physics.IgnoreCollision(temp, other.GetComponentInParent<Collider>(), false);
    }


    private bool m_bFlag = false;
    float timer = 0.0f;
    private Vector3 movementDirection;
    private float m_fJumpTimer;
    bool HasJumped;
    bool m_bJumpKeyReleased;
    public bool HasDoubleJumped;
    public int playerNumber;

    //update every frame

    void Update()
    {

        CharacterController temp = GetComponent<CharacterController>();
        if (temp.isGrounded)
        {
            // This is the Left/Right movement for X. always set Y to 0.
            HasJumped = false;
            HasDoubleJumped = false;

            UnityEngine.Debug.Log("IsGrounded"); // UnityEngine.Debug.Log just sends the developer a message when this line is reached. Displayed in the Unity Client.
            if (temp.isGrounded)
            {
                m_fJumpTimer = 0.0f;
                UnityEngine.Debug.Log("IsGrounded2");

                if (!HasJumped && Input.GetButton(playerNumber + "_Vertical"))// if the players jump button is down
                {

                    movementDirection.y = m_fJumpSpeed;

                    UnityEngine.Debug.Log("Jumping");
                    HasJumped = true;
                }
            }
        }

        if (HasJumped)
        {
            UnityEngine.Debug.Log("HasJumped");
            m_fJumpTimer += Time.deltaTime;
            UnityEngine.Debug.Log(m_fJumpTimer.ToString());
            if (m_fJumpTimer > 0.300)
            {
                if (Input.GetButtonUp(playerNumber + "_Vertical"))
                {
                    m_bJumpKeyReleased = true;
                }
                if (!HasDoubleJumped && m_bJumpKeyReleased && Input.GetButtonDown(playerNumber + "_Vertical")) // if the players jump button is down
                {
                    movementDirection.y = m_fJumpSpeed;
                    UnityEngine.Debug.Log("HasDoubleJumped");
                    HasDoubleJumped = true;
                }
            }
        }

        if (!HasDoubleJumped)
        {

        }
        m_fJumpTimer += Time.deltaTime;
        timer += Time.deltaTime;

        //if(m_fJumpTimer <= 150)
        //{

        //}

        temp.Move(new Vector3(Input.GetAxis(playerNumber + "_Horizontal") * Time.deltaTime * m_fMoveSpeed, 0));
        temp.Move(movementDirection * Time.deltaTime);
        movementDirection.y -= m_fGravity;
        //if (movementDirection.y > 0.0f)
        //{
        //    UnityEngine.Debug.Log("movement dir y > 0");
        //    this.transform.position += new Vector3(0 , m_fm_fMoveSpeed * Time.deltaTime);

        //}
        //if (movementDirection.y == 0.0f && temp.isGrounded)    // Is always true. Does not work not sure why.
        //{
        //    this.transform.position += new Vector3(0 , m_fm_fMoveSpeed * Time.deltaTime);
        //    UnityEngine.Debug.Log("movement dir y == 0");

        //}
        //if (wentThrough)
        //{
        //    foreach (Collider i in platformController.GetComponentsInChildren<Collider>())
        //    {
        //        Physics.IgnoreCollision(temp , i , false);
        //        UnityEngine.Debug.Log(i.name);
        //    }
        //}
    }

    //this is where the collision is ignored. once the player hits a platform with the name "platform" in it the collisions for the player and this collider are ignored. which are re enabled later after the trigger exit
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterController temp = GetComponent<CharacterController>();

    }


    void JumpDown()
    {
        Debug.Log("Jumped down");

    }

}
