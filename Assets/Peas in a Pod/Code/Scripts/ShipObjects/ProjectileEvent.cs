using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using TemporaryGameCompany;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _destroyOnHit;

    public Vector3 _initialLocation;

    public Vector3 _finalLocation;

    public float InterpTime;

    private Vector3 _toAdd;

    public ParticleSystem _particleTrail;

    public bool bApplyCameraShake;

    public float _shakeMagnitude = 0f;

    public float _shakeTime = 0f;

    public float dmgAmt = 20f;

    public ManagerRuntimeSet ManagerRuntimeSet;

    private ResourceManager _resourceManager;
    
    void Start()
    {
        transform.localPosition = _initialLocation;
        _toAdd = (_finalLocation - _initialLocation) / InterpTime;
        StartCoroutine(LifetimeCheck());
        _particleTrail.Play();
        _resourceManager = ManagerRuntimeSet.Items[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = transform.localPosition + _toAdd * Time.deltaTime;

       
    }

    private void OnBecameVisible()
    {
        
    }

    IEnumerator LifetimeCheck()
    {
        yield return new WaitForSeconds(InterpTime);
        if (bApplyCameraShake)
        {
            CameraShake s = Camera.main.GetComponent<CameraShake>();
            if (s != null)
            {
                
                s.StartShaking(_shakeTime, _shakeMagnitude);
            }
            
        }

        if (_resourceManager)
        {
            _resourceManager.ApplyShipDamage(dmgAmt);
        }
        if (_destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        
        _particleTrail.Stop();
        Destroy(_particleTrail, 0.5f);
    }
}
