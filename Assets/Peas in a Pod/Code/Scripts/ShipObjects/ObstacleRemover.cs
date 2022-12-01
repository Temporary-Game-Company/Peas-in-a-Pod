using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour
{

    public Obstacle.ObstacleTypes _removerType;

    private UnitRTS owner;

    public bool active; // true if it can be picked up right now

    public Vector3 offset;

    private BoxCollider2D _boxCollider;

    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (owner != null)
        {
            transform.localPosition = owner.transform.localPosition + offset;
        }
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (active && unit && !owner)
        {
            owner = unit;
        }
    }

    public void Unequip(){
        owner = null;
    }
}

