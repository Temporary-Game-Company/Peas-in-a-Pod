using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TemporaryGameCompany;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] FloatVariable foodAmount;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject[] foodPool; // pool of food objects

    // Start is called before the first frame update
    void OnEnable()
    {
        foodAmount.ValueChanged += SpawnFood;
        foreach(GameObject foodObject in foodPool) foodObject.SetActive(false); // disable all food in the pool
    }

    void OnDisable()
    {
        foodAmount.ValueChanged -= SpawnFood;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnFood(float oldValue, float newValue)
    // enable one food from the pool 
    {
        Debug.Log($"{oldValue}, {newValue}");
        if (newValue > oldValue)
            foreach (GameObject foodObject in foodPool) if (foodObject.activeSelf == false) {
                foodObject.transform.position = spawnPoint.position;
                foodObject.SetActive(true);
                break;
            }
    }
}
