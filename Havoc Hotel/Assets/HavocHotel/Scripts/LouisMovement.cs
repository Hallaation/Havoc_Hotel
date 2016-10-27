﻿using UnityEngine;
using System.Collections;



public class LouisMovement : MonoBehaviour
{
    //ALL THE PUBLIC VARIABLES.
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    //** Ignore booleans
    public int playerNumber; //Input manager to know which joypad number to use
  

    public CStates m_cState;
    //movement stuff
    #region
    public float m_fGravity = 50f;
    public float m_fMoveSpeed = 1.4f;

    public bool HasJumped;
    public bool m_bCanJump;
    public bool HasDoubleJumped;
    public bool m_bAllowDoubleJumpAlways;
    public bool m_bIsPushed = false;
  
    public float m_fJumpForce = 25f;  /// <summary>
                                      
                                      /// how far the player moves up in a normal jump
                                      /// </summary>
    public float m_fDoubleJumpMoveForce = 15f; //how far the player moves up in a double jump
    public float m_fMaxFallSpeed = 15f; //maximum falling speed *terminal velocity
    public float m_fMaxSpeedX = 10.0f; //setting for setting the maximum amount of momentum allowed.
    public float m_fJumpHeightBuffer = .15f;
    private bool m_bShortHop = false;
    bool m_bJumpKeyReleased;
    float m_fJumpTimer;
    float m_fTempFallSpeed; // Do not initialize/edit
    float m_fTempMoveSpeedX;
    float m_fTimeSinceJump;
    private float m_fGroundBuffer = 0.036f;
    private float m_fAirBourneTime;
    #endregion
    //pushing stuff
    #region
    public float m_fPushDistance = 0.5f; //determines how far the raycast will travel
    public float m_fPushForce = 10.0f; //determines how far the player pushes the other player.
    public float m_fPushTime = 0.5f; //time the player will be pushed for
    float m_fPushTimer;
    #endregion
    //wall jump stuff
    #region
    public float m_fHorizontalWallJumpForce = 20.0f; //how far the wall jump pushes it away from the wall horizontally --> || <--
    public float m_fVerticalWallJumpForce = 15.0f; //how far it pushes the player up from the wall. ^ || v
    public float m_fTurnDelay = 1.0f; //Delay when turning away from the wall
    public float m_fWallSlideSpeed = 0.5f; //wall sliding speed public so it can be edited outside of code
    #endregion
    //dive kick stuff
    #region
    public bool m_bIsKicking;
    public float m_fMaxKickSpeedY = 25.0f;
    public float m_fMaxKickSpeedX = 25.0f;
    public float m_fMaxStunTime = 15.0f; //how long the player is stunned for.
    public float m_fMaxKickTime = 5f;  //
    public float m_fHeadBounceForce = 20f; //player head bounce when stunning another player
    public float m_fKickYSpeed = 20; //
    public float m_fKickXSpeed = 10; //
    
    float m_fTimeSinceLastKick;
    float m_fKickCoolDown;
    float m_fCurrentKickTime;
    float m_fCurrentStunTime;
    #endregion

    //quick release / Player Status
    #region
    private int m_iQuickRelease;
    public int iReleaseCount = 10; //amount of times a player has to press to get out of stun.

    public bool m_bIsStunned;
    public bool m_bIsDead;

    public Vector3 movementDirection;
    #endregion

    //references
    #region
    public GameObject ref_KickHitBox;
    private CharacterController m_cCharacterController; //character controller reference
    public UnityEngine.UI.Text refPlayerStatus;
    public BlockController refBlockController;

    public GameObject refPlayerStartText;
    public PlayerTextController ref_PlayerArray;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    


    //declaring time related variables
    const int _ROTATION_SPEED = 20; // Not used yet.
    #region
    const float m_f1FramePasses = 0.0170f;
    float m_fButtonTimer = 0.0f;

    float timer = 0.0f;
    #endregion

   




    //maximum downfall momentum
    private bool m_bHitWall; //checks to see if the wall was hit or not.

    /// <summary>
    /// Wall jumping forces, higher value for bigger push force, lower for less
    /// </summary>

   


    private bool m_bIsPlaying;


    public bool m_bGameRunning = false;

    private bool m_bDestroyOnLoad;


    public bool DestroyOnLoad { get { return m_bDestroyOnLoad; } set { m_bDestroyOnLoad = value; } }
    //txtPlayers[i].text = (refPlayers[i].m_bIsDead) ? txtPlayers[i].text = "Player " + (i + 1) + ": Dead" : txtPlayers[i].text = "Player " + (i + 1) + ":  Alive";
    void Start()
    {
        #region
        m_fTempFallSpeed = m_fMaxFallSpeed;
        m_fTempMoveSpeedX = m_fMaxSpeedX;
        //GameObject[] list = GameObject.FindObjectsOfType<GameObject>();
        //list[0].name.Contains
        m_bIsPushed = false;
        m_bIsPlaying = false;
        m_iQuickRelease = 0;
        ref_KickHitBox.SetActive(false);
        //have the charactercontroller variable reference something
        m_cCharacterController = GetComponent<CharacterController>();
        m_bIsDead = false;

        refPlayerStartText.SetActive(false);
        #endregion
    }
    
    void OnTriggerEnter(Collider other)
    {
        #region
        if (other.tag == "Finish")
        {
            Debug.Log("Player " + playerNumber + " died!");
            m_bIsDead = true;
        }
        else
        {
            Physics.IgnoreCollision(m_cCharacterController, other.GetComponentInParent<Collider>());


        }
        #endregion
    }


    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(m_cCharacterController, other.GetComponentInParent<Collider>(), false);
    }


    //-------------------------------------------------------------------------------------------------------------------------------------//
    //update every frame
    //reset the z position ... essentially clamping the player to the z, never falling forward.
    void FixedUpdate()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.5f);
    }
    //Lincoln's messy code
    void Update()
    {

        //begin of mess
        CharacterController temp = GetComponent<CharacterController>();

        if (refPlayerStatus)
        {
            refPlayerStatus.text = (m_bIsDead) ? "Player " + (playerNumber + 1) + ": Dead" : "Player " + (playerNumber + 1) + ": Alive";
        }
        if (m_bIsPlaying)
        {

            if (!m_bIsDead)
            {
                PushCheck(); //check to see if still pushed

                //if (m_bIsKicking)
                //{
                //    PlayerKick(m_cCharacterController);
                //}

                // end of mess
                if (temp.isGrounded)
                {
                    m_fAirBourneTime = 0f;
                }
                switch (m_cState)
                {
                    case CStates.Stunned:
                        PlayerStun();
                        StunRelease();
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

                        else if (m_cCharacterController.isGrounded)
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
        else if (m_bGameRunning)
        {
            refPlayerStatus.text = "Press Start to join";
            if (Input.GetButtonDown(playerNumber + "_Start"))
            {

                m_bIsPlaying = true;
                ref_PlayerArray.refPlayers.Add(this);
                refPlayerStartText.SetActive(false);
            }
        }

        #endregion
    }


    //on floor movement
    void OnFloor()
    {
        #region
        RaycastHit hit;

        Debug.DrawRay(this.transform.position + this.transform.up, Vector3.up, Color.black, 1);
        if (Physics.Raycast(this.transform.position + this.transform.up, Vector3.up, out hit, 0.2f))
        {

            if (hit.transform.tag == "Wall")
            {
                movementDirection.y = 0;
                movementDirection.y -= 1;
            }
        }


        m_fTimeSinceLastKick += Time.deltaTime;
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
        #endregion
    }

    void PushCheck()
    {
        #region
        if (m_bIsPushed)
        {
            m_fMaxSpeedX = int.MaxValue;
            m_fPushTimer += Time.deltaTime;

            if (m_fPushTimer >= m_fPushTime)
            {
                m_fPushTimer = 0;
                m_bIsPushed = false;
            }
        }
        else
        {
            m_fMaxSpeedX = m_fTempMoveSpeedX;
        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //If the wall is hit, the character will slide slowly on the wall.
    public void WallSlide()
    {
        #region
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

        #endregion
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
            //m_fMaxSpeedX = m_fHorizontalWallJumpForce;
            //m_bIsPushed = true;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            m_cState = CStates.OnFloor;
        }
        else if (transform.rotation.eulerAngles.y >= 181.0f && transform.rotation.eulerAngles.y <= 271.0f)
        {
            //movementDirection.x = m_fHorizontalWallJumpForce;
            movementDirection.x = m_fHorizontalWallJumpForce;
            movementDirection.y = m_fVerticalWallJumpForce;
            //m_fMaxSpeedX = m_fHorizontalWallJumpForce;
            //m_bIsPushed = true;
            //m_cCharacterController.Move(Vector3.up * m_fVerticalWallJumpForce * Time.deltaTime);
            //m_cCharacterController.Move(movementDirection * Time.deltaTime * m_fJumpForce);
            //m_cCharacterController.Move(temp * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 90, 0);
            m_cState = CStates.OnFloor;
        }

        m_cState = CStates.OnFloor;
    }

    void Push()
    {
        #region
        int m_iLayerMask = 1 << 8;

        RaycastHit hit;
        LouisMovement referencedMovement;
        //shoots a raycast forward from the player, if it hits another player, it pushes them
        if (Input.GetButtonDown(this.playerNumber + "_AltFire"))
        {
            //ray origin is from the middle of the player at 0.5f
            Vector3 rayOrigin = this.transform.position + new Vector3(0f, 0.5f, 0f);
            Debug.DrawLine(rayOrigin, rayOrigin + this.transform.forward);
            Debug.DrawLine(this.transform.position - new Vector3(0f, 0f, 0f), (this.transform.position - new Vector3(0f, 0f, 0f) + this.transform.forward));



            if (Physics.Raycast(rayOrigin, this.transform.forward, out hit, m_fPushDistance, m_iLayerMask)
                || Physics.Raycast(this.transform.position - new Vector3(0f, 0.3f, 0f), this.transform.forward, out hit, m_fPushDistance, m_iLayerMask)
                || Physics.Raycast(this.transform.position + new Vector3(0f, 0.8f, 0f), this.transform.forward, out hit, m_fPushDistance, m_iLayerMask))

            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("Hit");
                    referencedMovement = hit.transform.gameObject.GetComponent<LouisMovement>();
                    //hit.transform.gameObject.GetComponent<LouisMovement>().m_cCharacterController.Move(new Vector3(m_fPushForce * Time.deltaTime, 0, 0));
                    referencedMovement.m_bIsPushed = true;
                    referencedMovement.movementDirection.x = this.transform.forward.x * m_fPushForce;
                }
            }
        }
        #endregion
    }
    //Lincolns shit
    void Jump(CharacterController temp)     // Checks if the user can jump, then executes on command if possible.
    {
        #region

        // This is the Left/Right movement for X. always set Y to 0.

        m_fAirBourneTime += Time.deltaTime;
        if (HasJumped == true && m_bShortHop == false)
        {
            m_fTimeSinceJump += Time.deltaTime;
            if(m_fTimeSinceJump < m_fJumpHeightBuffer && Input.GetButton(playerNumber + "_Fire"))
            {
                movementDirection.y = movementDirection.y * 0.5f;
                m_bShortHop = true;
            }
        }
        if (temp.isGrounded)
        {
            movementDirection.y = refBlockController.m_fOverworldSpeed;
            
        }

        if (temp.isGrounded || m_fAirBourneTime <= m_fGroundBuffer)
        {

            HasJumped = false;
            HasDoubleJumped = false;
            m_bShortHop = false;

            if (!HasJumped && Input.GetButtonDown(playerNumber + "_Fire"))// if the players jump button is down
            {

                movementDirection.y = m_fJumpForce;
                m_fJumpTimer = 0.0f;
                m_fTimeSinceJump = 0f;
                m_fAirBourneTime = m_fGroundBuffer + 1f;
                HasJumped = true;


            }
        }

        #endregion
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
        #region
        if (m_cCharacterController.isGrounded)
        {
            movementDirection.x += (m_fMoveSpeed * -Input.GetAxis(playerNumber + "_Horizontal")); // Calculates X Movement
        }
        else
        {
            movementDirection.x += (m_fMoveSpeed * 0.9f * -Input.GetAxis(playerNumber + "_Horizontal"));
        }

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

            if (movementDirection.x > m_fMaxSpeedX)
            {
                movementDirection.x = m_fMaxSpeedX;                   // Max speed settings
            }

            else if (movementDirection.x < -m_fMaxSpeedX)
            {
                movementDirection.x = -m_fMaxSpeedX;                   // Max speed settings
            }
        }
        #endregion
    }

    /// <summary>
    /// Turns the player around based on input.
    /// </summary>
    void PlayerTurnAround()
    {

        if (Input.GetAxis(playerNumber + "_Horizontal") > 0.5)
        {
            //x y z
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (Input.GetAxis(playerNumber + "_Horizontal") < 0.5)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        m_bHitWall = false;
    }

    public void PlayerStun()
    {
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
        #region
        if (!Temp.isGrounded && Input.GetButtonDown(playerNumber + "_Kick"))
        {
            m_bIsKicking = true;
            PlayerTurnAround();
            if (transform.rotation == Quaternion.Euler(0, -90, 0))
            {
                movementDirection.y = m_fKickYSpeed - refBlockController.m_fOverworldSpeed;
                movementDirection.x = -m_fKickXSpeed; ;
            }
            else
            {
                movementDirection.y = m_fKickYSpeed - refBlockController.m_fOverworldSpeed;
                movementDirection.x = m_fKickXSpeed;
            }
        }
        m_fCurrentKickTime += Time.deltaTime;
        // m_fMaxFallSpeed = 20f;
        if (m_bIsKicking == true)
        {
            if (m_fCurrentKickTime >= m_fMaxKickTime || Temp.isGrounded)
            {
                m_fMaxFallSpeed = m_fTempFallSpeed;
                m_fMaxSpeedX = m_fTempMoveSpeedX;
                m_bIsKicking = false;
                ref_KickHitBox.SetActive(false);
                m_fCurrentKickTime = 0f;
                m_cState = CStates.OnFloor;
                m_fTimeSinceLastKick = 0;
            }
            else
            {
                m_fMaxFallSpeed = m_fMaxKickSpeedY;
                m_fMaxSpeedX = m_fMaxKickSpeedX;
                movementDirection.y = -m_fKickYSpeed;
                if (movementDirection.x > 0)
                {
                    movementDirection.x = m_fKickXSpeed;
                }
                else
                    movementDirection.x = -m_fKickXSpeed;
                ref_KickHitBox.SetActive(true);
                m_bIsKicking = true;
                m_cState = CStates.Kicking;
            }
        }
        #endregion
    }


}
