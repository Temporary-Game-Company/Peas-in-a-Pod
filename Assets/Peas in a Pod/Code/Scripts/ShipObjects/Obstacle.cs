using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TemporaryGameCompany;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update

    public float Health;

    private float curHealth;

    public Slider HealthBar;



    public float _fatigueWhileInside = 3f;

    [SerializeField] bool drainsResource;
    [SerializeField] FloatVariable resourceAffected;
    float _drainRate;
    [SerializeField] float initialDrainRate;
    [SerializeField] float maxDrainRate;
    [SerializeField] float drainAcceleration;

    private List<UnitRTS> UnitsInside = new List<UnitRTS>();

    public enum ObstacleTypes
    {
        Fire,
        AirLeak
    }

    public ObstacleTypes obstacleType;
    
    void Start()
    {
        curHealth = Health;
        _drainRate = initialDrainRate;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(resourceAffected.Value);
        _drainRate = Mathf.Min(maxDrainRate, _drainRate + drainAcceleration * Time.deltaTime);
        resourceAffected.ApplyChange(-_drainRate * Time.deltaTime);
        HandleRemoval();
    }

    ObstacleRemover _remover = null;
    private void HandleRemoval()
    {
       
        if (_remover && _remover.isEquipped) curHealth -= Time.deltaTime;
        // Debug.Log(curHealth + "   " + (remover? true : false) + "   " + (remover? remover.isEquipped : false));
        if (HealthBar)
        {
            HealthBar.value = curHealth / Health;
        }
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        ObstacleRemover remover = col.GetComponent<ObstacleRemover>();
        if (remover != null)
        {
            if (remover.removerType == obstacleType)
            {
                this._remover = remover;
            }
        }

        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (!UnitsInside.Contains(unit))
            {
                UnitsInside.Add(unit);
                unit.AddToExhaustionDelta(_fatigueWhileInside);
            }
            
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
                unit.AddToExhaustionDelta(-_fatigueWhileInside);
            }
        }
        ObstacleRemover remover = other.GetComponent<ObstacleRemover>();
        if (remover != null)
        {
            if (remover.removerType == obstacleType)
            {
                _remover = null;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (UnitRTS r in UnitsInside)
        {
            r.AddToExhaustionDelta(-_fatigueWhileInside);
            UnitsInside.Remove(r);
        }
    }
}
