using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float m_fOverworldSpeed;

    //  public static Object prefab = Resources.Load("HavocHotel/Prefabs/Levels/Level_001");
    //  public static BlockController Create()
    //  {
    //      GameObject newObject = Instantiate(prefab) as GameObject;
    //      BlockController yourObject = newObject.GetComponent<BlockController>();
    //      //do additional initialization steps here
    //      return yourObject;
    //  }
    //


    // List<GameObject> prefabList = new List<GameObject>();
    // public GameObject Prefab1;
    // public GameObject Prefab2;
    // public GameObject Prefab3;

  //  public GameObject prefab;


    // Use this for initialization
    void Start()
    {
        m_fOverworldSpeed = 0.0f;
        // prefabList.Add(Prefab1);
        // prefabList.Add(Prefab2);
        // prefabList.Add(Prefab3);
        //
        // int prefabIndex = UnityEngine.Random.Range(0, 3);
        // Instantiate(prefabList[prefabIndex]);


     //   for (int i = 0; i < 10; i++)
      //      Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
    }

    void Update()
    {
        m_fOverworldSpeed += Time.deltaTime;
    }
}

// Update is called once per frame


