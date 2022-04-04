using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;

    public Transform _follow;
    public Transform _lookat;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineFreeLook = gameObject.GetComponent <CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        cinemachineFreeLook.Follow = _follow;
        cinemachineFreeLook.LookAt = _lookat;
    }
}
