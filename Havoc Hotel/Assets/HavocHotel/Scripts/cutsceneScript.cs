using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class cutsceneScript : MonoBehaviour {

    GameObject[] players ;
    //players[0].SetActive(false);

    public MovieTexture movie;
    //private AudioSource audio;
    // Use this for initialization
    void Awake () {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        players = GameObject.FindGameObjectsWithTag("Player");
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }

        movie.Play();
        //audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        // when input enable players and load next scene

        if(Input.GetKeyDown("p"))
        {
            movie.Stop();
            players[0].SetActive(true);
            SceneManager.LoadScene(2);
        }

        if(!movie.isPlaying)
        {
            foreach (GameObject player in players)
            {
                player.SetActive(true);
            }
            SceneManager.LoadScene(2);
        }

    }
    
    


}
