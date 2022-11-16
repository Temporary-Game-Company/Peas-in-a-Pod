using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    public RoomRuntimeSet RoomSet;

    public bool _producesFood;

    public bool _producesPower;

    public bool _producesHeat;

    public float _foodProducitonPerSecond;

    public GameObject _attentionIndicator;

    private SpriteRenderer attentionRenderer;

    private float damaged;

    private float maxdamage;

    public EventConfigSO.EventType affectedEvent;

    private bool _isDamaged;

    private ResourceManager _resourceManager;

    public ManagerRuntimeSet resourceManager;

    public bool usedThisWave;

    private List<UnitRTS> UnitsInside = new List<UnitRTS>();
    // Start is called before the first frame update
    void Start()
    {
        
        _attentionIndicator.SetActive(false);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_resourceManager == null)
        {
            _resourceManager = resourceManager.Items[0];
            
        }

        
        if (_producesFood)
        {
            IncreaseFood();
        }
        
        if (_isDamaged)
        {
            
            HandleRepairs();
        }
        
    }

    private void HandleRepairs()
    {
        
        damaged = (float) Math.Clamp(damaged - UnitsInside.Count * Time.deltaTime, 0, 100000.0);

        if (attentionRenderer)
        {
            attentionRenderer.color = new Color(attentionRenderer.color.r, attentionRenderer.color.g, attentionRenderer.color.b, damaged/maxdamage);
        }
        if (damaged == 0)
        {
            SystemRepaired();
        }
    }

    public void SystemDamaged(float DamageDone)
    {
        
        _isDamaged = true;
        damaged += DamageDone;
        maxdamage = damaged;
        
        _producesFood = false;
        _producesHeat = false;
        _producesPower = false;
        if (_attentionIndicator != null)
        {
            _attentionIndicator.SetActive(true);
        }
        
        
        
    }

    void SystemRepaired()
    {
        if (attentionRenderer)
        {
            attentionRenderer.color = new Color(attentionRenderer.color.r, attentionRenderer.color.g, attentionRenderer.color.b, 1f);
        }
        _isDamaged = false;
        if (_attentionIndicator != null)
        {
            _attentionIndicator.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        UnitRTS other = col.GetComponent<UnitRTS>();
        if (other != null)
        {
            UnitsInside.Add(other);
            _foodProducitonPerSecond = UnitsInside.Count;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UnitRTS unit = other.GetComponent<UnitRTS>();
        if (unit != null)
        {
            UnitsInside.Remove(unit);
            _foodProducitonPerSecond = UnitsInside.Count;
        }
    }

    private void IncreaseFood()
    {
        float toIncrease = _foodProducitonPerSecond * Time.deltaTime;
        if (_resourceManager != null)
        {
            _resourceManager.changeFood(toIncrease);
        }
    }

    private void OnDisable()
    {
        RoomSet.Remove(this);
    }
}
