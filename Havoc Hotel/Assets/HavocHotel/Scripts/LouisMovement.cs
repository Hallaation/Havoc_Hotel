using UnityEngine;
using System.Collections;

public class LouisMovement : MonoBehaviour {

    //declaring variables
    public float m_fMoveSpeed = 1.0f;
    const int _ROTATION_SPEED = 20; // Not used yet.
    const float m_f1FramePasses = 0.0170f;
    public float m_fJumpForce = 25.0f;
    public float m_fDoubleJumpMoveForce = 20f;
    public float m_fGravity = 2.0f;
    public Transform lookAt;

    float timer = 0.0f;
    public Vector3 movementDirection;
    private float m_fJumpTimer;
    bool HasJumped;
    bool m_bJumpKeyReleased;

    public bool HasDoubleJumped;
    public int playerNumber; //Input manager to know which joypad number to use
    public float m_fGroundedTime;
    public bool m_bAllowDoubleJumpAlways;
    public float m_fMaxFallSpeed = 25.0f; //maximum downfall momentum

    public CStates m_cState;
    public float m_fWallSlideSpeed = 0.5f; //wall sliding speed public so it can be edited outside of code
    private bool m_bHitWall; //checks to see if the wall was hit or not.

    /// <summary>
    /// Wall jumping forces, higher value for bigger push force, lower for less
    /// </summary>
    public float m_fHorizontalWallJumpForce;
    public float m_fVerticalWallJumpForce;

    private CharacterController m_cCharacterController; //character controller reference

    public GameObject platformController;
    void Start()
    {
        //have the charactercontroller variable reference something
        m_cCharacterController = GetComponent<CharacterController>();
    }


    void OnTriggerEnter(Collider other)
    {

        Physics.IgnoreCollision(m_cCharacterController , other.GetComponentInParent<Collider>());



    }

    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(m_cCharacterController , other.GetComponentInParent<Collider>() , false);
    }


    //-------------------------------------------------------------------------------------------------------------------------------------//
    //update every frame
    //Lincoln's messy code
    void Update()
    {
        switch (m_cState)
        {
            case CStates.OnFloor:
                OnFloor();
                break;
            case CStates.OnWall:
                if (!m_cCharacterController.isGrounded)
                {
                    WallSlide();
                }
                else
                {
                    OnFloor();
                }
                break;
            default:
                OnFloor();
                break;
        }
        Debug.Log(transform.rotation.eulerAngles.y);
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //on floor movement
    void OnFloor()
    {

        if (Input.GetAxis(playerNumber + "_Horizontal") > 0)
        {
            //x y z
            transform.rotation = Quaternion.Euler(0 , -90 , 0);
        }
        else if (Input.GetAxis(playerNumber + "_Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0 , 90 , 0);
        }

        if (m_cCharacterController.isGrounded)
        {
            // This is the Left/Right movement for X. always set Y to 0.
            HasJumped = false;
            HasDoubleJumped = false;
            m_fGroundedTime += Time.deltaTime;
            if (m_fGroundedTime >= m_f1FramePasses)
            {
                movementDirection.y = 0.01f;
            }


            if (m_cCharacterController.isGrounded)
            {
                m_fJumpTimer = 0.0f;


                if (!HasJumped && Input.GetButton(playerNumber + "_Fire"))// if the players jump button is down
                {

                    movementDirection.y = m_fJumpForce;


                    HasJumped = true;
                }
            }
        }

        if (!m_cCharacterController.isGrounded)
        {

            m_fJumpTimer += Time.deltaTime;
            if (m_fJumpTimer > 0.017)
            {
                if (Input.GetButtonUp(playerNumber + "_Fire"))
                {
                    m_bJumpKeyReleased = true;
                }
                if (!HasDoubleJumped && m_bJumpKeyReleased && Input.GetButtonDown(playerNumber + "_Fire")) // if the players jump button is down
                {
                    movementDirection.y = m_fDoubleJumpMoveForce;

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

        //if (m_cCharacterController.isGrounded && Input.GetAxis(playerNumber + "_Horizontal") > 100)
        //{
        //    movementDirection.x = m_fMoveSpeed;
        //}
        movementDirection.y -= m_fGravity;
        if (movementDirection.y < -m_fMaxFallSpeed)
        {
            movementDirection.y = -m_fMaxFallSpeed;
        }
        movementDirection.x -= (m_fMoveSpeed * Input.GetAxis(playerNumber + "_Horizontal"));
        if (movementDirection.x > 0.0f)
        {
            movementDirection.x -= 0.5f;                // if momemntum x > 0, reduce it.
        }


        //TODO: somehow clean this up.
        if (movementDirection.x > -0.26f && movementDirection.x < 0.26f && movementDirection.x != 0.0f)
        {
            movementDirection.x = 0.0f;                 // if momemntum within a range of .26 set it to 0;
        }
        if (movementDirection.x < 0.0f)
        {
            movementDirection.x += 0.5f;                // if momemntum x < 0, reduce it.
        }
        if (movementDirection.x > -0.26f && movementDirection.x < 0.26f && movementDirection.x != 0.0f)
        {
            movementDirection.x = 0.0f;                 // if momemntum within a range of .26 set it to 0;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------//
        if (movementDirection.x > 10)
        {
            movementDirection.x = 10;                   // Max speed settings
        }

        if (movementDirection.x < -10)
        {
            movementDirection.x = -10;                   // Max speed settings
        }
        //if (movementDirection.x < -m_fMoveSpeed)
        //{
        //    movementDirection.x = -m_fMoveSpeed;
        //}
        m_cCharacterController.Move(new Vector3(Time.deltaTime * movementDirection.x , movementDirection.y * Time.deltaTime));
        //m_cCharacterController.Move(movementDirection * Time.deltaTime);

        //if (movementDirection.y > 0.0f)
        //{
        //    UnityEngine.Debug.Log("movement dir y > 0");
        //    this.transform.position += new Vector3(0 , m_fm_fMoveSpeed * Time.deltaTime);

        //}
        //if (movementDirection.y == 0.0f && m_cCharacterController.isGrounded)    // Is always true. Does not work not sure why.
        //{
        //    this.transform.position += new Vector3(0 , m_fm_fMoveSpeed * Time.deltaTime);
        //    UnityEngine.Debug.Log("movement dir y == 0");

        //}
        //if (wentThrough)
        //{
        //    foreach (Collider i in platformController.GetComponentsInChildren<Collider>())
        //    {
        //        Physics.IgnoreCollision(m_cCharacterController , i , false);
        //        UnityEngine.Debug.Log(i.name);
        //    }
        //}
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //If the wall is hit, the character will slide slowly on the wall.
    void WallSlide()
    {
        m_bHitWall = true;
        if (m_bHitWall)
        {
            float tempGravity = m_fWallSlideSpeed;

            movementDirection.y = -tempGravity;
            m_cCharacterController.Move(movementDirection * Time.deltaTime);

            if (Input.GetButtonDown(playerNumber + "_Fire"))
            {
                WallJump();
                //movementDirection = Vector3.zero;
            }

        }
        else
        {
            movementDirection.y = 0;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------//
    //wall jumping
    //TODO:// move in opposite direction, currently only moves up
    void WallJump()
    {

        //movementDirection.y = m_fJumpSpeed;
        //HasJumped = true;
        if (transform.rotation.eulerAngles.y >= 1.0f && transform.rotation.eulerAngles.y <= 91.0f)
        {
            movementDirection.x = -m_fHorizontalWallJumpForce;
            movementDirection.y = m_fVerticalWallJumpForce;
            //m_cCharacterController.Move(movementDirection * Time.deltaTime * m_fJumpForce);
            m_cCharacterController.Move(movementDirection * Time.deltaTime);
        }
        else if (transform.rotation.eulerAngles.y >= 181.0f && transform.rotation.eulerAngles.y <= 271.0f)
        {
            Debug.Log(transform.rotation.eulerAngles.y);
            movementDirection.x = m_fHorizontalWallJumpForce;
            movementDirection.y = m_fVerticalWallJumpForce;
            //m_cCharacterController.Move(movementDirection * Time.deltaTime * m_fJumpForce);
            m_cCharacterController.Move(movementDirection * Time.deltaTime);
        }
    }
}
