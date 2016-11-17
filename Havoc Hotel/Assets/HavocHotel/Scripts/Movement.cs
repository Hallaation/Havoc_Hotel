using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum CStates
{
    OnFloor,
    OnWall,
    Kicking,
    Stunned
}

public class Movement : MonoBehaviour
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
    public float m_fTimeSinceLastPush = 99;
    public float m_fPushCoolDown = 1;
    public bool m_bHasPushed = false;
    #endregion
    //wall jump stuff
    #region
    public float m_fHorizontalWallJumpForce = 20.0f; //how far the wall jump pushes it away from the wall horizontally --> || <--
    public float m_fVerticalWallJumpForce = 15.0f; //how far it pushes the player up from the wall. ^ || v
    public float m_fTurnDelay = 1.0f; //Delay when turning away from the wall
    public float m_fWallSlidingSpeed = 0.5f;
    public float m_fMaxWallSlideSpeed;
    public float m_fNoSpeedLimitDuration; // how long does the player have no x speed limit after wall jumping.
    public float m_fWallSlideUpReduction;
    public float m_fFloorWallReduction;
    private float m_fWallSlideSpeed = 0.5f; //wall sliding speed public so it can be edited outside of code
    private float m_fTimeSinceWallJump = 999;
    private float m_fAirReduction;

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
    public float m_fKickKnockBack = 10;
    float m_fTimeSinceLastKick;
    public float m_fKickCoolDown = 5;
    float m_fCurrentKickTime;
    float m_fCurrentStunTime;
    #endregion


    //quick release / Player Status
    #region
    private int m_iQuickRelease;
    public int iReleaseCount = 10; //amount of times a player has to press to get out of stun.

    public bool m_bIsStunned;
    public bool m_bIsDead;
    private bool m_bEmitParticle;
    public Vector3 movementDirection;
    #endregion

    //references
    #region
    public GameObject ref_KickHitBox;
    private CharacterController m_cCharacterController; //character controller reference
    public BlockController refBlockController; //Block controller reference to know what the world speed is.
    private bool m_bIsWinner = false;
    public GameObject refPlayerStartText;
    public PlayerTextController ref_PlayerArray; //Now unused. Was used to control the players to "press start" to join. this is now done in the main menu. Should be used show player death messages.
    public GameObject ref_WallHitBox;
    public SpriteRenderer refPlayerStatus; //Text to show the player status. 
    public Sprite[] m_sStatusSprites;
    public Sprite m_sWinSprite;
    public int m_iCounter;

    #endregion

    //Animation
    #region
    Animator m_aAnimator;
    public float m_fAnimationSpeed = 1f;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

    //props
    public bool IsPlaying { get { return m_bIsPlaying; } set { m_bIsPlaying = value; } }

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
        m_iCounter = 0;
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
        m_aAnimator = GetComponentInChildren<Animator>();
        //ref_WallHitBox;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_fAirReduction = m_fWallSlideUpReduction;

        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void OnTriggerEnter(Collider other)
    {
        #region
        if (other.tag == "Killer")
        {
            GetComponent<AudioSource>().Play();
            //ps.gameObject.SetActive(false);
            //this.transform.position = new Vector3(0, -60);
            m_bIsDead = true;
        }
        else
        {
            Physics.IgnoreCollision(m_cCharacterController, other.GetComponent<Collider>());
        }

        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //once exiting the trigger, the parent's collider will no longer ignore collisions
    void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(m_cCharacterController, other.GetComponent<Collider>(), false);
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //update every frame
    //reset the z position ... essentially clamping the player to the z, never falling forward.
    void FixedUpdate()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.5f);
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //Lincoln's messy code
    void Update()
    {

        //refPlayerStatus.sprite = mySprites[m_iCounter];
        #region
        m_fTimeSinceWallJump += Time.deltaTime;
        if (m_bGameRunning == true)
        {
            //begin of mess
            CharacterController temp = GetComponent<CharacterController>();

            if (refPlayerStatus)
            {
                refPlayerStatus.sprite = (m_bIsDead) ? m_sStatusSprites[1] : m_sStatusSprites[0];
            }
            if (m_bIsPlaying)
            {
                #region if IsPlaying


                if (!m_bIsDead)
                {
                    //HeadCheck(); //check for head collisions
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
                            MovementCalculations();
                            temp.Move(new Vector3(movementDirection.x * Time.deltaTime, Time.deltaTime * movementDirection.y));
                            break;

                        case CStates.OnFloor:
                            m_aAnimator.SetBool("IsDiveKick", false);
                            m_aAnimator.SetBool("IsWallGrab", false);
                            OnFloor();

                            if (!m_bHasPushed)
                            {
                                Push();
                            }
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
                            m_aAnimator.SetBool("IsDiveKick", false);

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
                #endregion
            }
        }

        if (m_bHasPushed == true && m_fTimeSinceLastPush >= .1)
        {
            m_aAnimator.SetBool("IsPushing", false);
            m_bHasPushed = false;
        }
        m_fTimeSinceLastPush += Time.deltaTime;
        //refPlayerStatus.text = "Press Start to join";



        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //on floor movement
    void OnFloor()
    {
        #region
        //ray cast head up to find if you are hitting something to pull you back down.

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

        ref_KickHitBox.SetActive(false);
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void PushCheck()
    {
        #region
        if (m_bIsPushed)
        {
            m_fMaxSpeedX = int.MaxValue;
            m_fTimeSinceLastPush += Time.deltaTime;

            if (m_fTimeSinceLastPush >= m_fPushTime)
            {
                m_fTimeSinceLastPush = 0;
                m_bIsPushed = false;
            }
        }
        else
        {
            m_fMaxSpeedX = m_fTempMoveSpeedX;
        }

        //Cooldown stuff;
        //if (m_bHasPushed)
        //{
        //    m_fPushCooldownTimer += Time.deltaTime;
        //    if (m_fPushCooldownTimer >= m_fPushCooldown)
        //    {
        //        m_bHasPushed = false;
        //        m_fPushCooldownTimer = 0;
        //    }
        //}
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //If the wall is hit, the character will slide slowly on the wall.
    public void WallSlide()
    {
        #region
        m_bHitWall = true;
        if (m_bHitWall)                                                                                 // Got rid of math clamp
        {
            movementDirection.x = 0;
            m_bIsKicking = false;
            ref_KickHitBox.SetActive(false);
            if (movementDirection.y > 0)
            {
                movementDirection.y -= m_fWallSlideUpReduction * Time.deltaTime; // Deltatime added
            }

            m_aAnimator.SetBool("IsWallGrab", true);
            //if (m_fWallSlideSpeed > m_fMaxWallSlideSpeed + refBlockController.m_fOverworldSpeed)
            //{
            //    m_fWallSlideSpeed = -(refBlockController.m_fOverworldSpeed + m_fMaxWallSlideSpeed);
            //}
            //short delay when moving away from wall

            bool horizontalActive = Input.GetAxis(playerNumber + "_Horizontal") != 0;
            m_fButtonTimer += 0.05f * System.Convert.ToByte(horizontalActive);

            if (m_fButtonTimer >= m_fTurnDelay)
            {
                PlayerTurnAround();
                m_fButtonTimer = 0.0f;
            }


            if (movementDirection.y <= m_fMaxWallSlideSpeed + refBlockController.m_fOverworldSpeed)
            {
                movementDirection.y -= m_fWallSlidingSpeed * Time.deltaTime;
            }
            m_cCharacterController.Move(new Vector3(0, Time.deltaTime * movementDirection.y));

            if (Input.GetButtonDown(playerNumber + "_Fire"))
            {
                WallJump();
            }
        }

        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    //wall jumping
    //TODO:// move in opposite direction, currently only moves up
    void WallJump()
    {
        #region
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
        m_fTimeSinceWallJump = 0;
        m_aAnimator.SetBool("IsWallGrab", false);
        m_aAnimator.SetBool("IsJumping", true);
        m_cState = CStates.OnFloor;
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void Push()
    {


        #region
        int m_iLayerMask = 1 << 8;

        RaycastHit hit;
        Movement referencedMovement;
        //shoots a raycast forward from the player, if it hits another player, it pushes them
        if (m_cCharacterController.isGrounded && m_fAirBourneTime <= m_fGroundBuffer)
        {
            if (Input.GetButtonDown(this.playerNumber + "_AltFire") && m_fTimeSinceLastPush >= m_fPushCoolDown)
            {
                //ray origin is from the middle of the player at 0.5f
                Vector3 rayOrigin = this.transform.position + new Vector3(0f, 0.5f, 0f);
                Debug.DrawLine(rayOrigin, rayOrigin + this.transform.forward);
                Debug.DrawLine(this.transform.position - new Vector3(0f, 0f, 0f), (this.transform.position - new Vector3(0f, 0f, 0f) + this.transform.forward));
                m_aAnimator.SetBool("IsPushing", true);
                m_bHasPushed = true;
                m_fTimeSinceLastPush = 0;
                if (Physics.Raycast(rayOrigin, this.transform.forward, out hit, m_fPushDistance, m_iLayerMask)
                    || Physics.Raycast(this.transform.position - new Vector3(0f, 0.3f, 0f), this.transform.forward, out hit, m_fPushDistance, m_iLayerMask)
                    || Physics.Raycast(this.transform.position + new Vector3(0f, 0.8f, 0f), this.transform.forward, out hit, m_fPushDistance, m_iLayerMask))

                {
                    if (hit.transform.tag == "Player")
                    {
                        m_bHasPushed = true;
                        Debug.Log("Hit");
                        referencedMovement = hit.transform.gameObject.GetComponent<Movement>();
                        //hit.transform.gameObject.GetComponent<LouisMovement>().m_cCharacterController.Move(new Vector3(m_fPushForce * Time.deltaTime, 0, 0));
                        referencedMovement.m_bIsPushed = true;
                        referencedMovement.movementDirection.x = this.transform.forward.x * m_fPushForce;
                    }
                }
            }
        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void Jump(CharacterController temp)     // Checks if the user can jump, then executes on command if possible.
    {
        #region
        if (m_cState != CStates.OnWall)
        {
            // m_fWallSlideUpReduction = m_fAirReduction;
        }
        // This is the Left/Right movement for X. always set Y to 0.

        m_fAirBourneTime += Time.deltaTime;
        if (HasJumped == true && m_bShortHop == false)
        {
            m_fTimeSinceJump += Time.deltaTime;
            if (m_fTimeSinceJump < m_fJumpHeightBuffer && Input.GetButtonUp(playerNumber + "_Fire"))
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
            m_aAnimator.SetBool("IsJumping", false);
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
                m_aAnimator.SetBool("IsJumping", true);

            }
        }


        #endregion
    }
    // Double Jump
    void DoubleJump(CharacterController temp)
    {
        #region
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
                    transform.FindChild("Particle_Right_001").GetComponent<ParticleSystem>().Play();
                    transform.FindChild("Particle_Left_001").GetComponent<ParticleSystem>().Play();
                    movementDirection.y = m_fDoubleJumpMoveForce;

                    HasDoubleJumped = true;
                    m_aAnimator.SetBool("IsJumping", false);
                    m_aAnimator.SetBool("IsJumping", true);
                }
            }
        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void MovementCalculations()
    {
        #region
        if (m_cCharacterController.isGrounded && !m_bIsStunned)
        {
            movementDirection.x += (m_fMoveSpeed * -Input.GetAxis(playerNumber + "_Horizontal")); // Calculates X Movement
        }
        else if (!m_bIsStunned)
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

        if (movementDirection.x > -8f && movementDirection.x < 8f)
        {
            if (movementDirection.x > -0.26f && movementDirection.x < 0.26f)
            {
                movementDirection.x = 0.0f;                 // if momemntum within a range of .26 set it to 0;
                m_aAnimator.SetBool("IsRunning", false);
            }
            //else if (-Input.GetAxis(playerNumber + "_Horizontal") > 0.5 || -Input.GetAxis(playerNumber + "_Horizontal") < 0.5)
            //{
            //    m_aAnimator.SetBool("IsRunning", true);
            //
            //}
            else if (movementDirection.x < -1.26f || movementDirection.x > 1.26f)
            {
                m_aAnimator.SetBool("IsRunning", true);
            }
            m_aAnimator.SetBool("IsSliding", false);
        }
        else
        {
            //-------------------------------------------------------------------------------------------------------------------------------------//


            if (movementDirection.x > m_fMaxSpeedX && m_fTimeSinceWallJump > m_fNoSpeedLimitDuration)
            {
                movementDirection.x = m_fMaxSpeedX;                   // Max speed settings
            }

            else if (movementDirection.x < -m_fMaxSpeedX && m_fTimeSinceWallJump > m_fNoSpeedLimitDuration)
            {
                movementDirection.x = -m_fMaxSpeedX;                   // Max speed settings
            }
            if (-Input.GetAxis(playerNumber + "_Horizontal") < .5 && -Input.GetAxis(playerNumber + "_Horizontal") > -0.5)
            {
                m_aAnimator.SetBool("IsSliding", true);
                m_aAnimator.SetBool("IsRunning", false);

            }

        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    /// <summary>
    /// Turns the player around based on input.
    /// </summary>
    void PlayerTurnAround()
    {
        #region
        if (m_bGameRunning)
        {
            if (Input.GetAxis(playerNumber + "_Horizontal") > 0.5)
            {
                //x y z
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (Input.GetAxis(playerNumber + "_Horizontal") < -0.7)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            m_bHitWall = false;
        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    public void PlayerStun()
    {
        #region
        if (m_bIsStunned == false)
        {
            movementDirection.y = -m_fKickKnockBack + -refBlockController.m_fOverworldSpeed;  // fall speed when kicked is the kick force with the stage fall speed.
        }
        m_bIsStunned = true;
        m_cState = CStates.Stunned;
        m_fCurrentStunTime += Time.deltaTime;
        m_aAnimator.SetBool("IsStunned", true);
        if (m_fCurrentStunTime >= m_fMaxStunTime)
        {
            m_bIsStunned = false;
            m_cState = CStates.OnFloor;
            Debug.Log(playerNumber + " Leave stun");
            m_fCurrentStunTime = 0;
            m_aAnimator.SetBool("IsStunned", false);
        }

        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void StunRelease()
    {
        #region
        if (m_iQuickRelease >= iReleaseCount) //sets quick release to 0 and releases stun
        {
            m_bIsStunned = false;
            m_cState = CStates.OnFloor;
            m_iQuickRelease = 0;
            m_aAnimator.SetBool("IsStunned", false);
        }


        if (Input.GetButtonDown(playerNumber + "_Release")) // xbox controles
        {
            ++m_iQuickRelease;

        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    public void PlayerKick(CharacterController Temp)
    {
        #region
        if (!Temp.isGrounded && Input.GetButtonDown(playerNumber + "_Kick") && m_fTimeSinceLastKick > m_fKickCoolDown && m_bIsKicking == false)
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
                m_aAnimator.SetBool("IsDiveKick", false);
            }
            else
            {
                m_fMaxFallSpeed = m_fMaxKickSpeedY;
                m_fMaxSpeedX = m_fMaxKickSpeedX;
                movementDirection.y = -m_fKickYSpeed;
                m_aAnimator.SetBool("IsDiveKick", true);
                if (movementDirection.x > 0)
                {
                    movementDirection.x = m_fKickXSpeed;
                }
                else
                    movementDirection.x = -m_fKickXSpeed;
                ref_KickHitBox.SetActive(true);
                m_bIsKicking = true;
                m_cState = CStates.Kicking;
                m_fCurrentKickTime += Time.deltaTime;
            }
        }
        m_fTimeSinceLastKick += Time.deltaTime;

        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    public void DontDestroyOnLoad()
    {
        #region
        if (DestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void OnEnable()
    {
        SceneManager.sceneLoaded += LookForObjects;
    }
    //-------------------------------------------------------------------------------------------------------------------------------------//
    void LookForObjects(Scene a_scene, LoadSceneMode a_loadSceneMode)
    {
        #region 
        if (this)
        {
            movementDirection = Vector3.zero;
            Time.timeScale = 1;
            //look for references
            if (a_scene.buildIndex == 2)
            {
                m_bGameRunning = true;
                ParticleSystem ps = transform.FindChild("Particle_Death_001").GetComponent<ParticleSystem>();
                ps.gameObject.SetActive(true);
                Debug.Log("About to reference stuff in scene 2");
                m_aAnimator.SetBool("IsDancing", false);
                refBlockController = GameObject.Find("Level_Section_Spawner").GetComponent<BlockController>();

                this.m_bIsDead = false;
                this.IsPlaying = true;
                m_aAnimator.SetBool("IsDancing", false);


                if (playerNumber > 3)
                {
                    this.transform.position = GameObject.Find("Player4_Spawn").transform.position;
                    this.transform.rotation = GameObject.Find("Player4_Spawn").transform.rotation;
                    refPlayerStatus = GameObject.Find("Player" + 4 + "_Status").GetComponent<SpriteRenderer>();
                }
                else
                {
                    this.transform.rotation = GameObject.Find("Player4_Spawn").transform.rotation;
                    this.transform.position = GameObject.Find("Player" + (playerNumber + 1) + "_Spawn").transform.position;
                    refPlayerStatus = GameObject.Find("Player" + (playerNumber + 1) + "_Status").GetComponent<SpriteRenderer>();
                }


                GameObject.Find("UIManager").GetComponent<UIManager>().PlayersInScene = true;
                GameObject.Find("UIManager").GetComponent<UIManager>().GameStarted = true;
                //m_aAnimator.SetBool("IsDancing", false);
            }
            //look for spawn points to find where to put the player at the win screen.
            if (a_scene.buildIndex == 4)
            {
                if (this)
                {
                    m_bGameRunning = false;
                    if (!m_bIsDead)
                    {
                        this.transform.position = GameObject.Find("Player_Spawn").transform.position; // Sets position equal to spawn
                        this.transform.rotation = GameObject.Find("Player_Spawn").transform.rotation; // Sets rotation equal to spawn
                        GameObject.Find("WinSprite").GetComponent<SpriteRenderer>().sprite = m_sWinSprite;
                        m_aAnimator.SetBool("IsDancing", true); // makes player dance
                    }
                    else
                    {
                        m_bIsDead = true;
                        this.transform.position = GameObject.Find("Player_Dead").transform.position;   // Sets dead players to dead spawn
                        ParticleSystem ps = transform.FindChild("Particle_Death_001").GetComponent<ParticleSystem>();
                        ps.gameObject.SetActive(false);
                    }
                    IsPlaying = false;                              // Stops play moving during end scene
                }
            }
        }
        #endregion
    }

    void HeadCheck()
    {
        #region
        RaycastHit hit;
        Debug.DrawRay(this.transform.position + this.transform.up, Vector3.up, Color.black, 1);
        if (Physics.Raycast(this.transform.position + this.transform.up, Vector3.up, out hit, 0.4f))
        {

            if (hit.transform.tag == "Wall")
            {
                Debug.Log("Head is htting something");
                m_cState = CStates.OnFloor;
                //m_cCharacterController.Move(Vector3.down * 0.2f);
                movementDirection.y = 0;
                movementDirection.y -= (1.0f + refBlockController.m_fOverworldSpeed);
                // m_bIsKicking = false;
            }
        }
        #endregion
    }

    public void EmitDeathParticle()
    {
        ParticleSystem ps = transform.FindChild("Particle_Death_001").GetComponent<ParticleSystem>();
        ps.Play();
    }
}
