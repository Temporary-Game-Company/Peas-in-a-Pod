using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    [SerializeField] private float difficulty = 1f;
    [SerializeField] private int peaAmount = 1;
    [SerializeField] private float asteroidTimeAmount = 10f;
    
    [SerializeField] private float taskTimer = 0f;
    [SerializeField] private float damageTimer = 0f;
    
    
    void Update()
    {
        
        if (taskTimer >= asteroidTimeAmount)
        {
            Debug.Log("you're are alive");
        }
        else if (damageTimer >= asteroidTimeAmount)
        {
            Debug.Log("you're are dead");
        }
        else
        {
            taskTimer += peaAmount / difficulty * Time.deltaTime;
            damageTimer += Time.deltaTime;
        }
    }
}
