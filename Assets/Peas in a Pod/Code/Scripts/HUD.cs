using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI FoodText;

    public Slider healthSlider;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthValue(float value)
    {
        healthSlider.value = value;
    }

    public void UpdateHUDFood(float value)
    {
        FoodText.text = "Food: " + Math.Round(value).ToString();
    }
}
