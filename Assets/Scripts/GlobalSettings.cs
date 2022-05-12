using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static float sensitivity = 1f;

    public static float adsSensitivity = 0.1f;

    public static float globalVolume = 1f;

    public static int bulletBounces = 2;

    public static float explosionSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sensitivity <= 0) sensitivity = 0.1f;
        if (adsSensitivity <= 0) adsSensitivity = 0.01f;
        if (globalVolume < 0) globalVolume = 0;
        if (bulletBounces < 0) bulletBounces = 0;
        if (explosionSize < 0) explosionSize = 0.1f;
    }
}
