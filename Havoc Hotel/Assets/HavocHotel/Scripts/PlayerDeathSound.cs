using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerDeathSound : MonoBehaviour
{
    public GameObject camera;
    public AudioSource audio;

    public float rotateAmount;
    bool hasRotated;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //rotateAmount *= Time.deltaTime;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
          audio.Play();
         // Shake();
        }
         //AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    //*not working*
  //void Shake()
  //{
  //    if(hasRotated == false)
  //    {
  //      camera.transform.Rotate(0, 0, rotateAmount);
  //        camera.transform.Rotate(0, 0, -rotateAmount);
  //        camera.transform.Rotate(0, 0, -rotateAmount);
  //        camera.transform.Rotate(0, 0, rotateAmount);
  //        camera.transform.Rotate(0, 0, rotateAmount);
  //        camera.transform.Rotate(0, 0, -rotateAmount);
  //        camera.transform.Rotate(0, 0, -rotateAmount);
  //        hasRotated = true;
  //    }
  //    else if(hasRotated == true)
  //    {
  //        camera.transform.Rotate(0, 0, rotateAmount);
  //        hasRotated = false;
  //    }
  //}
}





