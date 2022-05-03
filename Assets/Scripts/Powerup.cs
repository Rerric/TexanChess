using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public string Type; //what type of powerup this is

    public float Health; //if its a medkit, how much hp this restores to the piece that picks it up

    private AudioManager audioScript;
    public AudioClip[] Sounds;

    // Start is called before the first frame update
    void Start()
    {
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Float();
    }

    void Float()
    {
        //move up and down slowly
    }

    public void Pickup(GameObject other)
    {
        
            var piece = other.GetComponent<Piece>();

            if (Type == "Medkit")
            {
                piece.health += Health;
                audioScript.PlaySoundPyramind(Sounds[0], other);
            }

            if (Type == "Jetpack")
            {

            }

            if (Type == "Whiskey")
            {

            }
           
            Destroy(gameObject);
        
    }
}
