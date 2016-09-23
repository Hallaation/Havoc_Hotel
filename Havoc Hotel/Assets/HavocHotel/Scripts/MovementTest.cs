using UnityEngine;
using System.Collections;

public class States
{
    public enum CharacterState
    {
        Ground,
        Wall
    }
}

public class MovementTest : MonoBehaviour
{

    public float m_fMoveSpeed = 1.0f;
    const int _ROTATION_SPEED = 20; // Not used yet.

    public float m_fJumpSpeed = 20.0f;
    public float m_fDoublem_fMoveSpeed = 10;
    public float m_fGravity = 2.0f;
    public float m_fWallSlideSpeed = 0.5f;
    public Transform lookAt;
    CharacterController CharacetrController;
    float timer = 0.0f;
    private Vector3 movementDirection;
    private float m_fJumpTimer;
    bool HasJumped;
    bool m_bJumpKeyReleased;
    public bool HasDoubleJumped;
    public int playerNumber;

    private bool m_bHitWall = false;
    public States.CharacterState m_cState = States.CharacterState.Ground;

    //private Vector3 movementDirection = Vector3.zero;

    public GameObject platformController;

    void Start()
    {
        CharacetrController = GetComponent<CharacterController>();
    }
    //update every frame
    //void Update()
    //{
    //    CharacterController temp = GetComponent<CharacterController>();
    //    if (temp.isGrounded || disable)
    //    {
    //        this.transform.position -= new Vector3(Input.GetAxis(PlayerNumber +"_Horizontal") * Time.deltaTime * m_fm_fMoveSpeed , 0);
    //        if (temp.isGrounded)
    //        {
    //            if (Input.GetButton(PlayerNumber + "_Fire") && Input.GetAxis(PlayerNumber + "_Fire") >= 0)
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
    //    if (Input.GetButton(PlayerNumber + "_Fire") && Input.GetAxis(PlayerNumber + "_Fire") < 0)
    //    {
    //        //JumpDown();
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        CharacterController temp = GetComponent<CharacterController>();

        Physics.IgnoreCollision(temp , other.GetComponentInParent<Collider>());


    }
    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        CharacterController temp = GetComponent<CharacterController>();
        Physics.IgnoreCollision(temp , other.GetComponentInParent<Collider>() , false);
    }



    //update every frame

    void Update()
    {

        CharacterController temp = GetComponent<CharacterController>();
        if (temp.isGrounded)
        {
            OnGround();
        }
        switch (m_cState)
        {
            case States.CharacterState.Ground:
                OnGround();
                break;
            case (States.CharacterState.Wall):
                if (!temp.isGrounded)
                {
                    m_bHitWall = true;
                    WallSlide();
                }
                movementDirection.x = 0;
                
                break;
            default:
                OnGround();
                break;
        }


    }


    void OnGround()
    {
        if (Input.GetAxis(playerNumber + ("_Horizontal")) > 0)
        {
            this.transform.rotation = Quaternion.Euler(0,-90,0);
        }
        else if (Input.GetAxis(playerNumber + ("_Horizontal")) < 0)
        {
            this.transform.rotation = Quaternion.Euler(0 , 90 , 0);
        }
        CharacterController temp = GetComponent<CharacterController>();
        if (temp.isGrounded)
        {
            // This is the Left/Right movement for X. always set Y to 0.
            HasJumped = false;
            HasDoubleJumped = false;


            if (temp.isGrounded)
            {
                m_fJumpTimer = 0.0f;


                if (!HasJumped && Input.GetButton(playerNumber + "_Fire"))// if the players jump button is down
                {
                    //Debug.Break();
                    movementDirection.y = m_fJumpSpeed;
                    HasJumped = true;
                }
            }
        }

        if (HasJumped)
        {
            m_fJumpTimer += Time.deltaTime;
            //UnityEngine.Debug.Log(m_fJumpTimer.ToString());
            if (m_fJumpTimer > 0.300)
            {
                if (Input.GetButtonUp(playerNumber + "_Fire"))
                {
                    m_bJumpKeyReleased = true;
                }
                if (!HasDoubleJumped && m_bJumpKeyReleased && Input.GetButtonDown(playerNumber + "_Fire")) // if the players jump button is down
                {
                    movementDirection.y = m_fJumpSpeed;
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

        temp.Move(new Vector3(Input.GetAxis(playerNumber + "_Horizontal") * -Time.deltaTime * m_fMoveSpeed , 0));
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

    void WallSlide()
    {
        m_bHitWall = true;
        if (m_bHitWall)
        {
            float tempGravity = m_fWallSlideSpeed;
            CharacterController temp = GetComponent<CharacterController>();
            Debug.Log("Trying to wall slide");
            movementDirection.y = -tempGravity;
            temp.Move(movementDirection * Time.deltaTime);

            if (Input.GetButtonDown(playerNumber + "_Fire"))
            {

                WallJump();
            }

        }
        else
        {
            movementDirection.y = 0;
        }
    }


    void WallJump()
    {
        Debug.Log("Trying to jump");
        //movementDirection.y = m_fJumpSpeed;
        //HasJumped = true;
        movementDirection.x = 10;
        movementDirection.y += 20;
        CharacetrController.Move(Vector3.up * Time.deltaTime * m_fJumpSpeed);
    }

}
