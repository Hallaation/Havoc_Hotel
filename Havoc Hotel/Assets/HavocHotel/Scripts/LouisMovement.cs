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
    private float m_fWallSlideSpeed; //wall sliding speed
    private bool m_bHitWall; //checks to see if the wall was hit or not.

    private CharacterController m_cCharacterController; //character controller reference

    public GameObject platformController;
    void Start()
    {
        m_cCharacterController = GetComponent<CharacterController>();
    }


    void OnTriggerEnter(Collider other)
    {
        CharacterController temp = GetComponent<CharacterController>();

        Debug.Log("TRIGGERED");
        Physics.IgnoreCollision(temp , other.GetComponentInParent<Collider>());
        Debug.Log(other.name);
        Debug.Log(GetComponent<Collider>().name);

    }

    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        CharacterController temp = GetComponent<CharacterController>();
        Physics.IgnoreCollision(temp , other.GetComponentInParent<Collider>() , false);
    }


    //-------------------------------------------------------------------------------------------------------------------------------------//
    //update every frame
    //Lincoln's messy code
    void Update()
    {

        CharacterController temp = GetComponent<CharacterController>();
        if (temp.isGrounded)
        {
            // This is the Left/Right movement for X. always set Y to 0.
            HasJumped = false;
            HasDoubleJumped = false;
            m_fGroundedTime += Time.deltaTime;
            if (m_fGroundedTime >= m_f1FramePasses)
            {
                movementDirection.y = 0.01f;
            }

            UnityEngine.Debug.Log("IsGrounded"); // UnityEngine.Debug.Log just sends the developer a message when this line is reached. Displayed in the Unity Client.
            if (temp.isGrounded)
            {
                m_fJumpTimer = 0.0f;
                UnityEngine.Debug.Log("IsGrounded2");

                if (!HasJumped && Input.GetButton(playerNumber + "_Fire"))// if the players jump button is down
                {

                    movementDirection.y = m_fJumpForce;

                    UnityEngine.Debug.Log("Jumping");
                    HasJumped = true;
                }
            }
        }

        if (!temp.isGrounded)
        {
            UnityEngine.Debug.Log("HasJumped");
            m_fJumpTimer += Time.deltaTime;
            UnityEngine.Debug.Log(m_fJumpTimer.ToString());
            if (m_fJumpTimer > 0.017)
            {
                if (Input.GetButtonUp(playerNumber + "_Fire"))
                {
                    m_bJumpKeyReleased = true;
                }
                if (!HasDoubleJumped && m_bJumpKeyReleased && Input.GetButtonDown(playerNumber + "_Fire")) // if the players jump button is down
                {
                    movementDirection.y = m_fDoubleJumpMoveForce;
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

        //if (temp.isGrounded && Input.GetAxis(playerNumber + "_Horizontal") > 100)
        //{
        //    movementDirection.x = m_fMoveSpeed;
        //}
        movementDirection.y -= m_fGravity;
        if (movementDirection.y < -m_fMaxFallSpeed)
        {
            movementDirection.y = -m_fMaxFallSpeed;
        }
        movementDirection.x += (m_fMoveSpeed * Input.GetAxis(playerNumber + "_Horizontal"));
        if (movementDirection.x > 0.0f)
        {
            movementDirection.x -= 0.5f;                // if momemntum x > 0, reduce it.
        }



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
        temp.Move(new Vector3(Time.deltaTime * movementDirection.x , movementDirection.y * Time.deltaTime));
        //temp.Move(movementDirection * Time.deltaTime);
        Debug.Log("FramesPassed");
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
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //useless remove it.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterController temp = GetComponent<CharacterController>();

    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //useless be sure to remove
    void JumpDown()
    {
        Debug.Log("Jumped down");

    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //If the wall is hit, the character will slide slowly on the wall.
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

    //-------------------------------------------------------------------------------------------------------------------------------------//
    //wall jumping
    //TODO:// move in opposite direction, currently only moves up
    void WallJump()
    {
        Debug.Log("Trying to jump");
        //movementDirection.y = m_fJumpSpeed;
        //HasJumped = true;
        movementDirection.x = 10;
        movementDirection.y += 20;
        m_cCharacterController.Move(Vector3.up * Time.deltaTime * m_fJumpForce);
    }
}
