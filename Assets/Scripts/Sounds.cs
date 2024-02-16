using System.Collections;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static AudioClip rainsound;
    public static AudioClip stormsound;
    public static AudioClip drizzlesound;
    static AudioSource audio_source;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        audio_source = GetComponent<AudioSource>();
        rainsound = Resources.Load<AudioClip>("rain_medium");
        stormsound = Resources.Load<AudioClip>("Thunder");
        drizzlesound = Resources.Load<AudioClip>("rain_light");
    }

    // Odtwarzaj d�wi�k deszczu
    public static void RainSounds()
    {
        audio_source.PlayOneShot(rainsound);
    }

    // Odtwarzaj d�wi�k burzy
    public static void StormSounds()
    {
        audio_source.PlayOneShot(stormsound);
    }

    // Odtwarzaj d�wi�k m�awki
    public static void DrizzleSounds()
    {
        audio_source.PlayOneShot(drizzlesound);
    }

    // Wy��cz d�wi�k przy dezaktywacji obiektu
    private void OnDisable()
    {
        audio_source.Stop();
    }
}