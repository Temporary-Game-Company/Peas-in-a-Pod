using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other) {
        UnitRTS pea = other.gameObject.GetComponent<UnitRTS>();
        if (pea && pea.hunger > 0.2 * UnitRTS.MAX_HUNGER) 
        {
            pea.Eat();
            gameObject.SetActive(false);
        }
    }
}
