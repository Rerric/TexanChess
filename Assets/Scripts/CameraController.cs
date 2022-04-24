using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public Transform cameraLookAt;

    private Cinemachine.CinemachineInputProvider inputAxisProvider;

    // Start is called before the first frame update
    void Start()
    {
        vcam = gameObject.GetComponent <CinemachineVirtualCamera>();

        inputAxisProvider = GetComponent<Cinemachine.CinemachineInputProvider>();
        xAxis.SetInputAxisProvider(0, inputAxisProvider);
        yAxis.SetInputAxisProvider(1, inputAxisProvider);

    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
    }
}
