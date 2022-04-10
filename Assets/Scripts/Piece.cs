using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public float pieceID; //unique Piece ID in the game

    public int team; //what team this piece is on

    public bool isKing; //"Only one may be king.." -some guy

    public GameObject gameManager;

    public Transform pieceTransform; //this piece's transform 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        var gmScript = gameManager.GetComponent<GameManager>();
        if (gmScript.turn == pieceID)
        {
            gmScript.pieceToFollow = pieceTransform;
        }
    }
}
