using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public string Type; //what type of powerup this is

    public float Health; //if its a medkit, how much hp this restores to the piece that picks it up
    public float jackedUpPercentage; //if its whiskey, how much it will empower the stats of the piece that picks it up

    public float rotationSpeed;

    private GameManager gmScript;
    private AudioManager audioScript;
    public AudioClip[] Sounds;

    public GameObject particle;

    private float direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        InvokeRepeating("Float", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gmScript.isPaused == false)
        {
            transform.Translate(0f, direction * Time.deltaTime * 0.5f, 0f);
            transform.Rotate(0f, rotationSpeed, 0f);
        }
    }

    void Float()
    {
        if (direction == 1)
        {
            direction = -1;
        }
        else direction = 1;
    }

    public void Pickup(GameObject other)
    {
        
            var piece = other.GetComponent<Piece>();

            if (Type == "Medkit")
            {
                piece.health += Health;
                var _particle = Instantiate(particle, other.transform.position, Quaternion.identity);
                _particle.transform.localScale *= 0.5f;
            }

            if (Type == "Jetpack")
            {
                piece.JetpackGet();
            }

            if (Type == "Whiskey")
            {
                piece.JackedUp(jackedUpPercentage);
            }

        audioScript.PlaySoundPyramind(Sounds[0], other);
        Destroy(gameObject);
        
    }
}
