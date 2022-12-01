using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    public RoomRuntimeSet RoomSet;

    public bool _producesGoods = true;

    private bool IsFocused = false;

    public FloatVariable _resourceProduced;

    public float _productionAmt = 2f;

    private float _timeSinceProduction = 0f;

    public float _productionTime = 2f;

    

    private SpriteRenderer attentionRenderer;

    private GameObject _roomIndicator;

    private float damaged;

    private float maxdamage;

    public float _powerConsumptionPerSecond = 0.5f;

    public EventConfigSO.EventType affectedEvent;

    private bool _isDamaged;

    public bool bProducing;

    private ResourceManager _resourceManager;

    public ManagerRuntimeSet resourceManager;

    public bool usedThisWave;

    private RepairBar _repairBar;

    private bool _isSelected;
    
    public RepairBar _productionBar;

    public ParticleSystem damagedParticles;

    public ParticleSystem _repairedParticles;

    public ParticleSystem _productionParticles;

    public float _fatigueValueProducing = 2f;

    public float _fatigueValueRepairing = 3f;

    public Vector3 _cameraFocusLoc = new Vector3(0,0,-10);

    public Turret _possessedOnClicked;
    
    [SerializeField] Animator roomAnimator;
    private float _currentIncreasePerSecond = 0f;

    [SerializeField] private AudioClip _peaEnteredSound;

    [SerializeField] private AudioClip _productionSound;

    [SerializeField] private AudioClip _damagedSound;

    [SerializeField] private AudioClip _repairedSound;

    private AudioSource _audioSource;
    

    private List<UnitRTS> UnitsInside = new List<UnitRTS>();
    // Start is called before the first frame update
    void Start()
    {
        if (_repairBar != null)
        {
            _repairBar.gameObject.SetActive(false);
        }

        StartCoroutine(UpdateProd());

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private IEnumerator UpdateProd()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            UpdateProduction();
        }
    }

    private void Awake()
    {
        RoomSet.Add(this);
        _isDamaged = false;
        damaged = 0;
        usedThisWave = true;
        

        _repairBar = GetComponentInChildren<RepairBar>();
        if (_repairBar == null)
        {
            Debug.Log("Null repair bar");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (gameObject.name == "WeaponsRoom") Debug.Log($"bProducing: {bProducing}  Units: {UnitsInside.Count}");

        if (_resourceManager == null)
        {
            _resourceManager = resourceManager.Items[0];

        }

        if (bProducing)
        {
            if (UnitsInside.Count == 0)
            {
                _timeSinceProduction = Math.Clamp(_timeSinceProduction - Time.deltaTime, 0, _productionTime + 1);
                if (roomAnimator) roomAnimator.SetBool("isActive", false);
            }
            else
            {
                if (roomAnimator) roomAnimator.SetBool("isActive", true);
                HandlePower();
                _timeSinceProduction += Time.deltaTime * _currentIncreasePerSecond; 
            }
        } else if (roomAnimator) roomAnimator.SetBool("isActive", false);
        
        if (_timeSinceProduction > _productionTime)
        {
            _timeSinceProduction = 0f;
            HandleProduction();
            
        }

        if (_productionBar)
        {
            _productionBar.updateFill(_timeSinceProduction/_productionTime);
        }

        if (_isDamaged)
        {
            
            HandleRepairs();
            _timeSinceProduction = Math.Clamp(_timeSinceProduction - Time.deltaTime, 0, _productionTime);
        }

        

    }

    private void HandlePower()
    {
        if (_resourceManager)
        {
            _resourceManager.ConsumePower(_powerConsumptionPerSecond * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        
    }


    private void OnMouseUp()
    {
        
        
    }

    private void OnMouseExit()
    {
        
    }

    private void OnMouseEnter()
    {
        
        
    }

    private void HandleRepairs()
    {
        
        damaged = (float) Math.Clamp(damaged - _currentIncreasePerSecond * Time.deltaTime, 0, 100000.0);
        if (_repairBar != null)
        {
            _repairBar.updateFill((maxdamage - damaged)/maxdamage);
        }
        
        if (attentionRenderer)
        {
            attentionRenderer.color = new Color(attentionRenderer.color.r, attentionRenderer.color.g, attentionRenderer.color.b, damaged/maxdamage);
        }
        if (damaged == 0)
        {
            SystemRepaired();
        }
    }

    private void HandleProduction()
    {
        if (_productionSound)
        {
            if (_audioSource)
            {
                _audioSource.clip = _productionSound;
                _audioSource.Play();
            }
        }
        if (_productionParticles)
        {
            _productionParticles.Play();
        }

        if (_producesGoods)
        {
            if (_resourceProduced != null)
            {
                _resourceProduced.ApplyChange(_productionAmt);
            }
            else
            {
                Debug.Log("No resource produced");
                if (GetComponent<FoodConsumption>() != null)
                {
                    GetComponent<FoodConsumption>().ConsumeFood();
                    
                }
                
            }
            
            if (_resourceManager)
            {
                _resourceManager.UpdateAllHUD();
            }
        }
    }

    public void UpdateProduction()
    {
        _currentIncreasePerSecond = 0f;
        foreach (UnitRTS r in UnitsInside)
        {
            _currentIncreasePerSecond += r.GetProductionPercentage();
        }
    }

    

    public void SystemDamaged(float DamageDone)
    {
        
        _isDamaged = true;
        if (damagedParticles)
        {
            damagedParticles.Play();
        }
        if (_damagedSound)
        {
            if (_audioSource)
            {
                _audioSource.clip = _damagedSound;
                _audioSource.Play();
            }
        }

        if (_possessedOnClicked)
        {
            _possessedOnClicked.Unpossess();
        }
        damaged += DamageDone;
        maxdamage = damaged;
        bProducing = false;
        if (_repairBar != null)
        {
            _repairBar.gameObject.SetActive(true);
        }
        
        
        
        foreach (UnitRTS r in UnitsInside)
        {
           
            r.AddToExhaustionDelta(_fatigueValueRepairing);
            r.AddToExhaustionDelta(-_fatigueValueProducing);
        }
        
        //TODO add self to tasks tab
        
        
    }

    void SystemRepaired()
    {
        if (_repairedSound)
        {
            if (_audioSource)
            {
                _audioSource.clip = _repairedSound;
                _audioSource.Play();
            }
        }
        bProducing = true;
        if (attentionRenderer)
        {
            attentionRenderer.color = new Color(attentionRenderer.color.r, attentionRenderer.color.g, attentionRenderer.color.b, 1f);
        }
        if (_repairBar != null)
        {
            _repairBar.gameObject.SetActive(false);
        }
        _isDamaged = false;
        

        if (_repairedParticles)
        {
            _repairedParticles.Play();
        }

        if (_possessedOnClicked && UnitsInside.Count != 0)
        {
            _possessedOnClicked.Possessed();
            
        }

        foreach (UnitRTS r in UnitsInside)
        {
           
            r.AddToExhaustionDelta(-_fatigueValueRepairing);
            r.AddToExhaustionDelta(_fatigueValueProducing);
        }
        
        //TODO remove self from tasks tab
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (_peaEnteredSound)
            {
                if (_audioSource)
                {
                    _audioSource.clip = _peaEnteredSound;
                    _audioSource.Play();
                }
            }
            unit.isWorking = true;
            if(!UnitsInside.Contains(unit))
            {
                UnitsInside.Add(unit);
            }

            if (_possessedOnClicked && !_isDamaged)
            {
                _possessedOnClicked.Possessed();
            }
            unit.EnteredRoom(this);
            _currentIncreasePerSecond += unit.GetProductionPercentage();
            if (_isDamaged)
            {
                
                unit.AddToExhaustionDelta(_fatigueValueRepairing);
            }
            else
            {
                unit.AddToExhaustionDelta(_fatigueValueProducing);
            }
            
        }
    }
    
    private void CalculateProductionPerSecond(){
    
    
    }

    

    private void OnTriggerStay2D(Collider2D col)
    {
        UnitRTS unit = col.GetComponent<UnitRTS>();
        if (unit != null)
        {
            if (!unit.isWorking) unit.isWorking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            unit.isWorking = false;
            if (UnitsInside.Contains(unit))
            {
                UnitsInside.Remove(unit);
            }
            
            
            unit.LeftRoom();
            
            UpdateProduction();
            
            if (_isDamaged)
            {
               
                unit.AddToExhaustionDelta(-_fatigueValueRepairing);
            }
            else
            {
                unit.AddToExhaustionDelta(-_fatigueValueProducing);
            }

            if (UnitsInside.Count == 0)
            {
                if (_possessedOnClicked)
                {
                    _possessedOnClicked.Unpossess();
                }
            }
            
        }
    }

    public void RemoveFromWorkers(UnitRTS remove)
    {
        if (UnitsInside.Contains(remove))
        {
            UnitsInside.Remove(remove);
        }
    }

    public void AddToWorkers(UnitRTS toAdd)
    {
        if (!UnitsInside.Contains(toAdd))
        {
            UnitsInside.Add(toAdd);
        }
    }

   

    private void OnDisable()
    {
        RoomSet.Remove(this);
    }
}
