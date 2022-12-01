using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour
{

    public Obstacle.ObstacleTypes _removerType;

    private UnitRTS owner = null;
    private bool isFacingRight;

    private Quaternion leftRotation = Quaternion.Euler(0f, 0, 0f);
    private Quaternion rightRotation = Quaternion.Euler(0f, 180f, 0f);

    [SerializeField] ParticleSystem particles;
    private ParticleSystem.EmissionModule emission;

    [HideInInspector] public bool active = true; // true if it can be picked up right now

    public Vector3 offset;

    private BoxCollider2D _boxCollider;

    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        active = true;
        emission = particles.emission;
        emission.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(owner);
        if (owner != null)
        {
            transform.position = owner.transform.position + (isFacingRight? -offset : offset);
            transform.rotation = isFacingRight? rightRotation : leftRotation;
        }
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (active && unit && !owner)
        {
            owner = unit;
            emission.enabled = true;
            Draggable ownerDraggable = owner.gameObject.GetComponent<Draggable>();
            if (ownerDraggable) 
            {
                ownerDraggable.OnDirectionChanged += OnChangeDirections;
                isFacingRight = ownerDraggable.isFacingRight;
            }
        }
    }

    public void Unequip(){
        Draggable ownerDraggable = owner? owner.gameObject.GetComponent<Draggable>() : null;
        owner = null;
        if (ownerDraggable) ownerDraggable.OnDirectionChanged -= OnChangeDirections;
        emission.enabled = false;
    }

    private void OnChangeDirections(bool value) {
        isFacingRight = value;
    }
}

