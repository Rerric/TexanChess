using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimingCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public Transform _follow;
    public Transform _lookat;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        cinemachineVirtualCamera.Follow = _follow;
        cinemachineVirtualCamera.LookAt = _lookat;
    }
}
