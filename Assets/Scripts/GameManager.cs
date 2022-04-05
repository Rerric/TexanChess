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

    public GameObject[] pieces;

    public List<int> pieceIDs = new List<int>(); //stores piece ID's

    public Transform[] pieceTransforms;

    //Important Scripts & Objects to communicate with
    public CameraController cameraScript;

    public GameObject spawnPoint1; //Team 1 Spawn
    public GameObject spawnPoint2; //Team 2 Spawn
    private Vector3 spawn;
    public float spawnOffset; //Determines how far away pieces spawn from the king

    public Vector3[] spawnPoints1 = new Vector3[5];
    public Vector3[] spawnPoints2 = new Vector3[5];

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Gameplay.Enable();

        controls.Gameplay.Fire.performed += ctx => NextTurn(); //controls here are enabled mainly for debugging and testing

        teams = Players;
        turn = 1; //team 1 starts first

        cameraScript = GameObject.Find("Third Person Camera").GetComponent<CameraController>();

        FindSpawnPoints();
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    void GameStart()
    {
        SpawnTeams(teams);
    }

    void SpawnTeams(int teams)
    {
        //Get Spawn positions
        var spawn1 = spawnPoints1[0];
        var spawn2 = spawnPoints2[0];
        

        //Spawn King for each team
        for (var i = 0; i < teams; i++)
        { 
            if (i == 0) spawn = spawn1;
            if (i == 1) spawn = spawn2;

            var id = i + 1;

            var king = Instantiate(pieces[0], spawn, transform.rotation);
            var kingTransform = king.GetComponent<Transform>();
            var kingScript = king.GetComponent<Piece>();
            kingScript.isKing = true;
            kingScript.team = id;
            kingScript.pieceID = id;
            pieceIDs.Add(id);

            var counter = 1;

            for (var e = id + 2; e < 11; e += 2) //Spawn 4 pieces for each team
            {
                var _id = e;

                if (id == 1) //Spawn piece for team 1
                {
                    var piece = Instantiate(pieces[0], spawnPoints1[counter], transform.rotation);
                    var pieceTransform = piece.GetComponent<Transform>();
                    var pieceScript = piece.GetComponent<Piece>();
                    pieceScript.team = id;
                    pieceScript.pieceID = _id;
                    pieceIDs.Add(_id);
                }

                if (id == 2) //Spawn piece for team 2
                {
                    var piece = Instantiate(pieces[0], spawnPoints2[counter], transform.rotation);
                    var pieceTransform = piece.GetComponent<Transform>();
                    var pieceScript = piece.GetComponent<Piece>();
                    pieceScript.team = id;
                    pieceScript.pieceID = _id;
                    pieceIDs.Add(_id);
                }

                counter += 1;

            }   
        }
        


    }

    void FindSpawnPoints()
    {
        //Team 1
        spawnPoints1[0] = spawnPoint1.transform.position;
        spawnPoints1[1] = new Vector3(spawnPoints1[0].x, spawnPoints1[0].y, spawnPoints1[0].z + spawnOffset);
        spawnPoints1[2] = new Vector3(spawnPoints1[0].x, spawnPoints1[0].y, spawnPoints1[0].z - spawnOffset);
        spawnPoints1[3] = new Vector3(spawnPoints1[0].x - spawnOffset / 2, spawnPoints1[0].y, spawnPoints1[0].z + spawnOffset / 2);
        spawnPoints1[4] = new Vector3(spawnPoints1[0].x - spawnOffset / 2, spawnPoints1[0].y, spawnPoints1[0].z - spawnOffset / 2);

        //Team 2
        spawnPoints2[0] = spawnPoint2.transform.position;
        spawnPoints2[1] = new Vector3(spawnPoints2[0].x, spawnPoints2[0].y, spawnPoints2[0].z + spawnOffset);
        spawnPoints2[2] = new Vector3(spawnPoints2[0].x, spawnPoints2[0].y, spawnPoints2[0].z - spawnOffset);
        spawnPoints2[3] = new Vector3(spawnPoints2[0].x + spawnOffset / 2, spawnPoints2[0].y, spawnPoints2[0].z + spawnOffset / 2);
        spawnPoints2[4] = new Vector3(spawnPoints2[0].x + spawnOffset / 2, spawnPoints2[0].y, spawnPoints2[0].z - spawnOffset / 2);
    }

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
