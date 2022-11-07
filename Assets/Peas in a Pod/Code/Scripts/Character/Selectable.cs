using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Selectable : MonoBehaviour
{
    private GameObject WhenSelected;

    private SphereCollider MyCollider;

    private UnitProducer unitProducer;

    private void Awake()
    {
        Transform t = transform.Find("Selected");
        if (t != null)
        {
            WhenSelected = t.gameObject;
            WhenSelected.SetActive(false);
        }

        unitProducer = GetComponent<UnitProducer>();
        MyCollider = GetComponent<SphereCollider>();
    }

    public void SetSelectableVisible(bool bVisible)
    {
        if (WhenSelected != null)
        {
            WhenSelected.SetActive(bVisible);
        }
    }

    public Vector3 GetNearbyLocation()
    {
        Vector3 CurrentLoc = transform.position;
        
       
        Vector2 v1 = Random.insideUnitCircle.normalized;
        float multiplier = 1.0f;
        if (MyCollider != null)
        {
            multiplier = MyCollider.radius;
        }

        Vector3 ToReturn =
            new Vector3(CurrentLoc.x + v1.x * multiplier, CurrentLoc.y, CurrentLoc.z + v1.y * multiplier);
        return ToReturn;
    }

    public void RightClicked()
    {
        if (unitProducer != null)
        {
            unitProducer.spawnUnit();
        }
    }
}
