using System.Collections;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static AudioClip rainsound;
    public static AudioClip stormsound;
    static AudioSource audio_source;

    // Start is called before the first frame update
    private void Start()
    {
        rainsound = Resources.Load<AudioClip>("rain_medium");
        stormsound = Resources.Load<AudioClip>("Thunder");

        audio_source = GetComponent<AudioSource>();
    }

    // Odtwarzaj dŸwiêk deszczu
    public static void RainSounds()
    {
        audio_source.PlayOneShot(rainsound);
    }

    // Odtwarzaj dŸwiêk burzy
    public static void StormSounds()
    {
        audio_source.PlayOneShot(stormsound);
    }
}
