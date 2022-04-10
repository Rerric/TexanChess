using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public float pieceID; //unique Piece ID in the game

    public int team; //what team this piece is on

    public bool isKing; //"Only one may be king.." -some guy

    public bool myTurn; //whether or not it's this piece's turn

    private ThirdPersonMovement moveScript;

    public GameObject gameManager;

    public Transform pieceTransform; //this piece's transform 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        moveScript = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        var gmScript = gameManager.GetComponent<GameManager>();
        if (gmScript.turn == pieceID)
        {
            gmScript.pieceToFollow = pieceTransform; //tell the camera to follow this piece
            myTurn = true;
        }
        else myTurn = false;

        if (myTurn == true)
        {
            moveScript.enabled = true;
        }
        else
        {
            moveScript.enabled = false;
        }

    }
}
