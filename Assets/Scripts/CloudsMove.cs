using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherInfo;

public class CloudsMove : MonoBehaviour
{
    private float endPosX;
    private float speed = 5;

    //void Awake()
    //{
    //    Instance = this;
    //}

    //IEnumerator getSpeed(WeatherData weatherData)
    //{
    //    WeatherInfo weatherInfo = FindObjectOfType<WeatherInfo>();
    //    double speed= weatherData.wind.speed;
    //}

    //private double speed = StartCaroutine(getSpeed(weatherData));

    public void StartFloating(float newspeed, float newendPosX)
    {
        speed = newspeed;
        endPosX = newendPosX;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        transform.Translate(new Vector3(movement, 0f, 0f));

        if(transform.position.x> endPosX)
        {
            Destroy(gameObject);
        }
    }

}
