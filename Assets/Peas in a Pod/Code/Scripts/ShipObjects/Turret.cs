using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update

    public bool bIsPossessed;
    public Transform loc;

    public Laser _toInstantiate;

    public Vector3 _originalSpawn;
    
    

    private Quaternion originalRotation;
    void Start()
    {
        _originalSpawn = loc.position;
    }

    // Update is called once per frame
    void Update()
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
            /*
            if (Angle > 90 || Angle < -90)
            {
                newPos = Vector3.RotateTowards(_originalSpawn - transform.position, newPos, (float)Math.PI/180f * 90f, (float)Math.PI/180f * -90f);
            }
            */
            transform.up = newPos; 
        }

        
        

        if (Input.GetMouseButtonDown(0))
        {
            if (!bIsPossessed)
            {
                return;
            }
            RaycastHit2D r = Physics2D.Raycast(loc.position,
                loc.position + (loc.position - transform.position).normalized * 20f);
            

            if (r.collider == null)
            {
                Laser l = Instantiate<Laser>(_toInstantiate, loc);
                if (l != null)
                {
                    l.startPoint = loc.position;
                    l.endPoint = loc.position + (loc.position - transform.position).normalized * 20f;
                }
                Debug.DrawLine(loc.position, loc.position + (loc.position - transform.position).normalized * 10f, Color.red, 2f);
                Destroy(l.gameObject, 0.5f);
                return;
            }else if (r.collider.gameObject.GetComponent<ProjectileEvent>() != null)
            {
                Debug.Log("Hit!");
                Laser l = Instantiate<Laser>(_toInstantiate, loc);
                if (l != null)
                {
                    l.startPoint = loc.position;
                    l.endPoint = r.point;
                }
                Debug.DrawLine(loc.position, l.endPoint, Color.red, 2f);
                Destroy(l.gameObject, 0.5f);
            }
            else
            {
                Laser l = Instantiate<Laser>(_toInstantiate, loc);
                if (l != null)
                {
                    l.startPoint = loc.position;
                    l.endPoint = r.point;
                } 
                //Destroy(l.gameObject, 2f);
            }
           
            
        }
    }

    public void Possessed()
    {
        bIsPossessed = true;
    }

    public void Unpossess()
    {
        bIsPossessed = false;
        transform.up = _originalSpawn;
    }
    
}
