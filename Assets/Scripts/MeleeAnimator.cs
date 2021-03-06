using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimator : MonoBehaviour
{
    public GameObject piece;
    public ThirdPersonAiming aimScript;
    public Piece pieceScript;
    private GameManager gmScript;

    public bool _isAiming;
    public bool isSwinging;

    Animator animator;
    int isAimingParam = Animator.StringToHash("isAiming");

    public Transform hitPoint;
    private List<Collider> _hitThisTurn = new List<Collider>();

    private AudioManager audioScript;
    public AudioClip[] Sounds;

    public GameObject particle; //particle for hitting things

    // Start is called before the first frame update
    void Start()
    {
        aimScript = piece.GetComponent<ThirdPersonAiming>();
        pieceScript = piece.GetComponent<Piece>();
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        else
        {
            animator.ResetTrigger("Reset");
        }
    }

    public void Swing()
    {
        isSwinging = true;
        gmScript.isSwinging = true;
        animator.SetTrigger("Attack");
    }

    void ResetSwing() //triggered at the end of swing animation
    {
        _hitThisTurn.Clear();
        isSwinging = false;
        
        gmScript.GoNext();
        pieceScript.DisableScripts();
    }

    void CheckHits()
    {
        Collider[] others = Physics.OverlapSphere(hitPoint.position, 2f);

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
                    var hitParticle = Instantiate(particle, hit.gameObject.transform.position, hit.gameObject.transform.rotation);
                    hitParticle.transform.localScale *= 0.5f;
                }

                if (hit.gameObject.CompareTag("Prop")) //hit a prop
                {
                    var prop = hit.gameObject.GetComponent<Prop>();
                    prop.ImHit();
                    audioScript.PlaySoundPyramind(Sounds[0], hit.gameObject);
                    var hitParticle = Instantiate(particle, hit.gameObject.transform.position, hit.gameObject.transform.rotation);
                    hitParticle.transform.localScale *= 0.5f;
                }

                Rigidbody _rb = hit.GetComponent<Rigidbody>();
                if (_rb != null)
                {
                    var radius = 2.5f;
                    var lift = 0.75f;
                    if (pieceScript.isKing)
                    {
                        radius += 0.5f;
                        lift += 1f;
                    }
                    _rb.AddExplosionForce(aimScript.meleeStrength, piece.transform.position, radius, lift);
                }
            }
            _hitThisTurn.Add(hit);
        }
    }
}
