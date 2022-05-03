using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collider;

    private float speed;
    private float damage;

    //Velocity variables for determining how fast something is moving + corresponding damage values should it collide with a piece while moving at that speed
    public float[] velocities;
    public float[] damageValues;

    private List<GameObject> hitThisTurn = new List<GameObject>();

    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
    }

    void OnCollisionEnter(Collision other)
    {
        if (hitThisTurn.Contains(other.gameObject))
        {

        }
        else
        {
            if (other.gameObject.layer == 6) //if it's a non-player piece
            {
                CalculateDamage();
                var piece = other.gameObject.GetComponent<Piece>();
                piece.health -= damage;
                //audioScript.PlaySoundPyramind(Sounds[0], hit.gameObject);
                piece.ImHit();
                if (damage > 0)
                {
                    var hitParticle = Instantiate(particle, other.gameObject.transform.position, other.gameObject.transform.rotation);
                    hitParticle.transform.localScale *= 0.5f;
                }

                StartCoroutine(ResetHits());
            }
        }
        hitThisTurn.Add(other.gameObject);
    }

    void CalculateDamage()
    {
        for (var i = 0; i < 3; i++)
        {
            if (speed > velocities[i])
            {
                damage = damageValues[i];
            }
        }

        if (speed < velocities[0])
        {
            damage = 0;
        }
    }

    public IEnumerator ResetHits()
    {
        yield return new WaitForSeconds(2f);
        hitThisTurn.Clear();
    }
}
