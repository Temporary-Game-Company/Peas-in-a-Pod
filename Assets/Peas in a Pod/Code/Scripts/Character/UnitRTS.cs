using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TemporaryGameCompany;
using UnityEngine.UI;

public delegate void BoolChangeDelegate(bool newValue);

[RequireComponent(typeof(NavMeshAgent))]
public class UnitRTS : MonoBehaviour
{
    [SerializeField] private PeaRuntimeSet PeaSet;

    private GameObject _selectedGameObject;

    [SerializeField] private float Velocity;

    private NavMeshAgent NavAgent;

    public Slider FatigueSlider;

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

    private float _exhaustion = 0f;

    private float _exhuastionDelta = 0f;

    public float _maxExhaustion = 20f;

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
        // raycastMask = ~LayerMask.GetMask(new string[] {"Player", "ShipExterior"});
        raycastMask = LayerMask.GetMask(new string[] {"Ship"});

        DesiredLocation = transform.position;
        CurrentHeat = StartingHeat;
        UpdateFatigue();
        
        isWorking = false;
        isGrounded = true;
        //TODO Add self to HUD in Units Tab
    }


    private void CheckIfGrounded()
    {
        // TODO consider changing this for a more performant option
        // checks if the pea is grounded using raycast, and updates isGrounded accordingly
        isGrounded = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 0.4f, raycastMask)? true : false;
        Debug.DrawRay(gameObject.transform.position, Vector2.down * 0.38f, isGrounded? Color.green : Color.red, 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckIfGrounded();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        CheckIfGrounded();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        RaycastHit2D r = Physics2D.Raycast(transform.localPosition, col.transform.localPosition);
        if (unit != null)
        {
            
            if (r.collider.Equals(col))
            {
                AddToExhaustionDelta(-1f);
                Debug.Log("Peas in a pod");
            }
            else
            {
                Debug.Log("Obstructed");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        RaycastHit2D r = Physics2D.Raycast(transform.localPosition, other.transform.localPosition);
        if (unit != null && r.collider.Equals(other))
        {
            Debug.Log("Pea removed from each other");
            AddToExhaustionDelta(1f);
        }
    }

    private void UpdateFatigue()
    {
        if (FatigueSlider != null)
        {
            FatigueSlider.value = _exhaustion / _maxExhaustion;
        }
    }

    public void AddToExhuastion(float amt)
    {
        _exhaustion += amt;
    }

    private void Update()
    {
        HandleFatigue();
    }

    private void HandleFatigue()
    {
        _exhaustion += _exhuastionDelta * Time.deltaTime;
        UpdateFatigue();
    }

    public void AddToExhaustionDelta(float value)
    {
        _exhuastionDelta += value;
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
