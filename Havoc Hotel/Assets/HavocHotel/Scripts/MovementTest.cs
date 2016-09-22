using UnityEngine;
using System.Collections;

public class MovementTest : MonoBehaviour
{
    const float _SPEED = 10.0f;
    const int _ROTATION_SPEED = 20;
<<<<<<< HEAD
    const float _JUMP_SPEED = 20.0f;
=======
    const float _JUMP_SPEED = 35.0f;
>>>>>>> 376b3c41c2390157c4b31ce1f74665fe2ad3aa1b
    const float _GRAVITY = 100.0f;
    public Transform lookAt;
    bool wentThrough = false;
    bool disable = true;
<<<<<<< HEAD
    float timer = 0.0f;
    private Vector3 movementDirection = Vector3.zero;

    public int PlayerNumber;
    public GameObject platformController;

    //update every frame
    void Update()
    {
        CharacterController temp = GetComponent<CharacterController>();
        if (temp.isGrounded || disable)
        {
            this.transform.position -= new Vector3(Input.GetAxis(PlayerNumber +"_Horizontal") * Time.deltaTime * _SPEED , 0);
            if (temp.isGrounded)
            {
                if (Input.GetButton(PlayerNumber + "_Fire") && Input.GetAxis(PlayerNumber + "_Vertical") >= 0)
                {
                    transform.position += new Vector3(0 , _JUMP_SPEED * Time.deltaTime);
                    movementDirection.y = _JUMP_SPEED;
                    //Debug.Log("Jumping");
                }
            }
        }
        timer += Time.deltaTime;
        
        movementDirection.y -= _GRAVITY * Time.deltaTime;
        temp.Move(movementDirection * Time.deltaTime);

        //if (wentThrough)
        //{
        //    foreach (Collider i in platformController.GetComponentsInChildren<Collider>())
        //    {
        //        Physics.IgnoreCollision(temp , i , false);
        //        Debug.Log(i.name);
        //    }
        //}

        if (Input.GetButton(PlayerNumber + "_Fire") && Input.GetAxis(PlayerNumber + "_Vertical") < 0)
        {
            JumpDown();
        }
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
=======
    private bool m_bFlag = false;
    float timer = 0.0f;
    private Vector3 movementDirection;
    private float m_fJumpTimer = 0.0f;
    public GameObject platformController;
    bool HasJumped;
    public int playerNumber;
    //update every frame
    void Update()
    {

        CharacterController temp = GetComponent<CharacterController>();
        if (temp.isGrounded || disable)
        {
            this.transform.position += new Vector3(Input.GetAxis(playerNumber + "_Horizontal") * Time.deltaTime * _SPEED, 0);       // This is the Left/Right movement for X. always set Y to 0.
            HasJumped = false;



            if (temp.isGrounded)
            {
                m_fJumpTimer = 0.0f;
                if (Input.GetButton("5_Vertical"))
                {
                    
                    movementDirection.y = _JUMP_SPEED;
                    UnityEngine.Debug.Log("Jumping");
                    HasJumped = true;
                }
            }
        }
        m_fJumpTimer += Time.deltaTime;
        timer += Time.deltaTime;

        //if(m_fJumpTimer <= 150)
        //{

        //}


        temp.Move(movementDirection * Time.deltaTime);

        if (movementDirection.y > 0)
        {
            UnityEngine.Debug.Log("movement dir y > 0");
            this.transform.position += new Vector3(0, _SPEED * Time.deltaTime);
        }
        if (movementDirection.y == 0.0f)
        {
            this.transform.position += new Vector3(0, _SPEED * Time.deltaTime);
            UnityEngine.Debug.Log ("movement dir y == 0");
        }
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
		if (!temp.isGrounded)
		{
			//UnityEngine.Debug.Log("Hit something");
			if (hit.collider.name.Contains("Platform")) // change later to tag
			{
				if (!wentThrough)
				{
					Physics.IgnoreCollision(temp , hit.collider);
                    UnityEngine.Debug.Log(hit.collider.name);
					UnityEngine.Debug.Log(GetComponent<Collider>().name);
				}
			}
		}

		if (Input.GetKey(KeyCode.K))
		{
			UnityEngine.Debug.Log(wentThrough);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		UnityEngine.Debug.Log("TRIGGERED");
	}
>>>>>>> 376b3c41c2390157c4b31ce1f74665fe2ad3aa1b

        CharacterController temp = GetComponent<CharacterController>();
        wentThrough = true;
        Physics.IgnoreCollision(temp , other.GetComponentInParent<Collider>() , false);
        //Debug.Log(other.transform.parent.GetComponent<Collider>().name);

<<<<<<< HEAD
    }
=======
		CharacterController temp = GetComponent<CharacterController>();
		wentThrough = true;
		Physics.IgnoreCollision(temp , other.transform.parent.GetComponent<Collider>() , false);
		//UnityEngine.Debug.Log(other.transform.parent.GetComponent<Collider>().name);
>>>>>>> 376b3c41c2390157c4b31ce1f74665fe2ad3aa1b

    void JumpDown()
    {
        Debug.Log("Jumped down");
    }
}
