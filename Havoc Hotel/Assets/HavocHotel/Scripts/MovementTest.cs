using UnityEngine;
using System.Collections;

public class MovementTest : MonoBehaviour
{
    const float _SPEED = 10.0f;
    const int _ROTATION_SPEED = 20;
    const float _JUMP_SPEED = 20.0f;
    const float _GRAVITY = 100.0f;
    public Transform lookAt;
    bool wentThrough = false;
    bool disable = true;
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

        CharacterController temp = GetComponent<CharacterController>();
        wentThrough = true;
        Physics.IgnoreCollision(temp , other.GetComponentInParent<Collider>() , false);
        //Debug.Log(other.transform.parent.GetComponent<Collider>().name);

    }

    void JumpDown()
    {
        Debug.Log("Jumped down");
    }
}
