using System;
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

    

    [SerializeField] private ManagerRuntimeSet ResourceManagerSet;
    
    

    public float initialFoodAmt = 100f;

    private float powerAmt = 0;

    public float initialPowerAmt = 100f;


    public float _initialOxygenAmt = 100f;

    private float OxygenAmt;

    public float _initialSunlightAmt;

    private float SunlightAmt;

    public float HullIntegrity = 100f;

    public PeaRuntimeSet Peas;

    private int _activePeas;
    
    

    

    public void changePower(float delta)
    {
        powerAmt = Math.Clamp(powerAmt + delta, 0, initialPowerAmt);
        
        updateHUDPower();
    }

    public void changeFood(float delta)
    {
        foodAmt += delta;
        updateHUDFood();
    }

    public void changeSunlight(float delta)
    {
        SunlightAmt += delta;
        updateHUDSunlight();
    }

    public void changeIntegrity(float delta)
    {
        HullIntegrity = Math.Clamp(HullIntegrity + delta, 0, 100f);
        updateHUDIntegrity();
    }

    public void changeOxygen(float delta)
    {
        OxygenAmt = Math.Clamp(OxygenAmt + delta, 0, 100f);
        updateHUDOxygen();
    }
    // Start is called before the first frame update
    void Start()
    {
        foodAmt = initialFoodAmt;
        powerAmt = initialPowerAmt;
        
        OxygenAmt = _initialOxygenAmt;
        _activePeas = 0;
        updateHUDIntegrity();
        updateHUDFood();
        updateHUDPeas();
        updateHUDPower(); 
        updateHUDOxygen();
        updateHUDSunlight();
        


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
            playerHUD.UpdateHUDPower(powerAmt/initialPowerAmt);
            
        }
    }

    private void updateHUDIntegrity()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDIntegrity(HullIntegrity);
        }
    }

    private void updateHUDOxygen()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDOxygen(OxygenAmt/_initialOxygenAmt);
        }
    }

    private void updateHUDSunlight()
    {
        if (playerHUD)
        {
            playerHUD.UpdateHUDSunlight(SunlightAmt);
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
}
