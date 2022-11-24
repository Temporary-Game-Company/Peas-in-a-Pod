using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStation : MonoBehaviour
{
    // Start is called before the first frame update

    private float GeneratePerSecond = 0.0f;

    private int NumUnitsInside = 0;

    private List<UnitRTS> UnitsInside;

    private ResourceManager rm;

    public bool bGenerating = true;

    public AudioSource g;
    private void OnTriggerEnter(Collider other)
    {
        UnitRTS r = other.GetComponent<UnitRTS>();
        if (r != null)
        {
            UnitsInside.Add(r);
            NumUnitsInside++;
            GeneratePerSecond += 1;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnitRTS r = other.GetComponent<UnitRTS>();
        if (r != null)
        {
            UnitsInside.Remove(r);
            NumUnitsInside--;
            
        }
    }

    void Start()
    {
        UnitsInside = new List<UnitRTS>();
        if (bGenerating)
        {
            StartCoroutine(GenerateResource());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateResource()
    {
        while (bGenerating)
        {
            yield return new WaitForSeconds(1f);
            if (rm != null)
            {
                rm.changeFood(NumUnitsInside);
                Debug.Log(NumUnitsInside);
            }
            else
            {
               
            }
        }
    }
}
