using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using UnityEngine;

public class FoodConsumption : MonoBehaviour
{
    private List<UnitRTS> UnitsInside = new List<UnitRTS>();

    public float _exhuastionReduction;

    public FloatVariable _FoodSupply;

    public void ConsumeFood()
    {
        foreach(UnitRTS r in UnitsInside){
            r.AddToExhuastion(-_exhuastionReduction);
            _FoodSupply.ApplyChange(-1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit != null)
        {
            UnitsInside.Add(unit);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (UnitsInside.Contains(unit))
            {
                UnitsInside.Remove(unit);
            }
            
        }
    }
}
