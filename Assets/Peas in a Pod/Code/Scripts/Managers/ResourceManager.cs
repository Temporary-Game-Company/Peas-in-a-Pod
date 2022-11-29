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

    private float initialPowerAmt = 30f;


    private float _initialOxygenAmt = 100f;

    public FloatVariable OxygenAmt;

    private float _initialSunlightAmt;

    public FloatVariable temperature;

    private float _initialTemperature = 30f;

    public FloatVariable HullIntegrity;

    public PeaRuntimeSet Peas;

    private int _activePeas;

    private float _temperatureDelta = 0f;

    public WindowRuntimeSet Windows;

    public float _timeBetweenHeatCycles = 1f;

    public float _outsideHeatValue = 50f;

    private float _lastHeatThreshold = 30;

    private float _tempCheck = 0f;

    private float _tempCheckTime = 1f;

    private float _oxygenThreshold = 1f;



    public void changePower(float oldPower, float newPower)
    {
        powerAmt.Value = Math.Clamp(powerAmt.Value, 0, initialPowerAmt);
        
        updateHUDPower();
    }

    public void changeFood(float oldAmt, float newAmt)
    {
        
        updateHUDFood();
    }

    public void changeTemp(float oldTemp, float newTemp)
    {
        
        updateHUDTemp();
    }

    public void changeIntegrity(float oldValue, float newValue)
    {
        HullIntegrity.Value = Math.Clamp(HullIntegrity.Value, 0, 100f);
        updateHUDIntegrity();
    }
    
    public void changeOxygen(float oldValue, float newValue)
    {
        OxygenAmt.Value = Math.Clamp(OxygenAmt.Value, 0, 100f);
        UpdatePassedOut();
        updateHUDOxygen();
    }

   

    // Start is called before the first frame update
    void Start()
    {
        foodAmt.Value = initialFoodAmt;
        foodAmt.ValueChanged += changeFood;
        OxygenAmt.ValueChanged += changeOxygen;
        OxygenAmt.Value = _initialOxygenAmt;
        temperature.Value = _initialTemperature;
        temperature.ValueChanged += changeTemp;
        
        powerAmt.Value = initialPowerAmt;
        powerAmt.ValueChanged += changePower;
        HullIntegrity.Value = 100f;
        HullIntegrity.ValueChanged += changeIntegrity;
        _activePeas = 0;
        updateHUDIntegrity();
        updateHUDFood();
        updateHUDPeas();
        updateHUDPower(); 
        updateHUDOxygen();
        updateHUDTemp();

        


    }
    
    private void Update()
    {

        _tempCheck += Time.deltaTime;
        if (_tempCheck > _tempCheckTime)
        {
            _tempCheck = 0f;
            CalculateHeatChange();
            HandleHeatTemperature();
        }
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
            playerHUD.UpdateHUDTemp(temperature.Value);
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

    public void UpdateAllHUD()
    {
        updateHUDIntegrity();
        updateHUDFood();
        updateHUDPeas();
        updateHUDPower(); 
        updateHUDOxygen();
        updateHUDTemp();
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

    public void ApplyShipDamage(float amt)
    {
        if (HullIntegrity)
        {
            HullIntegrity.ApplyChange(-amt);
        }
    }

    public void ConsumePower(float amt)
    {
        if (powerAmt)
        {
            powerAmt.ApplyChange(-amt);
        }
    }

    public void ConsumeOxygen(float amt)
    {
        OxygenAmt.Value -= amt;
    }
  

    private void HandleHeatTemperature()
    {
        if (temperature.Value > 20 && temperature.Value < 40)
        {
            if (_lastHeatThreshold != 30)
            {
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(-1f);
                    
                }
            }
            _lastHeatThreshold = 30;
            return;
        }
        if (temperature.Value > 50 && _lastHeatThreshold < 50)
        {
            _lastHeatThreshold = 50;
            if (_lastHeatThreshold == 30)
            {
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(2f);
                } 
            }
            else
            {
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(0.5f);
                }
            }
            
        }else if (temperature.Value > 45)
        {
            if (_lastHeatThreshold == 40)
            {
                _lastHeatThreshold = 45f;
                foreach (UnitRTS pea in Peas.Items)
                {
                    
                    pea.AddToExhaustionDelta(0.5f);
                }
            }else if (_lastHeatThreshold == 50)
            {
                _lastHeatThreshold = 45;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(-0.5f);
                    
                }
            }
            else
            {
                _lastHeatThreshold = 45;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(1.5f);
                    
                }
            }
            
        }else if (temperature.Value > 40)
        {
            if (_lastHeatThreshold == 45)
            {
                _lastHeatThreshold = 40;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(-0.5f); 
                }
            }else if (_lastHeatThreshold == 30)
            {
                _lastHeatThreshold = 40;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(1f); 
                }
            }
        }else if (temperature.Value < 10)
        {
            if (_lastHeatThreshold == 15)
            {
                _lastHeatThreshold = 10;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(0.5f); 
                }
            }
        }else if (temperature.Value < 15)
        {
            if (_lastHeatThreshold == 10)
            {
                _lastHeatThreshold = 15;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(-0.5f); 
                }
            }else if (_lastHeatThreshold == 20)
            {
                _lastHeatThreshold = 15;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(0.5f); 
                }
            } 
        }else if (temperature.Value < 20)
        {
            if (_lastHeatThreshold == 15)
            {
                _lastHeatThreshold = 20;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(-0.5f); 
                }
            }else if(_lastHeatThreshold == 30)
            {
                _lastHeatThreshold = 20;
                foreach (UnitRTS pea in Peas.Items)
                {
                    pea.AddToExhaustionDelta(1f); 
                }
            }
        }
    }

    private float CalculateHeatChange()
    {
        
        float toReturn = 0f;

        foreach (Window w in Windows.Items)
        {
            if (w._isOpen)
            {
                toReturn += 1;
            }
            else
            {
                toReturn -= 1f;
            }
        }

        temperature.ApplyChange(toReturn/10f);
        
        return toReturn/10f;
    }
    
     private void UpdatePassedOut()
    {
        float oxy = OxygenAmt.Value / _initialOxygenAmt;
        if (oxy < 0.66 && oxy > 0.33)
        {
            if (_oxygenThreshold == 1f)
            {
                _oxygenThreshold = 0.66f;
                foreach (UnitRTS r in Peas.Items)
                {
                    if (!r.IsPassedOut())
                    {
                        r.PassOut();
                        break;
                    }
                }
            }
            else if (_oxygenThreshold == 0.33f)
            {
                _oxygenThreshold = 0.66f;
                foreach (UnitRTS r in Peas.Items)
                {
                    if (r.IsPassedOut())
                    {
                        r.WakeUp();
                        break;
                    }
                }
            }
        }
        else if (oxy < 0.33 && oxy > 0)
        {
            if (_oxygenThreshold == 0.66f)
            {
                _oxygenThreshold = 0.33f;
                foreach (UnitRTS r in Peas.Items)
                {
                    if (!r.IsPassedOut())
                    {
                        r.PassOut();
                        break;
                    }
                }
            }
            else if (_oxygenThreshold == 0f)
            {
                _oxygenThreshold = 0.33f;
                foreach (UnitRTS r in Peas.Items)
                {
                    if (r.IsPassedOut())
                    {
                        r.WakeUp();
                        break;
                    }
                }
            }
        }
        else if (oxy <= 0.0)
        {
            //Lose game
        }
        else
        {
            // Oxygen is above 0.66
            if (_oxygenThreshold == 0.66f)
            {
                _oxygenThreshold = 1f;
                foreach (UnitRTS r in Peas.Items)
                {
                    if (r.IsPassedOut())
                    {
                        r.WakeUp();
                        break;
                    }
                }
            }
        }
    }
}
