using UnityEngine;
using System.Collections;

public class MoveLevel : MonoBehaviour
{
    public float m_fLevelSpeed = 1.0f; //-modifiable value for designers (modifiable in unity)- initial speed of level
    public float m_fSpeedIncrease = 1.1f; //-modifiable value to change rate at which level increases in speed

    public BlockController refController;


    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(Mathf.Clamp(Time.time, 0.0F, 3.0F), 0, 0);

        m_fLevelSpeed += Time.deltaTime;

        transform.Translate(Vector3.down * refController.m_fOverworldSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("triggered");
            Destroy(this.gameObject);
            //refController.Spawn();
        }
    }

   

}
