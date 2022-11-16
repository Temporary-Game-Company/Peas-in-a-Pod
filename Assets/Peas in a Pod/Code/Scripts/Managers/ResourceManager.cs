using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TemporaryGameCompany;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    private float foodAmt = 0;

    public HUD playerHUD;

    public static ResourceManager instance;

    [SerializeField] private ManagerRuntimeSet ResourceManagerSet;

   

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
        updateHUDPower(); 
        instance = this;
    }

    private void updateHUDFood()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDFood(foodAmt);
        }
    }

    private void updateHUDPower()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHealthValue(powerAmt/initialPowerAmt);
        }
    }

    private void updateHUDHeat()
    {
        
    }
    
    void OnEnable()
    {
        ResourceManagerSet.Add(this);
    }
    
    void OnDisable()
    {
        ResourceManagerSet.Remove(this);
    }
}
