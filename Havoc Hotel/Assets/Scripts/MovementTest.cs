using UnityEngine;
using System.Collections;

public class MovementTest : MonoBehaviour
{
	const float _SPEED = 10.0f;
	const int _ROTATION_SPEED = 20;
	const float _JUMP_SPEED = 35.0f;
	const float _GRAVITY = 100.0f;
	public Transform lookAt;
	bool wentThrough = false;
	bool disable = true;
	float timer = 0.0f;
	private Vector3 movementDirection = Vector3.zero;

	public GameObject platformController;

	//update every frame
	void Update()
	{
		CharacterController temp = GetComponent<CharacterController>();
		if (temp.isGrounded || disable)
		{
			this.transform.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * _SPEED , 0);
			if (temp.isGrounded)
			{
				if (Input.GetButton("Jump"))
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
	}

	//this is where the collision is ignored. once the player hits a platform with the name "platform" in it the collisions for the player and this collider are ignored. which are re enabled later after the trigger exit
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		CharacterController temp = GetComponent<CharacterController>();
		if (!temp.isGrounded)
		{
			//Debug.Log("Hit something");
			if (hit.collider.name.Contains("Platform"))
			{
				if (!wentThrough)
				{
					Physics.IgnoreCollision(temp , hit.collider);
					Debug.Log(hit.collider.name);
					Debug.Log(GetComponent<Collider>().name);
				}
			}
		}

		if (Input.GetKey(KeyCode.K))
		{
			Debug.Log(wentThrough);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("TRIGGERED");
	}

	//once exiting the trigger, the parent's collider will no longer ignore collisions
	void OnTriggerExit(Collider other)
	{

		CharacterController temp = GetComponent<CharacterController>();
		wentThrough = true;
		Physics.IgnoreCollision(temp , other.transform.parent.GetComponent<Collider>() , false);
		//Debug.Log(other.transform.parent.GetComponent<Collider>().name);

	}
}
