using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ThirdPersonAiming : MonoBehaviour
{
    PlayerControls controls;

    public CharacterController controller;
    public GameObject mainCam;
    public Transform cam;

    private ThirdPersonMovement moveScript;
    private CameraController camScript;
    private Piece pieceScript;

    public float turnSpeed = 0.1f;
    private float turnVelocity;

    public bool isAiming;
    public bool isCharging; //checks if player is currently charging a throw
    public float power; //variable that represents throwing power
    public float powerMax; //maximum power the piece can throw
    public float meleeDamage; //how much damage this piece does with melee attacks
    public float meleeStrength; //how hard this piece can hit things with its melee weapon (if any)

    public GameObject arms;
    public MeleeAnimator meleeAnim;

    public Transform firePoint; //place where the bullet / projectile is created
    public GameObject[] projectiles;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.TakeAim.performed += ctx => TakeAim();
        controls.Gameplay.TakeAim.canceled += ctx => StopAim();
        controls.Gameplay.Fire.performed += ctx => Fire();
        controls.Gameplay.Fire.canceled += ctx => Release();
        controls.Gameplay.CycleRight.performed += ctx => pieceScript.CycleWeapons(1);
        controls.Gameplay.CycleLeft.performed += ctx => pieceScript.CycleWeapons(-1);

        mainCam = GameObject.Find("Main Camera");
        camScript = GameObject.Find("Third Person Camera").GetComponent<CameraController>();
        cam = mainCam.GetComponent<Transform>();

        moveScript = GetComponent<ThirdPersonMovement>();
        pieceScript = GetComponent<Piece>();
    }

    public void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    public void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            //rotate piece itself
            var camRotationY = cam.eulerAngles.y;
            float angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, camRotationY, ref turnVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angleY, 0f);

            //rotate arms for aiming up/down
            var camRotationX = cam.eulerAngles.x;
            arms.transform.rotation = Quaternion.Euler(camRotationX, angleY, 0f);
        }
        else arms.transform.rotation = transform.rotation;
    }

    void TakeAim()
    {
        //toggle Aiming State for this object
        isAiming = true;
        camScript.isAiming = true;
        moveScript.OnDisable();
        
    }

    void StopAim()
    {
        if (isCharging == false)
        {
            isAiming = false;
            camScript.isAiming = false;
            moveScript.OnEnable();
        }
    }

    void Fire()
    {
        var weapon = pieceScript.weapons[pieceScript.currentWeapon];

        if (weapon.name == "Revolver")
        {
            if (isAiming) StopAim();
            pieceScript.DisableScripts();

            if (pieceScript.hasFired == false)
            {
                var projectile = pieceScript.currentWeapon;
                Instantiate(projectiles[projectile], firePoint.position, firePoint.rotation);
                pieceScript.hasFired = true;
            }
        }

        if (weapon.name == "Dynamite")
        {
            isCharging = true;
            InvokeRepeating("Charge", 0.0f, 0.05f);
        }

        if (weapon.name == "Shovel")
        {
            if (isAiming) StopAim();
            pieceScript.DisableScripts(); 

            if (pieceScript.hasFired == false)
            {
                pieceScript.hasFired = true;
                meleeAnim.Swing();
            }
        }
    }

    void Charge()
    {
        power += 1;
        if (power > powerMax) power = powerMax;
    }

    void Release()
    {
        if (isCharging)
        {
            isCharging = false;
            if (isAiming) StopAim();
            pieceScript.DisableScripts();

            if (pieceScript.hasFired == false)
            {
                var projectile = pieceScript.currentWeapon;
                var dynamite = Instantiate(projectiles[projectile], firePoint.position, firePoint.rotation);
                dynamite.GetComponent<Dynamite>().speed = power;
                pieceScript.hasFired = true;
                CancelInvoke();
            }
        }
        
    }
}
