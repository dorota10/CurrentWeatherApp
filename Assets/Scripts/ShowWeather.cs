using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherInfo;


public class ShowWeather : MonoBehaviour
{
    //public SunRotate SunRotate_script;
    public static ShowWeather Instance;
    public string currentWeather;
    private bool cloudy;
    private bool sunny;
    public GameObject cloudyObject;
    public GameObject sunnyObject;
    private Vector3 startPoint = new Vector3(0f, 3.5f, 0f); // Punkt docelowy przy aktywacji
    private Vector3 endPoint = new Vector3(2.5f, 6.5f, 0f); // Punkt docelowy przy dezaktywacji
    public float arcHeight = 0.1f;
    public float duration = 2f;
    public UnityEngine.Rendering.Universal.Light2D night;
    private System.DateTime currentTime;
    private bool isNight;
    public GameObject noc_niebo_Object;
    public GameObject dzien_niebo_Object;

    void Awake()
    {
        Instance = this;
    }

    public void DisplayWeather(WeatherData weatherData)
    {
        currentWeather = weatherData.weather[0].main;
        if (currentWeather == "Clouds" || currentWeather == "Drizzle" || currentWeather == "Rain" || currentWeather == "Snow")
        {
            SpawnCloudy();
        }
        else if (currentWeather == "Clear")
        {
            SpawnSunny();
        }
        else
        {
            None();
        }
        StartCoroutine(DayNight(weatherData)); //w�aczanie nocy
    }

    public void DisplayDemo(string currentWeather)
    {
        if (currentWeather == "Clouds" || currentWeather == "Drizzle" || currentWeather == "Rain" || currentWeather == "Snow")
        {
            SpawnCloudy();
        }
        else if (currentWeather == "Clear")
        {
            SpawnSunny();
        }
        else
        {
            None();
        }

        noc_niebo_Object.SetActive(false);
        dzien_niebo_Object.SetActive(true); //demo jest tylko w ci�gu dnia
        isNight = false;
        night.enabled = false;
    }


    void SpawnSunny()
    {
        sunny = true;
        sunnyObject.SetActive(true);
        StartCoroutine(MoveObjectAlongArcCoroutine(endPoint, startPoint));

        if (cloudy)
        {
            StartCoroutine(DisableCloudy());
        }
    }


    void SpawnCloudy()
    {
        cloudy = true;
        cloudyObject.SetActive(true);
        FindObjectOfType<CloudGen>().StartGeneratingClouds();
        if (sunny)
        {
            StartCoroutine(DisableSunny());
        }
    }

    void None()
    {
        if (cloudy)
        {
            StartCoroutine(DisableCloudy());
        }
        else if (sunny)
        {
            StartCoroutine(DisableSunny());
        }

    }

    IEnumerator DisableCloudy()
    {
        cloudy = false;
        FindObjectOfType<CloudGen>().StopGeneratingClouds(); //Najpierw trzeba zatrzyma� generowanie chmur
        GameObject cloudsParent = GameObject.Find("Clouds");

        if (cloudsParent != null)
        {
            foreach (Transform cloud in cloudsParent.transform)
            {
                Destroy(cloud.gameObject);
            }
        }
        cloudyObject.SetActive(false); //Potem dopiero mo�na je odaktywni�
        yield return new WaitForSeconds(0);
 
    }



    IEnumerator MoveObjectAlongArcCoroutine(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            float x = Mathf.Lerp(start[0], end[0], t);
            float y = Mathf.Lerp(start[1], end[1], t) + arcHeight * Mathf.Sin(t * Mathf.PI);

            sunnyObject.transform.position = new Vector3(x, y, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DisableSunny()
    {
        sunny = false;
        StartCoroutine(MoveObjectAlongArcCoroutine(startPoint, endPoint));
        yield return new WaitForSeconds(2);
        sunnyObject.SetActive(false);
    }

    IEnumerator DayNight(WeatherData weatherData)
    {
        WeatherInfo weatherInfo = FindObjectOfType<WeatherInfo>();
        double time = weatherData.timezone;
        double h = time / 3600 - 1;

        System.DateTime currentTime = System.DateTime.Now;
        System.DateTime newTime = currentTime.AddHours(h);

        double sunriseTime = weatherData.sys.sunrise;
        double sunsetTime = weatherData.sys.sunset;

        System.DateTime sunriseTimeUTC = WeatherInfo.UnixTimeStampToDateTime(sunriseTime);
        System.DateTime sunsetTimeUTC = WeatherInfo.UnixTimeStampToDateTime(sunsetTime);

        System.DateTime sunriseTimeOrg = sunriseTimeUTC.AddHours(h);
        System.DateTime sunsetTimeOrg = sunsetTimeUTC.AddHours(h);

        //print(night.enabled.ToString());
        bool isNight = (newTime > sunsetTimeOrg || newTime < sunriseTimeOrg);
        if (isNight)
        {
            night.enabled = true;
            noc_niebo_Object.SetActive(true);
            dzien_niebo_Object.SetActive(false);
            if (sunny)
            {
                sunnyObject.SetActive(false);
            }
        }
        else
        {
            night.enabled = false;
            noc_niebo_Object.SetActive(false);
            dzien_niebo_Object.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

    }
}