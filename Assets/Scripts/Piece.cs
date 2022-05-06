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
    private Vector3 startingPos;
    private bool set; //checks if the startingPos is set for this turn
    private bool hit; //has been hit this turn

    public GameObject[] weapons = new GameObject[6]; //weapons / powerups this piece has in its inventory
    public int currentWeapon;

    public bool hasFired; //checks if this piece has used an attack this turn
    private bool isJacked; //checks if this piece is jacked af
    public bool hasJetpack; //checks if this piece has a jetpack

    private ThirdPersonMovement moveScript;
    private ThirdPersonAiming aimScript;
    public CharacterController controller;
    public CapsuleCollider collider;
    public Rigidbody rigidbody;

    public GameObject gameManager;
    public GameManager gmScript;
    public GameObject[] body;
    public GameObject back;
    private UIManager uiScript;

    public Material[] newMaterial;

    public Transform pieceTransform; //this piece's transform 

    private AudioManager audioScript;
    public AudioClip deathSound;
    public AudioClip cycleSound;

    // Start is called before the first frame update
    void Start()
    {
        set = false;
        hit = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        gameManager = GameObject.Find("GameManager");
        gmScript = gameManager.GetComponent<GameManager>();
        uiScript = gameManager.GetComponent<UIManager>();
        moveScript = GetComponent<ThirdPersonMovement>();
        aimScript = GetComponent<ThirdPersonAiming>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        health = healthMax;

        for (int i = 0; i < body.Length; i++)
        {
            body[i].GetComponent<Renderer>().material = newMaterial[team];
        }

        isJacked = false;
        hasJetpack = false;

        currentWeapon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (team == 1 && gmScript.turn == 1 && gmScript.t1Count == pieceID)
        {
            myTurn = true;
        }
        else if (team == 2 && gmScript.turn == 2 && gmScript.t2Count == pieceID)
        {
            myTurn = true;
        }
        else myTurn = false;

        if (myTurn == true && gmScript.followingProjectile == false)
        {
            gmScript.pieceToFollow = pieceTransform; //tell the camera to follow this piece
            //while it's myTurn : control me and ignore natural physics
            gameObject.layer = 7;
            if (hasFired == false) EnableScripts();

            if (set == false)
            {
                startingPos = new Vector3(pieceTransform.position.x, 0f, pieceTransform.position.z);
                set = true;
                hasFired = false;
                aimScript.power = 0;
                moveScript.velocity.y = 0;
            }

            if (aimScript.isCharging)
            {
                gmScript.chargeMeter.SetActive(true);
                UpdateChargeMeter();
            }
            else gmScript.chargeMeter.SetActive(false);

            CheckDistance(); //calculate distance moved this turn
            UpdateMovementMeter();
            UpdateActionWheel();
            if (distanceMoved >= movementMax) moveScript.OnDisable();

            if (gmScript.isOverhead == false)
            {
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
        }
        else 
        {
            //otherwise : I can't be controlled and physics will affect me
            gameObject.layer = 6;
            if (hit == false) DisableScripts();
            set = false;
        }

        UpdateWeapon();
        UpdateHealth();

    }

    public void EnableScripts()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        hit = false;
        moveScript.enabled = true;
        aimScript.enabled = true;
        controller.enabled = true;
        collider.enabled = false;
        rigidbody.isKinematic = true;
    }

    public void DisableScripts()
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

    public void CycleWeapons(int dir)
    {
        if (aimScript.isCharging == false)
        {
            currentWeapon += dir;
            if (currentWeapon >= weapons.Length) currentWeapon = 0;
            if (currentWeapon < 0) currentWeapon = weapons.Length - 1;
            audioScript.PlaySoundPyramind(cycleSound, gameObject);
        }
    }

    void UpdateWeapon()
    {
        for (var i = 0; i < weapons.Length; i++)
        {
            if (currentWeapon == i)
            {
                weapons[i].SetActive(true);
            }
            else weapons[i].SetActive(false);
        }
    }

    void UpdateChargeMeter()
    {
        var scaleY = (aimScript.power / aimScript.powerMax) * 2.73f;

        gmScript.chargeMeterJuice.transform.localScale = new Vector3(scaleY, 1.8f, 1);
    }

    void UpdateHealth()
    {
        var scaleX = (health / healthMax) * 0.09f;
        healthBarJuice.transform.localScale = new Vector3(scaleX, 0.08f, 1);

        if (health > healthMax) health = healthMax;

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

    void UpdateActionWheel()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            uiScript._weapons[i] = weapons[i];
        }
        uiScript.currentSlot = currentWeapon;
    }

    public void ImHit()
    {
        hit = true;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void JackedUp(float percentage)
    {
        if (isJacked == false)
        {
            aimScript.meleeStrength *= percentage;
            aimScript.meleeDamage *= percentage;
            aimScript.powerMax *= percentage;
            isJacked = true;
        }
    }

    public void JetpackGet()
    {
        hasJetpack = true;
        back.SetActive(true);
    }

    public void Death()
    {
        var piece = this.gameObject;
        Debug.Log(piece + " " + "is Ded");
        if (team == 1) gmScript.pieces1.Remove(piece);
        if (team == 2) gmScript.pieces2.Remove(piece);

        gmScript.UpdatePieceIDs(team, pieceID);

        audioScript.PlaySoundPyramind(deathSound, gameObject);

        Destroy(healthBar);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 9) //killbox
        {
            Death();
        }

        if (collider.gameObject.layer == 11) //powerup
        {
            var powerup = collider.gameObject.GetComponent<Powerup>();
            powerup.Pickup(this.gameObject);
        }
    }
}
