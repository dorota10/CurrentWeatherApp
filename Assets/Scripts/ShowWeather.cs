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
    private bool foggy;
    private bool snowy;
    private bool rainy;
    public GameObject cloudyObject;
    public GameObject sunnyObject;
    public GameObject foggyObject;
    public GameObject snowyObject;
    public GameObject rainyObject;
    private Vector3 startPoint = new Vector3(0f, 3.5f, 0f); // Punkt docelowy przy aktywacji
    private Vector3 endPoint = new Vector3(2.5f, 6.5f, 0f); // Punkt docelowy przy dezaktywacji
    public float arcHeight = 0.1f;
    public float duration = 2f;
    public UnityEngine.Rendering.Universal.Light2D night;
    private System.DateTime currentTime;
    private bool isNight;
    public GameObject noc_niebo_Object;
    public GameObject dzien_niebo_Object;
    public GameObject snow_grass_Object;
    public GameObject tree_Object;
    public GameObject krzaczek_Object;
    public GameObject snow_tree_Object;
    public GameObject snow_krzaczek_Object;
    public GameObject chmurki_Object;



    void Awake()
    {
        Instance = this;
    }

    public void DisplayWeather(WeatherData weatherData)
    {
        currentWeather = weatherData.weather[0].main;
        if (currentWeather == "Clouds") //|| currentWeather == "Snow"
        {
            SpawnCloudy();
        }
        else if (currentWeather == "Clear")
        {
            SpawnSunny();
        }
        else if (currentWeather == "Fog")
        {
            SpawnFoggy();

        }
        else if (currentWeather == "Snow")
        {
            SpawnSnowy();

        }
        else if (currentWeather == "Rain")
        {
            SpawnRainy();
        }
        else
        {
            None();
        }
        StartCoroutine(DayNight(weatherData)); //w³aczanie nocy
    }

    public void DisplayDemo(string currentWeather)
    {
        if (currentWeather == "Clouds")//|| currentWeather == "Snow"
        {
            SpawnCloudy();
        }
        else if (currentWeather == "Clear")
        {
            SpawnSunny();
        }
        else if (currentWeather == "Fog")
        {
            SpawnFoggy();
        }
        else if (currentWeather == "Snow")
        {
            SpawnSnowy();

        }
        else if (currentWeather == "Rain")
        {
            SpawnRainy();
        }
        else
        {
            None();
        }

        noc_niebo_Object.SetActive(false);
        dzien_niebo_Object.SetActive(true); //demo jest tylko w ci¹gu dnia
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
        else if (foggy)
        {
            StartCoroutine(DisableFoggy());
        }
        else if (snowy)
        {
            StartCoroutine(DisableSnowy());
        }
        else if (rainy)
        {
            StartCoroutine(DisableRainy());
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
        else if (foggy)
        {
            StartCoroutine(DisableFoggy());
        }
        else if (snowy)
        {
            StartCoroutine(DisableSnowy());
        }
        else if (foggy) // wy³¹czam mg³ê
        {
            StartCoroutine(DisableFoggy());
        }
        else if (rainy)
        {
            StartCoroutine(DisableRainy());
        }
    }


    public void SpawnFoggy()
    {
        foggy = true;
        foggyObject.SetActive(true);
        FindObjectOfType<FogGen>().StartGeneratingFog();
        if (sunny)
        {
            StartCoroutine(DisableSunny());
        }
        if (cloudy) //wy³¹czam chmury
        {
            StartCoroutine(DisableCloudy());
        }
    }
    void SpawnSnowy()
    {
        snowy = true;
        snow_grass_Object.SetActive(true);
        krzaczek_Object.SetActive(false);
        tree_Object.SetActive(false);
        snow_krzaczek_Object.SetActive(true);
        snow_tree_Object.SetActive(true);
        snowyObject.SetActive(true);
        chmurki_Object.SetActive(true);
        FindObjectOfType<SnowGen>().StartGeneratingSnowflakes();
        if (sunny)
        {
            StartCoroutine(DisableSunny());
        }
        else if (foggy) //wy³¹czam mg³ê
        {
            StartCoroutine(DisableFoggy());
        }
        else if (cloudy)
        {
            StartCoroutine(DisableCloudy());
        }
    }
    void SpawnRainy()
    {
        rainy = true;
        
        FindObjectOfType<RainGen>().StartGeneratingRaindrops();
        if (sunny)
        {
            StartCoroutine(DisableSunny());
        }
        else if (foggy) //wy³¹czam mg³ê
        {
            StartCoroutine(DisableFoggy());
        }
        else if (cloudy)
        {
            StartCoroutine(DisableCloudy());
        }
        else if (snowy)
        {
            StartCoroutine(DisableSnowy());
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
        else if (foggy)
        {
            StartCoroutine(DisableFoggy());
        }
        else if (snowy)
        {
            StartCoroutine(DisableSnowy());
        }
        else if (rainy)
        {
            StartCoroutine(DisableRainy());
        }

    }

    IEnumerator DisableCloudy()
    {
        cloudy = false;
        FindObjectOfType<CloudGen>().StopGeneratingClouds(); //Najpierw trzeba zatrzymaæ generowanie chmur
        GameObject cloudsParent = GameObject.Find("Clouds");

        if (cloudsParent != null)
        {
            foreach (Transform cloud in cloudsParent.transform)
            {
                Destroy(cloud.gameObject);
            }
        }
        cloudyObject.SetActive(false); //Potem dopiero mo¿na je odaktywniæ
        yield return new WaitForSeconds(0);

    }
    IEnumerator DisableFoggy()
    {
        foggy = false;
        FindObjectOfType<FogGen>().StopGeneratingFog();
        GameObject fogcloudsParent = GameObject.Find("FogClouds"); //Tu by³a z³a nazwa Fog zamiast FogClouds, dlatego nie chwyta³o :)

        if (fogcloudsParent != null)
        {
            foreach (Transform fog in fogcloudsParent.transform)
            {
                Destroy(fog.gameObject);
            }
        }
        foggyObject.SetActive(false); //Potem dopiero mo¿na je odaktywniæ
        yield return new WaitForSeconds(0);
    }

    IEnumerator DisableSnowy()
    {
        snowy = false;
        snow_krzaczek_Object.SetActive(false);
        snow_tree_Object.SetActive(false);
        krzaczek_Object.SetActive(true);
        tree_Object.SetActive(true);
        chmurki_Object.SetActive(false);
        snow_grass_Object.SetActive(false); //usuniêcie snowgrass za ka¿dym razem gdy usuwamy œnieg
        FindObjectOfType<SnowGen>().StopGeneratingSnowflakes(); //Najpierw trzeba zatrzymaæ generowanie chmur
        GameObject snowflakesParent = GameObject.Find("Snowflakes");

        if (snowflakesParent != null)
        {
            foreach (Transform snow in snowflakesParent.transform)
            {
                Destroy(snow.gameObject);
            }
        }
        snowyObject.SetActive(false); //Potem dopiero mo¿na je odaktywniæ
        yield return new WaitForSeconds(0);

    }
    IEnumerator DisableRainy()
    {
        rainy = false;
        
        FindObjectOfType<RainGen>().StopGeneratingRaindrops(); //Najpierw trzeba zatrzymaæ generowanie chmur
        GameObject raindropsParent = GameObject.Find("RainDrops");

        if (raindropsParent != null)
        {
            foreach (Transform rain in raindropsParent.transform)
            {
                Destroy(rain.gameObject);
            }
        }
        rainyObject.SetActive(false); //Potem dopiero mo¿na je odaktywniæ
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
        isNight = (newTime > sunsetTimeOrg || newTime < sunriseTimeOrg);
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
