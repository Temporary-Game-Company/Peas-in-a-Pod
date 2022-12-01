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

    private int removersInside = 0;

    [SerializeField] bool drainsResource;
    [SerializeField] FloatVariable resourceAffected;
    float _drainRate;
    [SerializeField] float initialDrainRate;
    [SerializeField] float maxDrainRate;
    [SerializeField] float drainAcceleration;

    public enum ObstacleTypes
    {
        Fire,
        AirLeak
    }

    public ObstacleTypes _obstacleType;
    
    void Start()
    {
        curHealth = Health;
        Debug.Log(curHealth);
        _drainRate = initialDrainRate;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(resourceAffected.Value);
        _drainRate = Mathf.Min(maxDrainRate, _drainRate + drainAcceleration * Time.deltaTime);
        resourceAffected.ApplyChange(Mathf.Min(0f, -_drainRate * Time.deltaTime));
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
        Debug.Log("entered");
        ObstacleRemover remover = col.GetComponent<ObstacleRemover>();
        if (remover != null)
        {
            if (remover._removerType == _obstacleType)
            {
                Debug.Log("Removing");
                Debug.Log(removersInside);
                removersInside++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
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
