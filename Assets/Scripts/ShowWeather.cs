using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherInfo;


public class ShowWeather : MonoBehaviour
{
    //public SunRotate SunRotate_script;
    public static ShowWeather Instance;
    private string currentWeather;
    private float temp;
    private bool cloudy;
    private bool sunny;
    private bool foggy;
    private bool snowy;
    private bool rainy;
    private bool stormy;
    private bool isNight;
    public GameObject cloudyObject;
    public GameObject sunnyObject;
    public GameObject foggyObject;
    public GameObject snowyObject;
    public GameObject rainyObject;
    public GameObject stormyObject;
    private Vector3 startPoint = new Vector3(0f, 3.5f, 0f); // Punkt docelowy przy aktywacji s這鎍a
    private Vector3 endPoint = new Vector3(2.5f, 6.5f, 0f); // Punkt docelowy przy dezaktywacji s這鎍a
    private float arcHeight = 0.1f;
    private float duration = 2f;
    public UnityEngine.Rendering.Universal.Light2D lightness;
    private System.DateTime currentTime;
    public GameObject noc_niebo_Object;
    public GameObject dzien_niebo_Object;
    public GameObject ObiektyDeszcz;
    public GameObject ObiektySniegowe;
    public GameObject ObiektyZielone;
    public GameObject ObiektyBurza;
    public GameObject SnowClouds_Object;

    void Awake()
    {
        Instance = this;
    }

    public void DisplayWeather(WeatherData weatherData)
    {
        currentWeather = weatherData.weather[0].main;
        StartCoroutine(DayNight(weatherData));

        if (currentWeather == "Clouds")
        {
            SpawnCloudy();
        }
        else if (currentWeather == "Clear" && isNight==false)
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
        else if (currentWeather == "Thunderstorm")
        {
            SpawnStormy();
        }
        else
        {
            None();
        }

        temp=weatherData.main.temp;

        if (temp < 273.15)
        {
            ObiektySniegowe.SetActive(true);
            ObiektyZielone.SetActive(false);
        }
        else
        {
            ObiektySniegowe.SetActive(false);
            ObiektyZielone.SetActive(true);
        }
    }

    public void DisplayDemo(string currentWeather, bool sliderValue)
    {
        isNight= sliderValue;
        UstawNocDzien(isNight);

        if (currentWeather == "Clouds")//|| currentWeather == "Snow"
        {
            SpawnCloudy();
        }
        else if (currentWeather == "Clear" && isNight==false)
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
        else if (currentWeather == "Thunderstorm")
        {
            SpawnStormy();
        }
        else
        {
            None();
        }
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
        else if (stormy)
        {
            StartCoroutine(DisableStormy());
        }
    }


    void SpawnCloudy()
    {
        cloudy = true;
        if (sunny)
        {
            StartCoroutine(DisableSunny()); //s這鎍e zachodzi
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
        else if (stormy)
        {
            StartCoroutine(DisableStormy());
        }
        cloudyObject.SetActive(true);
        FindObjectOfType<CloudGen>().StartGeneratingClouds();
    }


    public void SpawnFoggy()
    {
        foggy = true;
        foggyObject.SetActive(true);
        FindObjectOfType<FogGen>().StartGeneratingFog();
        if (sunny)
        {
            sunnyObject.SetActive(false); //s這鎍e nie odchodzi, tylko znika
            //StartCoroutine(DisableSunny());
        }
        else if (cloudy)
        {
            StartCoroutine(DisableCloudy());
        }
        else if (snowy)
        {
            StartCoroutine(DisableSnowy());
        }
        else if (rainy)
        {
            StartCoroutine(DisableRainy());
        }
        else if (stormy)
        {
            StartCoroutine(DisableStormy());
        }
    }
    void SpawnSnowy()
    {
        snowy = true;
        ObiektyZielone.SetActive(false);
        ObiektySniegowe.SetActive(true);
        SnowClouds_Object.SetActive(true);
        FindObjectOfType<SnowGen>().StartGeneratingSnowflakes();
        if (sunny)
        {
            sunnyObject.SetActive(false);
            //StartCoroutine(DisableSunny());
        }
        else if (foggy)
        {
            StartCoroutine(DisableFoggy());
        }
        else if (cloudy)
        {
            StartCoroutine(DisableCloudy());
        }
        else if (rainy)
        {
            StartCoroutine(DisableRainy());
        }
        else if (stormy)
        {
            StartCoroutine(DisableStormy());
        }
    }
    void SpawnRainy()
    {
        rainy = true;
        if (sunny)
        {
            sunnyObject.SetActive(false);
            //StartCoroutine(DisableSunny());
        }
        else if (foggy)
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
        else if (stormy)
        {
            StartCoroutine(DisableStormy());
        }
        if (isNight == false)
        {
            lightness.intensity = 1f;
        }
        ObiektyDeszcz.SetActive(true);
        FindObjectOfType<RainGen>().StartGeneratingRaindrops();
    }
    void SpawnStormy()
    {
        stormy = true;
        if (sunny)
        {
            sunnyObject.SetActive(false);
            //StartCoroutine(DisableSunny());
        }
        else if (foggy)
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
        else if (rainy)
        {
            StartCoroutine(DisableRainy());
        }
        if (isNight == false)
        {
            lightness.intensity = 1f;
        }
        ObiektyBurza.SetActive(true);
        FindObjectOfType<StormGen>().StartGeneratingThunders();
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
        else if (stormy)
        {
            StartCoroutine(DisableStormy());
        }
    }

    IEnumerator DisableCloudy()
    {
        cloudy = false;
        FindObjectOfType<CloudGen>().StopGeneratingClouds();
        GameObject cloudsParent = GameObject.Find("Clouds");

        if (cloudsParent != null)
        {
            foreach (Transform cloud in cloudsParent.transform)
            {
                Destroy(cloud.gameObject);
            }
        }
        cloudyObject.SetActive(false);
        yield return new WaitForSeconds(0);

    }
    IEnumerator DisableFoggy()
    {
        foggy = false;
        FindObjectOfType<FogGen>().StopGeneratingFog();
        GameObject fogcloudsParent = GameObject.Find("FogClouds");

        if (fogcloudsParent != null)
        {
            foreach (Transform fog in fogcloudsParent.transform)
            {
                Destroy(fog.gameObject);
            }
        }
        foggyObject.SetActive(false);
        yield return new WaitForSeconds(0);
    }

    IEnumerator DisableSnowy()
    {
        snowy = false;
        FindObjectOfType<SnowGen>().StopGeneratingSnowflakes();
        GameObject snowflakesParent = GameObject.Find("Snowflakes");

        if (snowflakesParent != null)
        {
            foreach (Transform snow in snowflakesParent.transform)
            {
                Destroy(snow.gameObject);
            }
        }
        ObiektySniegowe.SetActive(false);
        ObiektyZielone.SetActive(true);
        SnowClouds_Object.SetActive(false);
        yield return new WaitForSeconds(0);

    }
    IEnumerator DisableRainy()
    {
        rainy = false;

        FindObjectOfType<RainGen>().StopGeneratingRaindrops();
        GameObject raindropsParent = GameObject.Find("RainDrops");

        if (raindropsParent != null)
        {
            foreach (Transform rain in raindropsParent.transform)
            {
                Destroy(rain.gameObject);
            }
        }
        ObiektyDeszcz.SetActive(false);
        yield return new WaitForSeconds(0);

    }
    IEnumerator DisableStormy()
    {
        stormy = false;

        FindObjectOfType<StormGen>().StopGeneratingThunders();
        GameObject thundersParent = GameObject.Find("Thunders");

        if (thundersParent != null)
        {
            foreach (Transform thunder in thundersParent.transform)
            {
                Destroy(thunder.gameObject);
            }
        }
        ObiektyBurza.SetActive(false);
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

        isNight = (newTime > sunsetTimeOrg || newTime < sunriseTimeOrg);
        UstawNocDzien(isNight);

        yield return new WaitForSeconds(0);

    }

    void UstawNocDzien(bool isNight)
    {
        if (isNight)
        {
            noc_niebo_Object.SetActive(true);
            dzien_niebo_Object.SetActive(false);
            lightness.intensity = 0.3f;
            if (sunny)
            {
                sunnyObject.SetActive(false);
            }
        }
        else
        {
            noc_niebo_Object.SetActive(false);
            dzien_niebo_Object.SetActive(true);
            lightness.intensity = 3f;
        }
        
    }
}
