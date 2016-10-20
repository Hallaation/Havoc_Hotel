using UnityEngine;
using System.Collections;

public class LouisMovement : MonoBehaviour
{
    //ALL THE PUBLIC VARIABLES.
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    public int playerNumber; //Input manager to know which joypad number to use
    public float m_fGravity = 50f;
    public float m_fMoveSpeed = 1.4f;

    public CStates m_cState;
    //movement stuff
    public bool HasJumped;
    public bool m_bCanJump;
    public bool HasDoubleJumped;
    public bool m_bAllowDoubleJumpAlways;
    public float m_fJumpForce = 25f;
    public float m_fDoubleJumpMoveForce = 15f;
    public float m_fMaxFallSpeed = 15f;

    public float m_fPushForce = 10.0f;
    //wall jump stuff
    public float m_fHorizontalWallJumpForce = 20.0f;
    public float m_fVerticalWallJumpForce = 15.0f;
    public float m_fTurnDelay = 1.0f;
    public float m_fWallSlideSpeed = 0.5f; //wall sliding speed public so it can be edited outside of code

    //dive kick stuff
    public bool m_bIsKicking;
    public float m_fMaxKickSpeed = 25.0f;
    public float m_fMaxStunTime = 15.0f;
    public float m_fMaxKickTime = 5f;
    public float m_fHeadBounceForce = 20f;

    //quick release
    private int m_iQuickRelease;
    public int iReleaseCount = 10;




    public bool m_bIsStunned;
    public bool m_bIsDead;

    public Vector3 movementDirection;


    //references
    public GameObject ref_KickHitBox;

    public UnityEngine.UI.Text refPlayerStatus;

    public PlayerTextController ref_PlayerArray;

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    private float m_fGroundBuffer = 0.036f;

    private float m_fAirBourneTime;
    //declaring variables
    const int _ROTATION_SPEED = 20; // Not used yet.
    const float m_f1FramePasses = 0.0170f;
    float m_fButtonTimer = 0.0f;

    float timer = 0.0f;
    private float m_fJumpTimer;
    bool m_bJumpKeyReleased;



    //maximum downfall momentum
    private bool m_bHitWall; //checks to see if the wall was hit or not.

    /// <summary>
    /// Wall jumping forces, higher value for bigger push force, lower for less
    /// </summary>

    private CharacterController m_cCharacterController; //character controller reference
    float m_fCurrentStunTime;
   
    float m_fTimeSinceLastKick;
    float m_fKickCoolDown;
    float m_fCurrentKickTime;

    private bool m_bIsPlaying;

    public BlockController refBlockController;
    //txtPlayers[i].text = (refPlayers[i].m_bIsDead) ? txtPlayers[i].text = "Player " + (i + 1) + ": Dead" : txtPlayers[i].text = "Player " + (i + 1) + ":  Alive";
    void Start()
    {
        //GameObject[] list = GameObject.FindObjectsOfType<GameObject>();
        //list[0].name.Contains

        m_bIsPlaying = false;
        m_iQuickRelease = 0;
        ref_KickHitBox.SetActive(false);
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

            Physics.IgnoreCollision(m_cCharacterController, other.GetComponentInParent<Collider>());


        }
    }


    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(m_cCharacterController, other.GetComponentInParent<Collider>(), false);
    }


    //-------------------------------------------------------------------------------------------------------------------------------------//
    //update every frame
    //Lincoln's messy code
    void Update()
    {
        //begin of mess
        CharacterController temp = GetComponent<CharacterController>();

        refPlayerStatus.text = (m_bIsDead) ? "Player " + (playerNumber + 1) + ": Dead" : "Player " + (playerNumber + 1) + ": Alive";

        if (m_bIsPlaying)
        {
            if (!m_bIsDead)
            {

                if (m_bIsKicking)
                {
                    PlayerKick(m_cCharacterController);
                }

                // end of mess
                if (temp.isGrounded)
                {
                    m_fAirBourneTime = 0f;
                }
                switch (m_cState)
                {
                    case CStates.Stunned:
                        PlayerStun();
                        temp.Move(new Vector3(0, Time.deltaTime * movementDirection.y));
                        break;

                    case CStates.OnFloor:
                        OnFloor();
                        Push();
                        break;

                    case CStates.Kicking:
                        if (m_bIsKicking == false)
                        {
                            m_cState = CStates.OnFloor;
                        }

                        PlayerKick(temp);
                        MovementCalculations();
                        m_fAirBourneTime = 0;
                        temp.Move(new Vector3(Time.deltaTime * movementDirection.x * m_fMoveSpeed, Time.deltaTime * movementDirection.y));
                        break;

                    case CStates.OnWall:
                        m_fAirBourneTime = 2;
                        if (!m_cCharacterController.isGrounded)
                        {
                            WallSlide();
                        }
                        else if(m_cCharacterController.isGrounded)
                        {
                            OnFloor();
                        }
                        break;
                }

            }
            else if (!m_bIsDead)
            {
                this.transform.position = new Vector3(20, -60, 20);
            }



        }
        else
        {
            refPlayerStatus.text = "Press Start to join";
            if (Input.GetButtonDown(playerNumber + "_Start"))
            {
                m_bIsPlaying = true;
                ref_PlayerArray.refPlayers.Add(this);

            }
        }
        // }
        //    }


        //        else
        //        {
        //            refPlayerStatus.text = "Press Start to join";
        //            if (Input.GetButtonDown(playerNumber + "_Start"))
        //            {
        //                m_bIsPlaying = true;
        //                ref_PlayerArray.refPlayers.Add(this);
        //}
        //        }
        //        //quick stun release. mash button to release stun (when in stun) 

    }

    //-------------------------------------------------------------------------------------------------------------------------------------//
    //on floor movement
    void OnFloor()
    {
        m_fCurrentKickTime = 0;
        PlayerTurnAround();

        Jump(m_cCharacterController);
        //

        PlayerKick(m_cCharacterController);

        //
        DoubleJump(m_cCharacterController);

        m_fJumpTimer += Time.deltaTime;
        timer += Time.deltaTime;
        MovementCalculations();
        m_cCharacterController.Move(new Vector3(Time.deltaTime * movementDirection.x * m_fMoveSpeed, Time.deltaTime * movementDirection.y));


    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //If the wall is hit, the character will slide slowly on the wall.
    public void WallSlide()
    {

        m_fAirBourneTime = 2;
        m_bHitWall = true;
        if (m_bHitWall)
        {
            m_fWallSlideSpeed = refBlockController.m_fOverworldSpeed + 1.5f;
            //short delay when moving away from wall

            bool horizontalActive = Input.GetAxis(playerNumber + "_Horizontal") != 0;
            m_fButtonTimer += 0.05f * System.Convert.ToByte(horizontalActive);

            if (m_fButtonTimer >= m_fTurnDelay)
            {
                PlayerTurnAround();
                m_fButtonTimer = 0.0f;
            }

            float tempGravity = m_fWallSlideSpeed;

            movementDirection.y = -tempGravity;
            m_cCharacterController.Move(movementDirection * Time.deltaTime);

            if (Input.GetButtonDown(playerNumber + "_Fire"))
            {
                WallJump();
                
                //movementDirection = Vector3.zero;
            }

        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------------//
    //wall jumping
    //TODO:// move in opposite direction, currently only moves up
    void WallJump()
    {

        //HasJumped = false;
        if (transform.rotation.eulerAngles.y >= 1.0f && transform.rotation.eulerAngles.y <= 91.0f)
        {
            //movementDirection.x = -m_fHorizontalWallJumpForce;
            movementDirection.x = -m_fHorizontalWallJumpForce;
            movementDirection.y = m_fVerticalWallJumpForce;
            //m_cCharacterController.Move(Vector3.up * m_fVerticalWallJumpForce * Time.deltaTime);
            //m_cCharacterController.Move(movementDirection * Time.deltaTime * m_fJumpForce);
            //m_cCharacterController.Move(temp * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, -90, 0);

        }
        else if (transform.rotation.eulerAngles.y >= 181.0f && transform.rotation.eulerAngles.y <= 271.0f)
        {
            //movementDirection.x = m_fHorizontalWallJumpForce;
            movementDirection.x = m_fHorizontalWallJumpForce;
            movementDirection.y = m_fVerticalWallJumpForce;
            //m_cCharacterController.Move(Vector3.up * m_fVerticalWallJumpForce * Time.deltaTime);
            //m_cCharacterController.Move(movementDirection * Time.deltaTime * m_fJumpForce);
            //m_cCharacterController.Move(temp * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        //movementDirection.x += -this.transform.forward * m_fHorizontalWallJumpForce;
        //movementDirection.y += this.transform.up.y * m_fVerticalWallJumpForce;
        //movementDirection = -this.transform.forward * m_fHorizontalWallJumpForce;
        //m_bHitWall = false;
        Debug.Log(movementDirection);
        //m_cCharacterController.Move(movementDirection);
        //movementDirection = Vector3.up * m_fVerticalWallJumpForce;
        m_cState = CStates.OnFloor;
    }

    void Push()
    {
        RaycastHit hit;
        //shoots a raycast forward from the player, if it hits another player, it pushes them
        if (Input.GetButtonDown(this.playerNumber + "_AltFire"))
        {
            Vector3 rayOrigin = this.transform.position + new Vector3(0f, 0.5f, 0f);
            Debug.DrawLine(rayOrigin, rayOrigin + this.transform.forward);
            if (Physics.Raycast(rayOrigin, this.transform.forward, out hit, 0.5f))
            {
                if (hit.transform.tag == "Player")
                {
                    hit.transform.gameObject.GetComponent<LouisMovement>().movementDirection.x += this.transform.forward.x * this.m_fPushForce;
                    Debug.Log("Push");
                }
            }
        }
    }
    //Lincolns shit
    void Jump(CharacterController temp)     // Checks if the user can jump, then executes on command if possible.
    {


        // This is the Left/Right movement for X. always set Y to 0.

        m_fAirBourneTime += Time.deltaTime;

        if (temp.isGrounded)
        {
            movementDirection.y = refBlockController.m_fOverworldSpeed;
        }

        if (temp.isGrounded || m_fAirBourneTime <= m_fGroundBuffer)
        {

            HasJumped = false;
            HasDoubleJumped = false;


            if (!HasJumped && Input.GetButtonDown(playerNumber + "_Fire"))// if the players jump button is down
            {

                movementDirection.y = m_fJumpForce;
                m_fJumpTimer = 0.0f;
                m_fAirBourneTime = m_fGroundBuffer + 1f;
                HasJumped = true;


            }
        }

    }
    // Double Jump
    void DoubleJump(CharacterController temp)
    {
        if (!temp.isGrounded && m_fAirBourneTime >= m_fGroundBuffer)
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
    }
    void MovementCalculations()
    {
        movementDirection.x += (m_fMoveSpeed * -Input.GetAxis(playerNumber + "_Horizontal")); // Calculates X Movement
        if (m_cCharacterController.isGrounded && HasJumped == false)
        {
            movementDirection.y = -refBlockController.m_fOverworldSpeed + -1;
            m_fAirBourneTime = 0;
        }
        if (m_fAirBourneTime <= m_fGroundBuffer) // && temp is grounded? 10/19/2016
        {
            // Can't be equals
            movementDirection.y -= (m_fGravity * Time.deltaTime);
        }
        if (m_fAirBourneTime >= m_fGroundBuffer)
        {

            movementDirection.y -= (m_fGravity * Time.deltaTime);
        }
        if (movementDirection.y < -m_fMaxFallSpeed)     // Prevents passing max fall speed
        {
            movementDirection.y = -m_fMaxFallSpeed;
        }


        if (movementDirection.x > 0.0f)
        {
            movementDirection.x -= 0.5f;                // if momemntum x > 0, reduce it.
        }
        else if (movementDirection.x < 0.0f)
        {
            movementDirection.x += 0.5f;                // if momemntum x < 0, reduce it.
        }


        if (movementDirection.x > -0.26f && movementDirection.x < 0.26f && movementDirection.x != 0.0f)
        {
            movementDirection.x = 0.0f;                 // if momemntum within a range of .26 set it to 0;

        }
        else
        {




            //-------------------------------------------------------------------------------------------------------------------------------------//
            if (movementDirection.x > 10)
            {
                movementDirection.x = 10;                   // Max speed settings
            }

            else if (movementDirection.x < -10)
            {
                movementDirection.x = -10;                   // Max speed settings
            }
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

    public void PlayerStun()
    {
        StunRelease();
        m_bIsStunned = true;
        m_cState = CStates.Stunned;
        m_fCurrentStunTime += Time.deltaTime;
        if (m_fCurrentStunTime >= m_fMaxStunTime)
        {
            m_bIsStunned = false;
            m_cState = CStates.OnFloor;
            Debug.Log(playerNumber + " Leave stun");
            m_fCurrentStunTime = 0;
        }
        movementDirection.y = -5f;

    }

    void StunRelease()
    {
        if (m_iQuickRelease >= iReleaseCount) //sets quick release to 0 and releases stun
        {
            m_bIsStunned = false;
            m_cState = CStates.OnFloor;
            m_iQuickRelease = 0;
        }


        if (Input.GetButtonDown(playerNumber + "_Release")) // xbox controles
        {
            ++m_iQuickRelease;

        }
    }
    public void PlayerKick(CharacterController Temp)
    {
        if (!Temp.isGrounded && Input.GetButtonDown(playerNumber + "_Kick"))
        {
            m_bIsKicking = true;
            PlayerTurnAround();
            if (transform.rotation == Quaternion.Euler(0, -90, 0))
            {
                movementDirection.y = -20f + -refBlockController.m_fOverworldSpeed;
                movementDirection.x = -10f;
            }
            else
            {
                movementDirection.y = -20f + -refBlockController.m_fOverworldSpeed;
                movementDirection.x = 10f;
            }
        }
        m_fCurrentKickTime += Time.deltaTime;
        // m_fMaxFallSpeed = 20f;
        if (m_bIsKicking == true)
        {
            if (m_fCurrentKickTime >= m_fMaxKickTime || Temp.isGrounded)
            {
                m_bIsKicking = false;
                ref_KickHitBox.SetActive(false);
                m_fCurrentKickTime = 0f;
                m_cState = CStates.OnFloor;
            }
            else
            {
                movementDirection.y = -20f;
                if (movementDirection.x > 0)
                {
                    movementDirection.x = 10f;
                }
                else
                    movementDirection.x = -10f;
                ref_KickHitBox.SetActive(true);
                m_bIsKicking = true;
                m_cState = CStates.Kicking;
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall")
        {
            if ((m_cCharacterController.collisionFlags & CollisionFlags.Above) != 0)
            {
                movementDirection.y = 0;
            }
        }
    }


}
