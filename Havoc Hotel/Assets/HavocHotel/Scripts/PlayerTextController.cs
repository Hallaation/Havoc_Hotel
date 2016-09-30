using UnityEngine;
using System.Collections;

public class PlayerTextController : MonoBehaviour {

    public LouisMovement refPlayer1;
    public LouisMovement refPlayer2;
    public LouisMovement refPlayer3;
    public LouisMovement refPlayer4;


    public UnityEngine.UI.Text txtPlayer1;
    public UnityEngine.UI.Text txtPlayer2;
    public UnityEngine.UI.Text txtPlayer3;
    public UnityEngine.UI.Text txtPlayer4;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        txtPlayer1.text = (refPlayer1.m_bIsDead) ? "Player 1: Dead" : "Player 1: Alive";
        txtPlayer2.text = (refPlayer2.m_bIsDead) ? "Player 2: Dead" : "Player 2: Alive";
        txtPlayer3.text = (refPlayer3.m_bIsDead) ? "Player 3: Dead" : "Player 3: Alive";
        txtPlayer4.text = (refPlayer4.m_bIsDead) ? "Player 4: Dead" : "Player 4: Alive";
    }
}
