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
        // Pobierz wybran� warto�� z Dropdowna
        string selectedWeather = weatherDropdown.options[weatherDropdown.value].text;

        // Zamie� warto�� z Slidera na typ bool
        float sliderValue = daySlider.value;
        bool sliderBoolValue = (sliderValue != 0.0f);

        // Przekazuj obie warto�ci do metody DisplayDemo w skrypcie ShowWeather
        showWeatherScript.DisplayDemo(selectedWeather, sliderBoolValue);
    }

}
