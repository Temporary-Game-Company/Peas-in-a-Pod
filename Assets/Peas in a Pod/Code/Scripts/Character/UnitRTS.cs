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


    public FloatVariable _shipTemperature;

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
    int _hitCount;
    RaycastHit2D[] _castHits = new RaycastHit2D[2];
    static LayerMask raycastMask;
    static ContactFilter2D contactFilter;


    private float _exhaustion = 0f;

    private float _exhuastionDelta = 0f;

    public float _initialExhuastionDelta = -1f;

    public float _maxExhaustion = 20f;

    private Room _currentRoom;

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
        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;


        DesiredLocation = transform.position;
        CurrentHeat = StartingHeat;
        _exhuastionDelta = _initialExhuastionDelta;
        UpdateFatigue();
        
        isWorking = false;
        isGrounded = true;
        //TODO Add self to HUD in Units Tab
    }


    private void CheckIfGrounded()
    {
        // checks if the pea is grounded using circlecast, and updates isGrounded accordingly
        // circlecast used in place of raycast because it is a bit more tolerant
        // isGrounded = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 0.4f, raycastMask)? true : false;
        // isGrounded = Physics2D.CircleCast(gameObject.transform.position, 0.2f, Vector2.down, 0.2f, raycastMask)? true : false;
        _hitCount = Physics2D.CircleCast(gameObject.transform.position, 0.2f, Vector2.down, contactFilter, _castHits, 0.2f);
        isGrounded = false;
        for (int i = 0; i < _hitCount; i++) if (_castHits[i].collider.gameObject != gameObject) isGrounded = true;
        
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
        _exhaustion = Math.Clamp(_exhaustion + amt, 0, _maxExhaustion);
        UpdateFatigue();
    }

    private void Update()
    {
        HandleTemperatureExhuastion();
        HandleFatigue();
    }

    private void HandleTemperatureExhuastion()
    {
        if (Math.Abs(_shipTemperature.Value - 30) > 10)
        {
            
        }
    }

    private void HandleFatigue()
    {
        _exhaustion = Math.Clamp(_exhaustion + Time.deltaTime * _exhuastionDelta, 0, _maxExhaustion);
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

    public void EnteredRoom(Room Entered)
    {
        _currentRoom = Entered;
    }

    public void LeftRoom()
    {
        _currentRoom = null;
    }

}
