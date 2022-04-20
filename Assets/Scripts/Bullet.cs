using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;

    public float damage;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.right * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6) //if it's a non-player piece
        {
            var hit = collision.gameObject.GetComponent<Piece>();
            hit.health -= damage;
            Destroy(gameObject, 0.2f);
        }
        else
        {
            Destroy(gameObject, 2.0f);
        }
    }
}
