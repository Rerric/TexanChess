using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject body;

    public float damageMax; //maximum damage
    public float damageMin; //minimum damage
    public float radius; //explosion radius
    public float forceMax; //maximum knockback
    public float forceUp; //how hard the explosion knocks things up

    public float speed;
    
    public float gravity = -9.81f; //note this only affects the piece while the controller is enabled

    public float lifetime; //how long this projectile can exist before expiring

    private GameManager gmScript;

    private AudioManager audioScript;
    public AudioSource source;
    public AudioClip[] Sounds;

    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        rb.AddTorque(transform.right * 1.5f, ForceMode.Impulse);

        StartCoroutine(Fuse(lifetime - 2f));

        if (gmScript.gameEnded == false) gmScript.FollowMe(this.transform);

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Explode()
    {
        rb.isKinematic = true;
        source.enabled = false;
        body.SetActive(false);
        audioScript.PlaySoundPyramind(Sounds[1], gameObject);
        var explosionPoint = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        var explosionParticle = Instantiate(particle, explosionPoint , Quaternion.identity);
        explosionParticle.transform.localScale *= 1.5f;

        Collider[] others = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in others)
        {
            if (hit.gameObject.layer == 6) //if it's a non-player piece
            {
                var damage = 0f;
                var distance = Vector3.Distance(transform.position, hit.gameObject.transform.position);
                if (distance <= radius / 2f) damage = damageMax;
                if (distance > radius / 2f) damage = damageMin;
                var piece = hit.gameObject.GetComponent<Piece>();
                piece.health -= damage;
                piece.ImHit();
            }

            if (hit.gameObject.CompareTag("Prop")) //if its a prop
            {
                var prop = hit.gameObject.GetComponent<Prop>();
                prop.ImHit();
            }

            Rigidbody _rb = hit.GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.AddExplosionForce(forceMax, transform.position, radius, forceUp);
            }
        }
    }

    public IEnumerator Fuse(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Explode();
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
