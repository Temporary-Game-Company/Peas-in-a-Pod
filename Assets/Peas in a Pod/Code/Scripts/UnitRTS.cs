using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class UnitRTS : MonoBehaviour
{
    private GameObject _selectedGameObject;

    [SerializeField] private float Velocity;

    private NavMeshAgent NavAgent;

    public HealthBar HealthBar;

    private Vector3 DesiredLocation;

    [SerializeField] private float StartingHeat;

    [SerializeField] private float MaximumHeat;

    private float CurrentHeat;
    
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

    private void OnEnable()
    {
        NavAgent = GetComponent<NavMeshAgent>();
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
        DesiredLocation = transform.position;
        CurrentHeat = StartingHeat;
        HealthBar.SetHealth(CurrentHeat, MaximumHeat);
        
    }

    private void Update()
    {
        
    }

    public void MoveTo(Vector3 targetPosition)
    {
        NavAgent.SetDestination(targetPosition);
    }
}
