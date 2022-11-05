using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public Slider slider;

    public void SetHealth(float CurHealth, float MaxHealth)
    {
        slider.value = CurHealth / MaxHealth;
    }

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
}
