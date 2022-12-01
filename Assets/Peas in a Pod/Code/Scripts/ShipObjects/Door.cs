using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private MeshRenderer _renderer;

    public Material _lit;

    public Material _unlit;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (_renderer)
        {
            if (_unlit)
            {
                _renderer.material = _unlit;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS other = col.GetComponent<UnitRTS>();
        if (other)
        {
            if (_renderer)
            {
                if (_lit != null)
                {
                    _renderer.material = _lit; 
                }
                
            }

           
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (_renderer)
            {
                if (_unlit)
                {
                    _renderer.material = _unlit;
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitRTS>() != null)
        {
            Debug.Log("Pea has entered door!");
        }
    }
}
