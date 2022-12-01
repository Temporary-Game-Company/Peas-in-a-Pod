using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI FoodText;

    public TextMeshProUGUI TemperatureText;

    public TextMeshProUGUI OxygenText;

    public TextMeshProUGUI PeasCount;

    public TextMeshProUGUI HullIntegrity;

    public Slider PowerSlider;

    public Gradient TempGradient;

    public Canvas _winScreen;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void UpdateHUDFood(float value)
    {
        FoodText.text = Math.Round(value).ToString();
    }

    public void UpdateHUDActivePeas(float active, float total)
    {
        PeasCount.text = active.ToString() + "/" + total.ToString();
    }

    public void UpdateHUDOxygen(float percent)
    {
        OxygenText.text = Math.Round((percent * 100), 1).ToString() + "%";
    }

    public void UpdateHUDIntegrity(float amt)
    {
        HullIntegrity.text = amt.ToString();
    }
    
    


    public void UpdateHUDPower(float percent)
    {
        PowerSlider.value = percent;
    }

    public void UpdateHUDTemp(float temp)
    {
        if (TempGradient != null)
        {
            TemperatureText.color = TempGradient.Evaluate(temp/60);
        }
        TemperatureText.fontSize = (48 + Math.Abs(temp - 30) * 2);
        if (TemperatureText)
        {
            TemperatureText.text = Math.Round(temp, 2).ToString() + "C";
        }
    }

    public void DisplayWinScreen()
    {
        if (_winScreen)
        {
            _winScreen.gameObject.SetActive(true);
        }
    }
}
