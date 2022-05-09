using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    PlayerControls controls;

    public CharacterController controller;
    public GameObject mainCam;
    public Transform cam;
    private Piece pieceScript;

    Vector2 move;

    public float speed = 6f;
    public Vector3 velocity;
    public float gravity = -9.81f; //note this only affects the piece while the controller is enabled
    public float jumpHeight;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public LayerMask entityMask;
    public bool isGrounded;

    public bool isFlying;
    public float flySpeed; //how fast this moves when activating the jetpack

    public float turnSpeed = 0.1f;
    private float turnVelocity;

    private AudioManager audioScript;
    public AudioClip[] moveSounds;

    public GameObject moveParticle;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Jetpack.performed += ctx => Fly(true);
        controls.Gameplay.Jetpack.canceled += ctx => Fly(false);


        mainCam = GameObject.Find("Main Camera");
        cam = mainCam.GetComponent<Transform>();
        pieceScript = GetComponent<Piece>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        isFlying = false;
    }

    public void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    public void OnDisable()
    {
        controls.Gameplay.Disable();
        moveParticle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPhysics();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = move.x;
        float vertical = move.y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //movement
        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        if (isFlying)
        {
            velocity.y += flySpeed * Time.deltaTime;
            //audioScript.PlaySoundPyramind(moveSounds[0], gameObject);
        }

        //gravity
        if (isFlying == false) velocity.y += gravity * Time.deltaTime;

        if (pieceScript.myTurn == true)
        {
            controller.Move(velocity * Time.deltaTime);

            if ((horizontal != 0f || vertical != 0f) && isGrounded)
            {
                moveParticle.SetActive(true);
            }
            else moveParticle.SetActive(false);
        }
    }

    void CheckPhysics()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, entityMask))
        {
            isGrounded = true;
        }
        else isGrounded = false;
        
    }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Fly(bool active)
    {
        if (pieceScript.hasJetpack)
        {
            isFlying = active;
        }
    }
}
