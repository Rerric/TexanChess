using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    PlayerControls controls;

    //Game Initialization
    public int Players; //number of active players

    public int teams; //number of different teams in the game

    public int turn; //what team's turn it currently is

    private bool gameStarted; //determines if the game has begun

    public GameObject[] piecePrefabs;

    public List<GameObject> pieces1 = new List<GameObject>(); //stores pieces
    public List<GameObject> pieces2 = new List<GameObject>();

    public int t1Count; //team 1's turn tracker variable
    public int t2Count; //team 2's turn tracker variable

    public GameObject cameraLookAt;
    public Transform pieceToFollow; //which piece the camera is currently following
    private Transform projectileToFollow; 
    public bool followingProjectile; //if the game is currently following a projectile

    //Important Scripts & Objects to communicate with
    public Camera mainCam;
    private CinemachineVirtualCamera overheadCam;
    public CameraController cameraScript;

    public bool isOverhead; //whether the view of the game is currently in the overhead view
    public bool isPaused; //whether the game is currently paused

    private Vector3 spawn;

    public GameObject[] spawnPoints1 = new GameObject[6];
    public GameObject[] spawnPoints2 = new GameObject[6];

    public Canvas defaultCanvas;
    public Canvas aimCanvas;
    public GameObject controlsText;
    public GameObject winner1;
    public GameObject winner2;
    public Image movementBarJuice;
    public GameObject chargeMeter;
    public Image chargeMeterJuice;
    public Image teamFlag;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Gameplay.Enable();

        Cursor.lockState = CursorLockMode.Locked;

        controls.Gameplay.EndTurn.performed += ctx => NextTurn(); //controls here are enabled mainly for debugging and testing
        controls.Gameplay.Toggle.performed += ctx => ToggleControls();
        controls.Gameplay.Overhead.performed += ctx => ToggleOverhead(); //toggles overhead / bird's eye view camera
        controls.Gameplay.Pause.performed += ctx => TogglePause();
        controls.Gameplay.Quit.performed += ctx => Application.Quit();

        teams = Players;
        turn = 1; //team 1 starts first
        t1Count = 2;
        t2Count = 1;

        cameraScript = GameObject.Find("Third Person Camera").GetComponent<CameraController>();
        overheadCam = GameObject.Find("Overhead Camera").GetComponent<CinemachineVirtualCamera>();

        isOverhead = false;

        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        UpdateUI();
        if (pieces1.Count == 0) winner2.SetActive(true);
        if (pieces2.Count == 0) winner1.SetActive(true);
    }

    void GameStart()
    {
        SpawnTeams(teams);
        gameStarted = true;
    }

    void SpawnTeams(int teams)
    {
        //Get Spawn positions
        var spawn1 = spawnPoints1[0].transform.position;
        var spawn2 = spawnPoints2[0].transform.position;
        

        //Spawn King for each team
        for (var i = 0; i < teams; i++)
        { 
            if (i == 0) spawn = spawn1;
            if (i == 1) spawn = spawn2;

            var id = 1;

            var king = Instantiate(piecePrefabs[0], spawn, transform.rotation);
            var kingTransform = king.GetComponent<Transform>();
            var kingScript = king.GetComponent<Piece>();
            kingScript.isKing = true;
            kingScript.team = i + 1;
            kingScript.pieceID = id;
            if (kingScript.team == 1) pieces1.Add(king);
            if (kingScript.team == 2) pieces2.Add(king);

            var counter = 1;

            for (var e = 0; e < 5; e++) //Spawn 4 pieces for each team
            {
                var _id = e + 2;

                if (kingScript.team == 1) //Spawn piece for team 1
                {
                    var piece = Instantiate(piecePrefabs[counter], spawnPoints1[counter].transform.position, transform.rotation);
                    var pieceTransform = piece.GetComponent<Transform>();
                    var pieceScript = piece.GetComponent<Piece>();
                    pieceScript.team = kingScript.team;
                    pieceScript.pieceID = _id;
                    pieces1.Add(piece);
                }

                if (kingScript.team == 2) //Spawn piece for team 2
                {
                    var piece = Instantiate(piecePrefabs[counter], spawnPoints2[counter].transform.position, transform.rotation);
                    var pieceTransform = piece.GetComponent<Transform>();
                    var pieceScript = piece.GetComponent<Piece>();
                    pieceScript.team = kingScript.team;
                    pieceScript.pieceID = _id;
                    pieces2.Add(piece);
                }

                counter += 1;

            }   
        }

    }

    void UpdateCamera()
    {

        //tell the camera what object (Transform) to follow
        if (pieceToFollow != null && followingProjectile == false) cameraScript.target = pieceToFollow;

        if (projectileToFollow != null && followingProjectile == true) cameraScript.target = projectileToFollow;

        if (pieceToFollow == null && projectileToFollow == null) cameraScript.target = null;
    }

    void UpdateUI()
    {
        if (followingProjectile || overheadCam.Priority > 10)
        {
            defaultCanvas.enabled = false;
            aimCanvas.enabled = false;
        }

        if (turn == 1) teamFlag.sprite = sprites[0];
        if (turn == 2) teamFlag.sprite = sprites[1];
    }

    public void NextTurn()
    {
        if (followingProjectile == false)
        {
            turn += 1;
            if (turn > teams)
            {
                turn = 1;
            }

            if (turn == 1)
            {
                t1Count += 1;
                if (t1Count > pieces1.Count) t1Count = 1;
            }

            if (turn == 2)
            {
                t2Count += 1;
                if (t2Count > pieces2.Count) t2Count = 1;
            }
        }
    }

    public void GoNext()
    {
        StartCoroutine(Next(3f));
    }

    public IEnumerator Next(float seconds) //tells the game to automatically go to the next turn 
    {
        yield return new WaitForSeconds(seconds);
        if (followingProjectile) followingProjectile = false;
        NextTurn();
        
    }

    public void UpdatePieceIDs(int team, int id)
    {
        var count = 0;

        if (team == 1)
        {
            for (var i = 0; i < pieces1.Count; i++)
            {
                if (pieces1[i].GetComponent<Piece>().pieceID > id)
                {
                    if (t1Count != 1 && count == 0)
                    {
                        t1Count -= 1;
                        count = 1;
                    }
                    pieces1[i].GetComponent<Piece>().pieceID -= 1;
                }
            }
        }

        if (team == 2)
        {
            
            for (var i = 0; i < pieces2.Count; i++)
            {
                if (pieces2[i].GetComponent<Piece>().pieceID > id)
                {
                    if (t2Count != 1 && count == 0)
                    {
                        t2Count -= 1;
                        count = 1;
                    }
                    pieces2[i].GetComponent<Piece>().pieceID -= 1;
                }
            }
        }
    }

    public void FollowMe(Transform obj)
    {
        followingProjectile = true;
        projectileToFollow = obj;
    }

    void ToggleControls()
    {
        if (controlsText.active == true) controlsText.SetActive(false);
        else controlsText.SetActive(true);
    }

    void ToggleOverhead()
    {
        if (overheadCam.Priority < 10)
        {
            isOverhead = true;
            mainCam.orthographic = true;
            overheadCam.Priority += 10;
        }
        else
        {
            isOverhead = false;
            mainCam.orthographic = false;
            overheadCam.Priority -= 10;
        }
    }

    void TogglePause()
    {
        if (isPaused == false)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
