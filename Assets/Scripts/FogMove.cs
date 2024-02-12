using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherInfo;

public class FogMove : MonoBehaviour
{
    private float endPosX;
    private float speed = 2;


    public void StartFloating(float newspeed, float newendPosX)
    {
        speed = newspeed;
        endPosX = newendPosX;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        transform.Translate(new Vector3(movement, 0f, 0f));

        if (transform.position.x > endPosX)
        {
            Destroy(gameObject);
        }
    }

}
   