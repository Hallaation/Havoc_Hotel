using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerDeathSound : MonoBehaviour
{
    public AudioSource audio;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audio.Play();
        }
        // AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}





