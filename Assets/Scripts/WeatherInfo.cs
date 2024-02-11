using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using UnityEngine.Networking;
using TMPro;

public class WeatherInfo : MonoBehaviour
{
    public string apiKey;
    public TMP_InputField city_field;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI weatherText;
    public TextMeshProUGUI windText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI sunriseText;
    public TextMeshProUGUI sunsetText;

    [System.Serializable]
    public class WeatherData
    {
        public MainData main;
        public Weather[] weather;
        public Wind wind;
        public int timezone;
        public SysData sys;
    }

    [System.Serializable]
    public class MainData
    {
        public float temp;
    }

    [System.Serializable]
    public class Coord
    {
        public double lon;
        public double lat;
    }

    [System.Serializable]
    public class Weather
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [System.Serializable]
    public class Wind
    {
        public double speed;
        public int deg;
    }

    [System.Serializable]
    public class SysData
    {
        public double sunrise;
        public double sunset;
    }

    public void API_data()
    {
        string city = city_field.text;
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

        StartCoroutine(FetchWeatherData(url));
    }

    public static System.DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }


    private WeatherData currentWeatherData;

    IEnumerator FetchWeatherData(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error + ". Check the city name.");
            }
            else
            {
                currentWeatherData = JsonUtility.FromJson<WeatherData>(webRequest.downloadHandler.text);

                double temperature = currentWeatherData.main.temp;
                double windSpeed = currentWeatherData.wind.speed;
                string weatherMain = currentWeatherData.weather[0].main;

                temperatureText.text = (temperature - 273.15).ToString("0.00") + " °C";
                windText.text = windSpeed.ToString("0.00") + " m/s";
                weatherText.text = weatherMain;

                double time = currentWeatherData.timezone;
                double h = time / 3600 - 1;

                System.DateTime currentTime = System.DateTime.Now;
                System.DateTime newTime = currentTime.AddHours(h);
                timeText.text = newTime.ToLongTimeString();

                double sunriseTime = currentWeatherData.sys.sunrise;
                double sunsetTime = currentWeatherData.sys.sunset;

                System.DateTime sunriseTimeUTC = UnixTimeStampToDateTime(sunriseTime);
                System.DateTime sunsetTimeUTC = UnixTimeStampToDateTime(sunsetTime);

                System.DateTime sunriseTimeOrg = sunriseTimeUTC.AddHours(h);
                System.DateTime sunsetTimeOrg = sunsetTimeUTC.AddHours(h);

                sunriseText.text = sunriseTimeOrg.ToLongTimeString();
                sunsetText.text = sunsetTimeOrg.ToLongTimeString();

                ShowWeather.Instance.DisplayWeather(currentWeatherData);

            }
        }
    }
}