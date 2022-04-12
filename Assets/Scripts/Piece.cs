using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public float pieceID; //unique Piece ID in the game

    public int team; //what team this piece is on

    public bool isKing; //"Only one may be king.." -some guy

    public bool myTurn; //whether or not it's this piece's turn

    public float healthMax;
    public float health;

    private ThirdPersonMovement moveScript;
    public CharacterController controller;
    public CapsuleCollider collider;
    public Rigidbody rigidbody;

    public GameObject gameManager;

    public Transform pieceTransform; //this piece's transform 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        moveScript = GetComponent<ThirdPersonMovement>();

        health = healthMax;
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
            //while it's myTurn : control me and ignore natural physics
            gameObject.layer = 7;
            moveScript.enabled = true;
            controller.enabled = true;
            collider.enabled = false;
            rigidbody.isKinematic = true;
        }
        else 
        {
            //otherwise : I can't be controlled and physics will affect me
            gameObject.layer = 6;
            moveScript.enabled = false;
            controller.enabled = false;
            collider.enabled = true;
            rigidbody.isKinematic = false;
        }

    }
}
