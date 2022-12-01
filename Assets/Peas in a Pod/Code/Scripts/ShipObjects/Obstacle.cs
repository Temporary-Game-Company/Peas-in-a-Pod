using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update

    public float Health;

    private float curHealth;

    public Slider HealthBar;

    private int removersInside = 0;

    public float _fatigueWhileInside = 3f;

    public enum ObstacleTypes
    {
        Fire
    }

    public ObstacleTypes _obstacleType;
    
    void Start()
    {
        curHealth = Health;
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleRemoval();
    }

    private void HandleRemoval()
    {
        curHealth = Math.Clamp(curHealth - removersInside * Time.deltaTime, 0, Health);
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
            if (remover._removerType == _obstacleType)
            {
                removersInside++;
            }
        }

        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit != null)
        {
            unit.AddToExhaustionDelta(_fatigueWhileInside);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            unit.AddToExhaustionDelta(-_fatigueWhileInside);
        }
        ObstacleRemover remover = other.GetComponent<ObstacleRemover>();
        if (remover != null)
        {
            if (remover._removerType == _obstacleType)
            {
                removersInside--;
            }
        }
    }
}
