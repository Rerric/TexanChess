using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet : MonoBehaviour
{
    public GameObject piece;
    private ThirdPersonMovement moveScript;

    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = piece.GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveScript.isFlying)
        {
            particle.SetActive(true);
        }
        else particle.SetActive(false);
    }
}
