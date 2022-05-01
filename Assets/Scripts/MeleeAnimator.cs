using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimator : MonoBehaviour
{
    public GameObject piece;
    public ThirdPersonAiming aimScript;
    public Piece pieceScript;

    public bool _isAiming;
    public bool isSwinging;

    Animator animator;
    int isAimingParam = Animator.StringToHash("isAiming");

    public Transform hitPoint;
    private List<Collider> _hitThisTurn = new List<Collider>();

    private AudioManager audioScript;
    public AudioClip[] Sounds;

    // Start is called before the first frame update
    void Start()
    {
        aimScript = piece.GetComponent<ThirdPersonAiming>();
        pieceScript = piece.GetComponent<Piece>();
        animator = GetComponent<Animator>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        isSwinging = false;
    }

    // Update is called once per frame
    void Update()
    {
        _isAiming = aimScript.isAiming;

        if (isSwinging)
        {
            CheckHits();
        }

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
        isSwinging = true;
        animator.SetTrigger("Attack");
    }

    void ResetSwing() //triggered at the end of swing animation
    {
        _hitThisTurn.Clear();
        isSwinging = false;
    }

    void CheckHits()
    {
        Collider[] others = Physics.OverlapSphere(hitPoint.position, 1f);

        foreach (Collider hit in others)
        {
            if (_hitThisTurn.Contains(hit))
            {

            }
            else
            {
                if (hit.gameObject.layer == 6) //if it's a non-player piece
                {
                    var damage = aimScript.meleeDamage;
                    var piece = hit.gameObject.GetComponent<Piece>();
                    piece.health -= damage;
                    audioScript.PlaySoundPyramind(Sounds[0], hit.gameObject);
                    piece.ImHit();
                }

                Rigidbody _rb = hit.GetComponent<Rigidbody>();
                if (_rb != null)
                {
                    _rb.AddExplosionForce(aimScript.meleeStrength, piece.transform.position, 3f, 1f);
                }
            }
            _hitThisTurn.Add(hit);
        }
    }
}
