using UnityEngine;
using System.Collections;

public class LouisMovement : MonoBehaviour {

    //declaring variables
    public float m_fMoveSpeed = 1.4f;
    const int _ROTATION_SPEED = 20; // Not used yet.
    const float m_f1FramePasses = 0.0170f;
    public float m_fJumpForce = 25.0f;
    public float m_fDoubleJumpMoveForce = 20f;
    public float m_fGravity = 1.4f;
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
    public float m_fHorizontalWallJumpForce = 20.0f;
    public float m_fVerticalWallJumpForce = 15.0f;

    private CharacterController m_cCharacterController; //character controller reference

    public bool m_bIsDead;

    public UnityEngine.UI.Text refPlayerStatus;
    //txtPlayers[i].text = (refPlayers[i].m_bIsDead) ? txtPlayers[i].text = "Player " + (i + 1) + ": Dead" : txtPlayers[i].text = "Player " + (i + 1) + ":  Alive";
    void Start()
    {
        //have the charactercontroller variable reference something
        m_cCharacterController = GetComponent<CharacterController>();
        m_bIsDead = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("Player " + playerNumber + " died!");
            m_bIsDead = true;
        }
        else
        {
            Debug.Log("TRIGGERED");
            Physics.IgnoreCollision(m_cCharacterController, other.GetComponentInParent<Collider>());
            Debug.Log(other.name);
            Debug.Log(GetComponent<Collider>().name);
        }
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
        refPlayerStatus.text = (m_bIsDead) ? "Player " + (playerNumber + 1) + ": Dead" : "Player " + (playerNumber + 1) + ": Alive";
        if (!m_bIsDead)
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
    
        }
        else
        {
            this.transform.position = new Vector3(20,-60,20);
        }
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //on floor movement
    void OnFloor()
    {
        PlayerTurnAround();

        CharacterController temp = GetComponent<CharacterController>();
        Jump(temp);
        //

        if(!temp.isGrounded && Input.GetButtonDown(playerNumber + "_AltFire"))
        {
            
        }



        //
        DoubleJump(temp);

        m_fJumpTimer += Time.deltaTime;
        timer += Time.deltaTime;
        MovementCalculations();

        temp.Move(new Vector3(Time.deltaTime * movementDirection.x * m_fMoveSpeed, Time.deltaTime * movementDirection.y));


    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //If the wall is hit, the character will slide slowly on the wall.
    void WallSlide()
    {
        m_bHitWall = true;
        if (m_bHitWall)
        {

            PlayerTurnAround();

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
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (transform.rotation.eulerAngles.y >= 181.0f && transform.rotation.eulerAngles.y <= 271.0f)
        {
            Debug.Log(transform.rotation.eulerAngles.y);
            movementDirection.x = m_fHorizontalWallJumpForce;
            movementDirection.y = m_fVerticalWallJumpForce;
            //m_cCharacterController.Move(movementDirection * Time.deltaTime * m_fJumpForce);
            transform.rotation = Quaternion.Euler(0, 90, 0);
            m_cCharacterController.Move(movementDirection * Time.deltaTime);
        }
        m_bHitWall = false;
    }


    //Lincolns shit
    void Jump(CharacterController temp)     // Checks if the user can jump, then executes on command if possible.
    {

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

                if (!HasJumped && Input.GetButtonDown(playerNumber + "_Fire"))// if the players jump button is down
                {

                    movementDirection.y = m_fJumpForce;

                    UnityEngine.Debug.Log("Jumping");
                    HasJumped = true;

                }
            }
        }
    }
    // Double Jump
    void DoubleJump(CharacterController temp)
    {
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
    }
    void MovementCalculations()
    {
        movementDirection.y -= m_fGravity;              // Gravity reduces Y movement every frame
        if (movementDirection.y < -m_fMaxFallSpeed)     // Prevents passing max fall speed
        {
            movementDirection.y = -m_fMaxFallSpeed;
        }
        movementDirection.x += (m_fMoveSpeed * -Input.GetAxis(playerNumber + "_Horizontal")); // Calculates X Movement
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
    }

    /// <summary>
    /// Turns the player around based on input.
    /// </summary>
    void PlayerTurnAround()
    {

        if (Input.GetAxis(playerNumber + "_Horizontal") > 0)
        {
            //x y z
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (Input.GetAxis(playerNumber + "_Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        m_bHitWall = false;
    }
}
