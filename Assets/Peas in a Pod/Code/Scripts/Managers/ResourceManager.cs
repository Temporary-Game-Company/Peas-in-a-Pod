using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TemporaryGameCompany;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    public FloatVariable foodAmt;

    public HUD playerHUD;

    

    [SerializeField] private ManagerRuntimeSet ResourceManagerSet;
    
    

    private float initialFoodAmt = 100f;

    public FloatVariable powerAmt;

    private float initialPowerAmt = 100f;


    private float _initialOxygenAmt = 100f;

    public FloatVariable OxygenAmt;

    private float _initialSunlightAmt;

    public FloatVariable temperature;

    private float _initialTemperature = 50f;

    public FloatVariable HullIntegrity;

    public PeaRuntimeSet Peas;

    private int _activePeas;

    private float _temperatureDelta = 0f;

    

    public void changePower(float delta)
    {
        powerAmt.Value = Math.Clamp(powerAmt.Value + delta, 0, initialPowerAmt);
        
        updateHUDPower();
    }

    public void changeFood(float delta)
    {
        foodAmt.Value += delta;
        updateHUDFood();
    }

    public void changeTemp(float delta)
    {
        temperature.Value += delta;
        updateHUDTemp();
    }

    public void changeIntegrity(float delta)
    {
        HullIntegrity.Value = Math.Clamp(HullIntegrity.Value + delta, 0, 100f);
        updateHUDIntegrity();
    }

    public void changeOxygen(float delta)
    {
        OxygenAmt.Value = Math.Clamp(OxygenAmt.Value + delta, 0, 100f);
        updateHUDOxygen();
    }
    // Start is called before the first frame update
    void Start()
    {
        foodAmt.Value = initialFoodAmt;
        OxygenAmt.Value = _initialOxygenAmt;
        temperature.Value = _initialTemperature;
        powerAmt.Value = initialPowerAmt;
        HullIntegrity.Value = 100f;
        _activePeas = 0;
        updateHUDIntegrity();
        updateHUDFood();
        updateHUDPeas();
        updateHUDPower(); 
        updateHUDOxygen();
        updateHUDTemp();
        


    }

    private void updateHUDFood()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDFood(foodAmt.Value);
        }
    }

    private void updateHUDPower()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDPower(powerAmt.Value/initialPowerAmt);
            
        }
    }

    private void updateHUDIntegrity()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDIntegrity(HullIntegrity.Value);
        }
    }

    private void updateHUDOxygen()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDOxygen(OxygenAmt.Value/_initialOxygenAmt);
        }
    }

    private void updateHUDTemp()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDTemp(temperature.Value/100f);
        }
    }

    
    
    void OnEnable()
    {
        ResourceManagerSet.Add(this);
    }
    
    void OnDisable()
    {
        ResourceManagerSet.Remove(this);
    }

    public void increaseActivePeas()
    {
        _activePeas++;
        updateHUDPeas();
    }

    private void updateHUDPeas()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDActivePeas(_activePeas, Peas.Items.Count);
        }
    }

    public void decreaseActivePeas()
    {
        _activePeas--;
        updateHUDPeas();
    }

    public void IncreaseTemperatureDelta(float amt)
    {
        _temperatureDelta += amt;
    }

    public void DecreaseTemperatureDelta(float amt)
    {
        _temperatureDelta -= amt;
    }

    private void Update()
    {
        if (_temperatureDelta != 0)
        {
            temperature.Value += _temperatureDelta * Time.deltaTime;
        }
        updateHUDTemp();
    }
}
