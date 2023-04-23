using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TemporaryGameCompany;

public class Food : MonoBehaviour
{
    [SerializeField] private FloatReference HungryPercent;

    void OnTriggerStay2D(Collider2D other) {
        UnitRTS pea = other.gameObject.GetComponent<UnitRTS>();
        if (pea && pea.hunger/UnitRTS.MAX_HUNGER > HungryPercent) 
        {
            pea.Eat();
            gameObject.SetActive(false);
        }
    }
}
