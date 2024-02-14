using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoVersion : MonoBehaviour
{
    public TMP_Dropdown weatherDropdown;
    private ShowWeather showWeatherScript;
    public Slider daySlider;

    void Start()
    {
        showWeatherScript = GetComponent<ShowWeather>();
    }

    public void SelectedValues()
    {
        // Pobierz wybran¹ wartoœæ z Dropdowna
        string selectedWeather = weatherDropdown.options[weatherDropdown.value].text;

        // Zamieñ wartoœæ z Slidera na typ bool
        float sliderValue = daySlider.value;
        bool sliderBoolValue = (sliderValue != 0.0f);

        // Przekazuj obie wartoœci do metody DisplayDemo w skrypcie ShowWeather
        showWeatherScript.DisplayDemo(selectedWeather, sliderBoolValue);
    }

}
