using UnityEngine;
using System.Collections;

public class ChunkDestroyer : MonoBehaviour {

    void OnTriggerEnter(Collider a_collider)
    {
        if (a_collider.transform.tag == "Level")
        {
            Destroy(a_collider.transform.parent.parent.parent.gameObject);
        }
    }
}
