using UnityEngine;
using System.Collections;

public class ArrowMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

   
            this.transform.localPosition += Vector3.up * 2 * Time.deltaTime;
        

	}
}
