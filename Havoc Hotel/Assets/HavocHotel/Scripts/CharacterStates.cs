using UnityEngine;
using System.Collections;

public class CharacterStates : MonoBehaviour
{
    public MovementTest m_refMovement;
    // Use this for initialization
    void Start()
    {
        Debug.Log("wall checker online");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Debug.Log("Entered something");
            Debug.Log("You should now be in a wall jumping state");
            m_refMovement.m_cState = States.CharacterState.Wall;
        }
    }

    void OnTriggerExit(Collider a_collision)
    {
        m_refMovement.m_cState = States.CharacterState.Ground;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("I entered something");
    }
}
