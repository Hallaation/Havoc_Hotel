using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlockController : MonoBehaviour
{
    #region Variables

    
    /// <summary>
    /// The default speed the levels will start with.
    /// </summary>
    public float m_fOverworldSpeed = 5.0f; //start speed of levels

    /// <summary>
    /// A maximum world speed. *20 is already a high number
    /// </summary>
    public float m_fOverworldMaxSpeed = 50.0f; //max speed overworld can go (for countering too high acceleration)
    /// <summary>
    /// Modifiable value to change how fast the world moves.
    /// </summary>
    public float m_fSpeedIncrease = 1.0f; //-modifiable value to change rate at which level increases in speed
    /// <summary>
    /// Time until the game starts, the higher the number, the longer it will wait.
    /// </summary>
    public float m_fTimeStart = 0.0f;

    public float m_fChunkHeight;

    public GameObject go;

    /// <summary>
    /// An array that randomly selects what to spawn. Be sure these are prefabs or everything breaks.
    /// </summary>
    public GameObject[] m_LevelChunk; // array to have list of modular level pieces

    public bool m_bRunning; // bool to control leel movent

    public bool m_bIsPaused; // seeing if game is paused
    #endregion
    // Use this for initialization
    void Start()
    {
        m_bRunning = false;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetButtonDown("0_LB"))
            {
                m_bRunning = false;
                Debug.Log("Pressed");
            }
            else if (Input.GetButtonDown("0_RB"))
            {
                m_bRunning = true;

            }
            if (Input.GetAxis("0_DpadH") > 0)
            {
                m_fOverworldSpeed += 1;
            }
            else if (Input.GetAxis("0_DpadH") < 0)
            {
                m_fOverworldSpeed -= 1;
            }
        }


        m_fTimeStart -= Time.deltaTime;//use delta time to have seconds until bRunning is true
        
        if (m_fTimeStart <= 0 && !m_bIsPaused)
        {
            m_bRunning = true;
        }
        else if (m_bIsPaused)
        {
            m_bRunning = false;
        }

        if (m_bRunning)
        {
            if (m_fOverworldSpeed < m_fOverworldMaxSpeed) //if state to make max speed posible
            {
                m_fOverworldSpeed += m_fSpeedIncrease * Time.deltaTime;
            }
        }

    }

    /// <summary>
    /// Used to control the spwaning. Once the spawned chunck moves away from the spawnbox, another one spawns.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other) // when level block leave box collider activate
    {
        if (this.tag == "Player")
        {
            Debug.Log("Player is trying something");
        }
        if (this.tag == "Spawner") // talking to box directly with "Spawner" tag
        {
            if (other.tag == "Level") // talking level block with "Level" tag
            {
                Spawn(); // activate Spawn() funct 
            }
        }

    }


    /// <summary>
    /// This will instantiate a chunk randomly selecting what is in the array. Array objects are set in the Unity editor
    /// </summary>
    void Spawn()
    {
        if (m_LevelChunk.Length != 0)
        {
            int iRandIndex = Random.Range(0, m_LevelChunk.Length); // rand list to choose level block or in this case "chunk"

            m_LevelChunk[iRandIndex].GetComponent<MoveLevel>().refController = this; //Make it reference this.

            if (go)
            {
                go = Instantiate(m_LevelChunk[iRandIndex], go.transform.position + Vector3.up * m_fChunkHeight, Quaternion.identity) as GameObject; //pick a level "chunk" from rand list
            }
        }
    }
}







