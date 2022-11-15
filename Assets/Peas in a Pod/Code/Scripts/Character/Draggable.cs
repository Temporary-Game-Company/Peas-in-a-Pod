using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TemporaryGameCompany;

public class Draggable : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField] Rigidbody2D RigidBody;
    [SerializeField] FloatReference DragVelocity;



    void OnMouseDown()
    {
        // set offsets to where on the screen you clicked.
        _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        
    }

    void OnMouseDrag()
    {
        Vector3 targetDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - _offset;
        Debug.Log(targetDirection.ToString());
    
        RigidBody.velocity = targetDirection * DragVelocity;
    }

    void OnMouseUp()
    {
        
    }
}
