using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour
{

    public Obstacle.ObstacleTypes _removerType;

    private UnitRTS owner;

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

    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit)
        {
            owner = unit;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        UnitRTS unit = col.gameObject.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (_boxCollider)
            {
                _boxCollider.isTrigger = true;
            }

            owner = unit;
            _rb.gravityScale = 0f;

        }
    }
}

