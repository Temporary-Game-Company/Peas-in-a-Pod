using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    private float foodAmt = 0;

    public TextMeshProUGUI foodText;

    public static ResourceManager instance;

    public float initialFoodAmt = 100f;

    private float powerAmt = 0;

    public float initialPowerAmt = 100f;

    private float heatAmt = 0;

    public float initialHeatAmt = 100f;

    public void changeHeat(float delta)
    {
        heatAmt += delta;
        updateHUDHeat();
    }

    public void changePower(float delta)
    {
        powerAmt += delta;
        updateHUDPower();
    }

    public void changeFood(float delta)
    {
        foodAmt += delta;
        updateHUDFood();
    }
    // Start is called before the first frame update
    void Start()
    {
        foodAmt = initialFoodAmt;
        powerAmt = initialPowerAmt;
        heatAmt = initialFoodAmt;
        updateHUDFood();
        instance = this;
    }

    private void updateHUDFood()
    {
        if (foodText)
        {
            foodText.text = "Food: " + foodAmt.ToString();
        }
    }

    private void updateHUDPower()
    {
        
    }

    private void updateHUDHeat()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
