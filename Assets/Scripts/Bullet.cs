using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;

    public float damage;

    public float speed;
    Vector3 velocity;
    Vector3 movementDirection;
    public float gravity = -9.81f; //note this only affects the piece while the controller is enabled
    public float gravityOffset;

    public int bounces;
    public float lifetime; //how long this projectile can exist before expiring

    private GameManager gmScript;

    private AudioManager audioScript;
    public AudioClip[] hitSounds;

    // Start is called before the first frame update
    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        gmScript.FollowMe(this.transform);

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravityOffset);

        movementDirection = rb.velocity;

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection.normalized, Vector3.forward);

            transform.rotation = toRotation; //Rotate in direction of movement
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == 6) //if it's a non-player piece
        {
            audioScript.PlaySoundPyramind(hitSounds[1], gameObject);
            var hit = collision.gameObject.GetComponent<Piece>();
            hit.health -= damage;
            hit.ImHit();
        }
        else
        {
            audioScript.PlaySoundPyramind(hitSounds[0], gameObject);
        }

        if (bounces == 0) Destroy(gameObject);
        bounces -= 1;

        var _speed = movementDirection.magnitude;
        var direction = Vector3.Reflect(movementDirection.normalized, collision.contacts[0].normal);

        rb.velocity = direction * speed;

        gravityOffset += 0.2f;
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 9) //killbox
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        gmScript.GoNext();
    }
}
