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

    [SerializeField] private float _oxygenConsumptionPerSecond = 0.1f;

    [SerializeField] private FloatVariable _oxygenAmt;
    
    public BoolChangeDelegate OnRestingChanged;
    private bool _isResting;
    public bool isResting{
        private set {
            if (value != _isResting) {
                _isResting = value;
                if (OnRestingChanged != null) OnRestingChanged(_isResting);
            }
        }
        get => _isResting;
    }

    public BoolChangeDelegate OnPassedOutChanged;
    private bool _isPassedOut = false;
    public bool isPassedOut {
        private set {
            if (value != _isPassedOut) {
                _isPassedOut = value;
                if (OnPassedOutChanged != null) OnPassedOutChanged(_isPassedOut);
            }
        }
        get => _isPassedOut;
    }

    private bool _canWork = true;


    public FloatVariable _shipTemperature;

    private AudioSource _audioSource;

    [SerializeField] private AudioClip _eatingNoise;

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

    public float _maxExhaustion = 80f;

    private Room _currentRoom;


    private float _hunger = 0f; // tracks how long since the pea has eaten
    private float _hungerDelta = 1f; // how much hunger should go up per second
    public float hunger {
        private set {
            _hunger = value;
            if (_hunger / MAX_HUNGER > 0.2)
            {
                if (_selectedGameObject)
                {
                    _selectedGameObject.SetActive(true);
                }
            }
            isStarving = (_hunger >= MAX_HUNGER);
        } 
        get => _hunger;
    }
    public const float MAX_HUNGER = 40f; // max hunger before starving
    // private bool _isStarving = false; 
    public bool isStarving { // true if the pea is at or above max hunger
        private set; get;
    }
    private const float _STARVING_FATIGUE_MULTIPLIER = 4f; // how much faster fatigue is gained when pea is starving

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

        Draggable drag = GetComponent<Draggable>();
        if (drag != null)
        {
            drag.enabled = true;
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


        _audioSource = gameObject.AddComponent<AudioSource>();
    }


    private void CheckIfGrounded()
    {
        // checks if the pea is grounded using circlecast, and updates isGrounded accordingly
        // circlecast used in place of raycast because it is a bit more tolerant
        // isGrounded = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 0.4f, raycastMask)? true : false;
        // isGrounded = Physics2D.CircleCast(gameObject.transform.position, 0.2f, Vector2.down, 0.2f, raycastMask)? true : false;
        _hitCount = Physics2D.CircleCast(gameObject.transform.position, 0.2f, Vector2.down, contactFilter, _castHits, 0.2f);
        isGrounded = false;
        for (int i = 0; i < _hitCount; i++)
            if (_castHits[i].collider.gameObject != gameObject)
            {
                isGrounded = true;
            }
            else
            {
               
            }
        
        
        Debug.DrawRay(gameObject.transform.position, Vector2.down * 0.38f, isGrounded? Color.green : Color.red, 3f);
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
            
            
                
               
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        RaycastHit2D r = Physics2D.Raycast(transform.localPosition, other.transform.localPosition);
        if (unit != null && r.collider.Equals(other))
        {
            Debug.Log("Pea removed from each other");
            
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

    public void ResetExhuastionDelta()
    {
        _exhuastionDelta = _initialExhuastionDelta;
    }

    public void SetIsResting(bool bIsresting)
    {
        isResting = bIsresting;
    }

    private void Update()
    {
        HandleTemperatureExhuastion();
        HandleFatigue();

        HandleOxygen();
        // if (gameObject.name == "Pea-Alex") Debug.Log($"Hunger:{_hunger}, Fatigue:{_exhaustion}");
    }

    private void HandleOxygen()
    {
        if (_oxygenAmt)
        {
            _oxygenAmt.ApplyChange(-Time.deltaTime * _oxygenConsumptionPerSecond);
        }
    }

    private void HandleTemperatureExhuastion()
    {
        if (Math.Abs(_shipTemperature.Value - 30) > 10)
        {
            
        }
    }

    private void HandleFatigue()
    {
        hunger += _hungerDelta * Time.deltaTime;
        if (!isStarving || _exhuastionDelta < 0)
            _exhaustion = Math.Clamp(_exhaustion + Time.deltaTime * _exhuastionDelta, 0, _maxExhaustion);
        else
            _exhaustion = Math.Clamp(_exhaustion + Time.deltaTime * _exhuastionDelta * _STARVING_FATIGUE_MULTIPLIER, 0, _maxExhaustion);
        UpdateFatigue();
    }

    

    public void AddToExhaustionDelta(float value)
    {
        // Debug.Log(value);
        _exhuastionDelta = Math.Clamp(_exhuastionDelta + value, _initialExhuastionDelta, 100f);
        // Debug.Log(_exhuastionDelta);
        
    }

    public float GetProductionPercentage()
    {
        if (_isPassedOut)
        {
            return 0;
        }
        if (_exhaustion == _maxExhaustion)
        {
            return 0;
        }else if (_exhaustion / _maxExhaustion < 0.5)
        {
            return 1;
        }
        else
        {
            return 0.5f;
        }
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

    public void PassOut()
    {
        
        isPassedOut = true;
        
        Draggable drag = GetComponent<Draggable>();
        if (_currentRoom)
        {
            _currentRoom.RemoveFromWorkers(this);
        }
        if (drag != null)
        {
            drag.Disable();
        }
    }

    public void WakeUp()
    {
        isPassedOut = false;
        if (_currentRoom)
        {
            _currentRoom.AddToWorkers(this);
        }

        Draggable drag = GetComponent<Draggable>();

        if (drag != null)
        {
            drag.Disable();
        }
    }

    public bool IsPassedOut()
    {
        return isPassedOut;
    }

    public bool CanWork()
    {
        return _canWork;
    }


    public void Eat()
    {
        if (_audioSource && _eatingNoise)
        {
            _audioSource.clip = _eatingNoise;
            _audioSource.Play();

        }
        if (_selectedGameObject)
        {
            _selectedGameObject.SetActive(false);
        }
        hunger = 0;
    }
}
