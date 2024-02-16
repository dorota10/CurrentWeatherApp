using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherInfo;


public class DrizzleMove : MonoBehaviour
{
    private float endPosY;
    private float speed = 15;

    public void StartFloating(float newspeed, float newendPosY)
    {
        speed = newspeed;
        endPosY = newendPosY;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        transform.Translate(Vector3.down * movement); // Zmiana przesuniêcia na w dó³

        if (transform.position.y < endPosY)
        {
            Destroy(gameObject);
        }


        Sounds.DrizzleSounds();
    }
}
