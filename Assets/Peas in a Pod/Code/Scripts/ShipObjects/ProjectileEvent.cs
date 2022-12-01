using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using TemporaryGameCompany;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _destroyOnHit;

    public Vector3 _initialLocation;

    public Vector3 _finalLocation;

    public float InterpTime;

    public ParticleSystem _particleTrail;

    public bool bApplyCameraShake;

    public float _shakeMagnitude = 0f;

    public float _shakeTime = 0f;

    public float dmgAmt = 0.2f;

    public ManagerRuntimeSet ManagerRuntimeSet;

    private ResourceManager _resourceManager;
    private Rigidbody2D rb2d;

    public AudioSource _onHit;

    [SerializeField] private float _hitsRequired = 3;

    private float _hitsTaken = 0f;
    
    void Start()
    {
        transform.localPosition = _initialLocation;
        _particleTrail.Play();
        _resourceManager = ManagerRuntimeSet.Items[0];

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = (_finalLocation - _initialLocation)/InterpTime;
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    private void OnBecameVisible()
    {
        
    }

    IEnumerator LifetimeCheck()
    {
        yield return new WaitForSeconds(InterpTime);
        
    }

    public void Hit()
    {
        if (_onHit)
        {
            _onHit.Play();
        }
        _hitsTaken++;
        if (_hitsTaken >= _hitsRequired)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ship")
        {
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
    }

    private void OnDestroy()
    {
        
        _particleTrail.Stop();
        Destroy(_particleTrail, 0.5f);
    }
}
