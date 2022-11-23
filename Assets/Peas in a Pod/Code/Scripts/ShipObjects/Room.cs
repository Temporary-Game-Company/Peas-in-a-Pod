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

    public bool _producesFood;

    public bool _producesPower;

    public bool _repairsIntegrity;

    public bool _producesOxygen;

    public float _oxygenProductionPerCycle;

    public float _integrityPerProduction;

    public float _foodProducitonPerProduction;

    public float PowerProductionPerCycle;

    private float _timeSinceProduction = 0f;

    public float _productionTime = 2f;

    public GameObject _attentionIndicator;

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

    public float _fatigueValue = 2f;
    

    private List<UnitRTS> UnitsInside = new List<UnitRTS>();
    // Start is called before the first frame update
    void Start()
    {
        
        _attentionIndicator.SetActive(false);

        Transform t = transform.Find("Hovered");
        if (t != null)
        {
            _roomIndicator = t.gameObject;
            _roomIndicator.SetActive(false);
        }

        // TODO add self to HUD in rooms tab
    }

    private void Awake()
    {
        RoomSet.Add(this);
        _isDamaged = false;
        damaged = 0;
        usedThisWave = true;
        if (_attentionIndicator)
        {
            attentionRenderer = _attentionIndicator.GetComponent<SpriteRenderer>();
        }

        _repairBar = GetComponentInChildren<RepairBar>();
        if (_repairBar == null)
        {
            Debug.Log("Null repair bar");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_resourceManager == null)
        {
            _resourceManager = resourceManager.Items[0];

        }

        if (bProducing)
        {
            if (UnitsInside.Count == 0)
            {
                _timeSinceProduction -= Time.deltaTime;
            }
            _timeSinceProduction += Time.deltaTime * UnitsInside.Count; 
        }
        
        if (_timeSinceProduction > _productionTime)
        {
            HandleProduction();
            _timeSinceProduction = 0f;
        }

        if (_productionBar)
        {
            _productionBar.updateFill(_timeSinceProduction/_productionTime);
        }

        if (_isDamaged)
        {
            
            HandleRepairs();
        }

        HandlePower();

    }

    private void HandlePower()
    {
        if (_resourceManager)
        {
            _resourceManager.changePower(_powerConsumptionPerSecond * -1 * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        _isSelected = true;
        if (_roomIndicator)
        {
            SpriteRenderer s = _roomIndicator.GetComponent<SpriteRenderer>();
            if (s != null)
            {
                s.color = Color.green;
            }
        }
    }


    private void OnMouseUp()
    {
        if (_roomIndicator)
        {
            SpriteRenderer s = _roomIndicator.GetComponent<SpriteRenderer>();
            if (s != null)
            {
                s.color = Color.white;
            }
        }

        if (_isSelected && UnitsInside.Count > 0)
        {
            _isSelected = false;
            CameraShake cs = Camera.main.GetComponent<CameraShake>();
            if (cs != null)
            {
                StartCoroutine(cs.GoToLoc(cs._weaponsLocation));
            }  
        }
        
    }

    private void OnMouseExit()
    {
        if (_roomIndicator)
        {
            _roomIndicator.SetActive(false);
        }
        _isSelected = false;
    }

    private void OnMouseEnter()
    {
        
        if (_roomIndicator)
        {
            _roomIndicator.SetActive(true);
            SpriteRenderer s = _roomIndicator.GetComponent<SpriteRenderer>();
            if (s != null)
            {
                s.color = Color.yellow;
            }
        }
    }

    private void HandleRepairs()
    {
        
        damaged = (float) Math.Clamp(damaged - UnitsInside.Count * Time.deltaTime, 0, 100000.0);
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
        if (_productionParticles)
        {
            _productionParticles.Play();
        }
        if (_producesFood)
        {
            
            IncreaseFood(_foodProducitonPerProduction);
        }

        if (_producesOxygen)
        {
            IncreaseOxygen(_oxygenProductionPerCycle);
        }

        if (_repairsIntegrity)
        {
            IncreaseIntegrity(_integrityPerProduction);
        }

        if (_producesPower)
        {
            IncreasePower(PowerProductionPerCycle);
        }
    }

    private void IncreasePower(float amt)
    {
        if (_resourceManager)
        {
            _resourceManager.changePower(amt);
        }
    }

    private void IncreaseOxygen(float amt)
    {
        if (_resourceManager)
        {
            _resourceManager.changeOxygen(amt);
        }
    }

    private void IncreaseIntegrity(float amt)
    {
        if (_resourceManager)
        {
            _resourceManager.changeIntegrity(amt);
        }
    }
    
    private void IncreaseFood(float amt)
    {
        
        if (_resourceManager != null)
        {
            _resourceManager.changeFood(amt);
        }
    }

    public void SystemDamaged(float DamageDone)
    {
        _timeSinceProduction = 0f;
        _isDamaged = true;
        if (damagedParticles)
        {
            damagedParticles.Play();
        }
        damaged += DamageDone;
        maxdamage = damaged;
        bProducing = false;
        if (_repairBar != null)
        {
            _repairBar.gameObject.SetActive(true);
        }
        
        
        if (_attentionIndicator != null)
        {
            //_attentionIndicator.SetActive(true);
        }
        foreach (UnitRTS r in UnitsInside)
        {
            _resourceManager.increaseActivePeas();
        }
        
        //TODO add self to tasks tab
        
        
    }

    void SystemRepaired()
    {
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
        if (_attentionIndicator != null)
        {
            _attentionIndicator.SetActive(false);
        }

        if (_repairedParticles)
        {
            _repairedParticles.Play();
        }

        foreach (UnitRTS r in UnitsInside)
        {
            _resourceManager.decreaseActivePeas();
        }
        
        //TODO remove self from tasks tab
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS other = col.GetComponent<UnitRTS>();
        if (other != null)
        {
            UnitsInside.Add(other);
            if (_isDamaged)
            {
                _resourceManager.increaseActivePeas();
            }
            other.AddToExhaustionDelta(_fatigueValue);
            
            
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            UnitsInside.Remove(unit);
            if (_isDamaged)
            {
                _resourceManager.decreaseActivePeas();
            }
            unit.AddToExhaustionDelta(-_fatigueValue);
        }
    }

   

    private void OnDisable()
    {
        RoomSet.Remove(this);
    }
}
