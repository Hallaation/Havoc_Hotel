using UnityEngine;
using System.Collections;

public class cutsceneScript : MonoBehaviour {

    public MovieTexture movie;
    private AudioSource audio;
    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.mainTexture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

    }
    
   public void Play()
   {
       movie.Play();
   }

   // this line of code will make the Movie Texture begin playing
    //((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();


}
