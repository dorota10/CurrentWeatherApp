using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoVersion : MonoBehaviour
{
    public TMP_Dropdown weatherDropdown;
    private ShowWeather showWeatherScript;

    void Start()
    {
        showWeatherScript = GetComponent<ShowWeather>();
    }

    public void selection()
    {
        // Pobieramy wybran¹ wartoœæ z Dropdowna
        string selectedWeather = weatherDropdown.options[weatherDropdown.value].text;
        showWeatherScript.DisplayDemo(selectedWeather);
    }

}
