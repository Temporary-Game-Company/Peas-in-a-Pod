using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using UnityEngine;

public class FoodConsumption : MonoBehaviour
{
    private HashSet<UnitRTS> UnitsInside = new HashSet<UnitRTS>();

    public float _exhuastionReduction;

    public FloatVariable _FoodSupply;

    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (UnitsInside.Add(unit))
                unit.AddToExhaustionDelta(-_exhuastionReduction);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (UnitsInside.Remove(unit))
            {
                unit.AddToExhaustionDelta(_exhuastionReduction);
            }
            
        }
    }
}
