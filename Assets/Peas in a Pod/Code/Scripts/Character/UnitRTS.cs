using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TemporaryGameCompany;

public delegate void BoolChangeDelegate(bool newValue);

[RequireComponent(typeof(NavMeshAgent))]
public class UnitRTS : MonoBehaviour
{
    [SerializeField] private PeaRuntimeSet PeaSet;

    private GameObject _selectedGameObject;

    [SerializeField] private float Velocity;

    private NavMeshAgent NavAgent;

    public HealthBar HealthBar;

    private Vector3 DesiredLocation;

    [SerializeField] private float StartingHeat;

    [SerializeField] private float MaximumHeat;

    private float CurrentHeat;

    static LayerMask raycastMask;

   // tracking whether pea is working, providing delegate for catching changes in state
    public BoolChangeDelegate OnWorkingChanged;
    bool _isWorking = false;
    public bool isWorking 
    {
        set {
            _isWorking = value;
            if (OnWorkingChanged != null) OnWorkingChanged(_isWorking);
        }
        get => _isWorking;
    }

    // tracking whether pea is grounded, providing delegate for catching changes in state
    public BoolChangeDelegate OnGroundedChanged;
    bool _isGrounded = true;
    private bool isGrounded 
    {
        set {
            _isGrounded = value;
            if (OnGroundedChanged != null) OnGroundedChanged(_isGrounded);
        }
        get => _isGrounded;
    }

    private void Awake()
    {
        Transform t = transform.Find("Selected");
        if (t != null)
        {
            _selectedGameObject = t.gameObject;
            if (_selectedGameObject)
            {
                _selectedGameObject.SetActive(false);
            }
            
        }

        HealthBar = GetComponentInChildren<HealthBar>();
    }

    public void SetSelectedVisible(bool visible)
    {
        if (_selectedGameObject != null)
        {
            _selectedGameObject.SetActive(visible);
        }
        
    }

    private void Start()
    {
        raycastMask = ~LayerMask.GetMask(new string[] {"Player", "ShipExterior"});

        DesiredLocation = transform.position;
        CurrentHeat = StartingHeat;
        if (HealthBar != null)
        {
            HealthBar.SetHealth(CurrentHeat, MaximumHeat);
        }
        
        isWorking = false;
        isGrounded = true;
        //TODO Add self to HUD in Units Tab
    }

    private void FixedUpdate()
    {
        // TODO consider changing this for a more performant option
        // checks if the pea is grounded using raycast, and updates isGrounded accordingly
        isGrounded = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 0.38f, raycastMask)? true : false;
        Debug.DrawRay(gameObject.transform.position, Vector2.down * 0.38f, isGrounded? Color.green : Color.red, 0.0f);
    }

    /* private void OnMouseDown()
    {
        if (GlobalSelection.instance != null)
        {
            GlobalSelection.instance.AddUnit(this.gameObject);
        }
    } */

    public void MoveTo(Vector3 targetPosition)
    {
        // NavAgent.SetDestination(targetPosition);
    }
    
    void OnEnable()
    {
        // NavAgent = GetComponent<NavMeshAgent>();
        PeaSet.Add(this);
    }
    
    void OnDisable()
    {
        PeaSet.Remove(this);
    }


}
