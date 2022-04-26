using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private GameManager gmScript;
    public CinemachineVirtualCamera vcam;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public Transform cameraLookAt;
    public Transform target;

    public float speed;

    private Cinemachine.CinemachineInputProvider inputAxisProvider;

    public bool isAiming;

    Animator animator;
    int isAimingParam = Animator.StringToHash("isAiming");

    // Start is called before the first frame update
    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        vcam = gameObject.GetComponent <CinemachineVirtualCamera>();

        inputAxisProvider = GetComponent<Cinemachine.CinemachineInputProvider>();
        xAxis.SetInputAxisProvider(0, inputAxisProvider);
        yAxis.SetInputAxisProvider(1, inputAxisProvider);

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        if (target != null)
        {
            cameraLookAt.transform.position = Vector3.Lerp(cameraLookAt.transform.position, target.position, speed * Time.deltaTime);
        }

        if (isAiming)
        {
            animator.SetBool(isAimingParam, false);

        }
        else
        {
            animator.SetBool(isAimingParam, true);
            
        }
    }

}
