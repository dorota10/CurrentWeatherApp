using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherInfo;


public class RainMove : MonoBehaviour
{
    private float endPosY;
    private float speed = 8;

    public void StartFloating(float newspeed, float newendPosY)
    {
        speed = newspeed;
        endPosY = newendPosY;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        transform.Translate(Vector3.down * movement); // Zmiana przesuni�cia na w d�

        if (transform.position.y < endPosY)
        {
            Destroy(gameObject);
        }

        Sounds.RainSounds();//dzwiek deszczu
    }
}