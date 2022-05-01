using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimator : MonoBehaviour
{
    public GameObject piece;
    public ThirdPersonAiming aimScript;
    public Piece pieceScript;

    public bool _isAiming;

    Animator animator;
    int isAimingParam = Animator.StringToHash("isAiming");

    // Start is called before the first frame update
    void Start()
    {
        aimScript = piece.GetComponent<ThirdPersonAiming>();
        pieceScript = piece.GetComponent<Piece>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _isAiming = aimScript.isAiming;

        if (_isAiming)
        {
            animator.SetBool(isAimingParam, true);

        }
        else
        {
            animator.SetBool(isAimingParam, false);

        }

        if (pieceScript.myTurn == false)
        {
            animator.SetTrigger("Reset");
            animator.ResetTrigger("Attack");
        }
        else animator.ResetTrigger("Reset");
    }

    public void Swing()
    {
        animator.SetTrigger("Attack");
    }
}
