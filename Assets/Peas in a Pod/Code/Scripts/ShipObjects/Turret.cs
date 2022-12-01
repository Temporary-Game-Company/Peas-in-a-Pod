using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Turret : MonoBehaviour
{
    // Start is called before the first frame update

    public bool bIsPossessed;
    public Transform loc;

    public Laser _toInstantiate;

    public Vector3 _originalSpawn;

    private Vector3 _forwardVector;

    private bool IsFiring = false;

    private Vector3 _returnAfterFiring;

    private float _timeSinceBeganFiring = 0f;

    [SerializeField] private float _laserRetentionTime = 0.5f;

    [SerializeField] public float _maxChargeTime = 1f;

    public Slider _chargeSlider;

    public float _cooldownTime = 1f;

    private float _cooldownRemaining = 0f;

    private Laser myLaser;

    public AudioSource _chargeSound;

    public AudioSource _fireSound;
    
    

    private Quaternion originalRotation;
    void Start()
    {
        _originalSpawn = loc.transform.position;
        originalRotation = loc.rotation;
        if (_chargeSlider)
        {
            _chargeSlider.value = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsPossessed)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            
            if (IsFiring)
            {
                _cooldownRemaining = _cooldownTime;
                
                IsFiring = false;
                
                Fire(_timeSinceBeganFiring/_maxChargeTime);
                _timeSinceBeganFiring = 0f;
            }
        }
        if (IsFiring)
        {
            _timeSinceBeganFiring = Math.Clamp(_timeSinceBeganFiring + Time.deltaTime, 0, _maxChargeTime);
            if (_chargeSlider)
            {
                _chargeSlider.value = _timeSinceBeganFiring / _maxChargeTime;
            }

            if (_timeSinceBeganFiring != _maxChargeTime)
            {
                loc.localPosition = _returnAfterFiring - new Vector3(0, _timeSinceBeganFiring/_maxChargeTime * 0.3f, 0f);
            }
            
        }

        if (myLaser != null)
        {
            myLaser.startPoint = loc.position;
            myLaser.endPoint = loc.position + (loc.position - transform.position).normalized * 20f;
        }

        if (_cooldownRemaining == 0)
        {
            HandleRotation();
        }
       

        if (_cooldownRemaining > 0)
        {
            loc.localPosition = _returnAfterFiring - new Vector3(0, _cooldownRemaining/_maxChargeTime * 0.3f, 0f);
            _cooldownRemaining = Math.Clamp(_cooldownRemaining - Time.deltaTime, 0, _cooldownTime);
            if (_chargeSlider)
            {
                _chargeSlider.value = _cooldownRemaining / _cooldownTime;
            }
        }

        _forwardVector = (loc.position - transform.position).normalized;


        if (Input.GetMouseButtonDown(0) && _cooldownRemaining == 0)
        {
            if (!CanFire())
            {
                return;
            }
            if (_chargeSound != null)
            {
                _chargeSound.Play();
            }
            _timeSinceBeganFiring = 0.1f;
            _returnAfterFiring = loc.localPosition;
            IsFiring = true;


        }
        

        
    }

    private bool CanFire()
    {
        RaycastHit2D r = Physics2D.Raycast(loc.position,
            loc.position + _forwardVector * 20f);
        if (r.collider == null) return true;
        return (r.collider.GetComponent<ProjectileEvent>() == null);
    }
        
    

    private void HandleRotation()
    {
        if (bIsPossessed)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector3 noX = transform.position;
            noX.z = 0;
            Vector3 newPos = mousePosition - noX;

            newPos.z = Math.Clamp(newPos.z, -30, 30);
            float Angle = Vector3.Angle(newPos, _originalSpawn - transform.position);


            if (Angle > 90 || Angle < -90)
            {
                newPos = Vector3.RotateTowards(_originalSpawn - transform.position, -newPos, (float)Math.PI / 180f * 90f,
                    (float)Math.PI / 180f * -90f);
            }

            transform.up = newPos;
        }
    }

    private void Fire(float width)
    {
        if (_fireSound != null)
        {
            if (_chargeSound && _chargeSound.isPlaying)
            {
                _chargeSound.Stop();
            }
            _fireSound.volume = width;
            
            _fireSound.Play();
        }
        RaycastHit2D r = Physics2D.Raycast(loc.position,
            loc.position + _forwardVector * 20f);


        if (r.collider == null)
        {
            Laser l = Instantiate<Laser>(_toInstantiate, loc);
            if (l != null)
            {
                myLaser = l;
                l.startPoint = loc.position;
                l.endPoint = loc.position + (loc.position - transform.position).normalized * 20f;
                l.SetLaserLineWidth(width);
            }

            Debug.DrawLine(loc.position, loc.position + (loc.position - transform.position).normalized * 10f, Color.red,
                2f);
            Destroy(l.gameObject, _laserRetentionTime);
            return;
        }
        else if (r.collider.gameObject.GetComponent<ProjectileEvent>() != null)
        {
            r.collider.gameObject.GetComponent<ProjectileEvent>().Hit();
            Debug.Log("Hit!");
            Laser l = Instantiate<Laser>(_toInstantiate, loc);
            if (l != null)
            {
                myLaser = l;
                l.startPoint = loc.position;
                l.endPoint = new Vector3(r.point.x, r.point.y, 0f) + (_forwardVector * 3f);
                l.SetLaserLineWidth(width);
            }

            Debug.DrawLine(loc.position, l.endPoint, Color.red, 2f);
            Destroy(l.gameObject, _laserRetentionTime);
        }
    }

    public void Possessed()
    {
        bIsPossessed = true;
    }

    public void Unpossess()
    {
        bIsPossessed = false;
        IsFiring = false;
        
    }
    
}
