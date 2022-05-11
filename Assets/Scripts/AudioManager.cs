using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float GlobalVolume;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVolume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = GlobalVolume;
    }

    public void PlaySoundPyramind(AudioClip clip, GameObject objectToPlayOn)
    {
        AudioSource.PlayClipAtPoint(clip, objectToPlayOn.transform.position);
    }
}
