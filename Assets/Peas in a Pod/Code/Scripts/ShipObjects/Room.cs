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

    public float _fatigueValueProducing = 2f;

    public float _fatigueValueRepairing = 3f;

    public Vector3 _cameraFocusLoc = new Vector3(0,0,-10);

    public Turret _possessedOnClicked;
    
    [SerializeField] Animator roomAnimator;
    private float _currentIncreasePerSecond = 0f;
    

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
        
        if (_repairBar != null)
                {
                    _repairBar.gameObject.SetActive(false);
                }

        StartCoroutine(UpdateProd());
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
                _timeSinceProduction = Math.Clamp(_timeSinceProduction - Time.deltaTime, 0, _productionTime + 1);
                if (roomAnimator) roomAnimator.SetBool("isActive", false);
            }
            else
            {
                if (roomAnimator) roomAnimator.SetBool("isActive", true);
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

        HandlePower();

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
            if (IsFocused)
            {
                IsFocused = false;
                _isSelected = false;
                CameraShake cs = Camera.main.GetComponent<CameraShake>();
                if (cs != null)
                {
                    //StartCoroutine(cs.BackToStart());
                }   
                if (_possessedOnClicked)
                {
                    _possessedOnClicked.Unpossess();
                }
            }
            else
            {
                IsFocused = true;
                _isSelected = false;
                CameraShake cs = Camera.main.GetComponent<CameraShake>();
                if (cs != null)
                {
                    //StartCoroutine(cs.GoToLoc(_cameraFocusLoc));
                    
                }   
                
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
            r.AddToExhaustionDelta(_fatigueValueRepairing);
            r.AddToExhaustionDelta(-_fatigueValueProducing);
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
            unit.isWorking = true;
            if(!UnitsInside.Contains(unit))
            {
                UnitsInside.Add(unit);
            }

            if (_possessedOnClicked)
            {
                _possessedOnClicked.Possessed();
            }
            unit.EnteredRoom(this);
            _currentIncreasePerSecond += unit.GetProductionPercentage();
            if (_isDamaged)
            {
                _resourceManager.increaseActivePeas();
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
                _resourceManager.decreaseActivePeas();
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
