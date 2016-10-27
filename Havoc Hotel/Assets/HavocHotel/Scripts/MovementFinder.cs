using UnityEngine;
using System.Collections;

public class MovementFinder : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponentInParent<Movement>().refPlayerStartText = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
