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

    public bool isAiming;

    public GameObject arms;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.TakeAim.performed += ctx => TakeAim();
        controls.Gameplay.TakeAim.canceled += ctx => StopAim();

        mainCam = GameObject.Find("Main Camera");
        cam = mainCam.GetComponent<Transform>();

        aimingCam = GameObject.Find("Aiming Camera").GetComponent<CinemachineVirtualCamera>();

        moveScript = GetComponent<ThirdPersonMovement>();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            arms.transform.rotation = cam.rotation;
        }
    }

    void TakeAim()
    {
        //toggle Aiming State for this object
        isAiming = true;
        Debug.Log("Now Aiming!");
        //movescript
        aimingCam.Priority += 10;
    }

    void StopAim()
    {
        isAiming = false;
        Debug.Log("Stopped Aiming!");
        //movescript
        aimingCam.Priority -= 10;
    }
}
