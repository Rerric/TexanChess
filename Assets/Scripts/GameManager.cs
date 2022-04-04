using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    PlayerControls controls;

    //Game Initialization
    public int Players; //number of active players

    public int teams; //number of different teams in the game

    public int turn; //what team's turn it currently is

    //public List<int> pieces = new List<int>(); //stores piece ID's

    public Transform[] pieceTransforms;

    //Important Scripts to communicate with
    public CameraController cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Gameplay.Enable();

        controls.Gameplay.Fire.performed += ctx => NextTurn(); //controls here are enabled mainly for debugging and testing

        teams = Players;
        turn = 1; //team 1 starts first

        cameraScript = GameObject.Find("Third Person Camera").GetComponent<CameraController>();

        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    void GameStart()
    {
        //SpawnTeams();
    }

    /*void SpawnTeams(int teams)
     *  {
     *      //find team spawnpoints based on the current level
     *      //determine number of pieces to spawn
     *      //spawn pieces accordingly
     *      //Grant piece ID's based on order spawned
     *      //Add their ID's to the List (pieces)
     *  }
    */

    void UpdateCamera()
    {
        var pieceToFollow = pieceTransforms[turn - 1];

        //tell the camera what object (Transform) to follow
        cameraScript._follow = pieceToFollow;
        cameraScript._lookat = pieceToFollow;

    }

    public void NextTurn()
    {
        turn += 1;
        if (turn > teams)
        {
            turn = 1;
        }
    }
}
