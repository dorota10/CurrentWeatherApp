using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{


    public static AudioClip rainsound;
    static AudioSource audio_source;

    // Start is called before the first frame update
    private void Start()
    {
        rainsound = Resources.Load<AudioClip>("rain_medium");

        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public static void RainSounds()
    {
        audio_source.PlayOneShot(rainsound);
    }
}
