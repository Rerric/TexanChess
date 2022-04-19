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

    public CinemachineVirtualCamera aimingCam;

    private ThirdPersonMovement moveScript;
    private Piece pieceScript;

    public float turnSpeed = 0.1f;
    private float turnVelocity;

    public bool isAiming;

    public GameObject arms;

    public Transform firePoint; //place where the bullet / projectile is created
    public GameObject[] projectiles;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.TakeAim.performed += ctx => TakeAim();
        controls.Gameplay.TakeAim.canceled += ctx => StopAim();
        controls.Gameplay.Fire.performed += ctx => Fire();

        mainCam = GameObject.Find("Main Camera");
        cam = mainCam.GetComponent<Transform>();

        aimingCam = GameObject.Find("Aiming Camera").GetComponent<CinemachineVirtualCamera>();

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
        Debug.Log("Now Aiming!");
        //movescript
        aimingCam.Priority += 10; //switch camera
    }

    void StopAim()
    {
        isAiming = false;
        Debug.Log("Stopped Aiming!");
        //movescript
        aimingCam.Priority -= 10;
    }

    void Fire()
    {
        if (pieceScript.hasFired == false)
        {
            Instantiate(projectiles[0], firePoint.position, firePoint.rotation);
            pieceScript.hasFired = true;
        }
    }
}
