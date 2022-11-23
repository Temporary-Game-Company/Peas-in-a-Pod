using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorGradient : MonoBehaviour
{
    private Image img;

    public Gradient _Gradient;
    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (img)
        {
            img.color = _Gradient.Evaluate(img.fillAmount);
            Debug.Log(img.fillAmount);
        }
    }
}
