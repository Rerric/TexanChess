using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int pieceID; //unique Piece ID in the game

    public int team; //what team this piece is on

    public bool isKing; //"Only one may be king.." -some guy

    public bool myTurn; //whether or not it's this piece's turn

    public float healthMax;
    public float health;
    public GameObject healthBar;
    public GameObject healthBarJuice;

    public float movementMax; //how far this piece can move in one turn
    public float distanceMoved; //how far this piece has moved this turn
    public Vector3 startingPos;
    private bool set; //checks if the startingPos is set for this turn

    public bool hasFired; //checks if this piece has used an attack this turn

    private ThirdPersonMovement moveScript;
    private ThirdPersonAiming aimScript;
    public CharacterController controller;
    public CapsuleCollider collider;
    public Rigidbody rigidbody;

    public GameObject gameManager;
    public GameManager gmScript;
    public GameObject body;

    public Material[] newMaterial;

    public Transform pieceTransform; //this piece's transform 

    // Start is called before the first frame update
    void Start()
    {
        set = false;

        gameManager = GameObject.Find("GameManager");
        gmScript = gameManager.GetComponent<GameManager>();
        moveScript = GetComponent<ThirdPersonMovement>();
        aimScript = GetComponent<ThirdPersonAiming>();

        health = healthMax;

        body.GetComponent<Renderer>().material = newMaterial[team];
    }

    // Update is called once per frame
    void Update()
    {
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
            EnableScripts();

            if (set == false)
            {
                startingPos = new Vector3(pieceTransform.position.x, 0f, pieceTransform.position.z);
                set = true;
                hasFired = false;
            }

            CheckDistance(); //calculate distance moved this turn
            UpdateMovementMeter();
            if (distanceMoved >= movementMax) moveScript.OnDisable();

            if (aimScript.isAiming)
            {
                gmScript.aimCanvas.enabled = true;
                gmScript.defaultCanvas.enabled = false;
            }
            else
            {
                gmScript.aimCanvas.enabled = false;
                gmScript.defaultCanvas.enabled = true;
            }
        }
        else 
        {
            //otherwise : I can't be controlled and physics will affect me
            gameObject.layer = 6;
            DisableScripts();

            set = false;
        }

        UpdateHealth();

    }

    void EnableScripts()
    {
        moveScript.enabled = true;
        aimScript.enabled = true;
        controller.enabled = true;
        collider.enabled = false;
        rigidbody.isKinematic = true;
    }

    void DisableScripts()
    {
        moveScript.enabled = false;
        aimScript.enabled = false;
        controller.enabled = false;
        collider.enabled = true;
        rigidbody.isKinematic = false;
    }

    void CheckDistance()
    {
        Vector3 currentPos = new Vector3(pieceTransform.position.x, 0f, pieceTransform.position.z);
        float distance = Vector3.Distance(startingPos, currentPos);
        distanceMoved = distance;
    }

    void UpdateHealth()
    {
        var scaleX = (health / healthMax) * 0.09f;
        healthBarJuice.transform.localScale = new Vector3(scaleX, 0.08f, 1);

        if (health <= 0)
        {
            Death();
        }
    }

    void UpdateMovementMeter()
    {
        var scaleX = (distanceMoved / movementMax) * 2.73f;

        gmScript.movementBarJuice.transform.localScale = new Vector3(scaleX, 1.8f, 1);
    }

    public void Death()
    {
        var id = pieceID;
        Debug.Log("Ded" + " " + id);
        gmScript.UpdatePieceIDs(id);
        
        Destroy(healthBar);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 9) //killbox
        {
            Death();
        }
    }
}
