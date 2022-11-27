using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TemporaryGameCompany;

public delegate void SelectedChangeDelegate(bool newValue);

public class Draggable : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField] Rigidbody2D RigidBody;
    [SerializeField] FloatReference DragVelocity;

    // tracking whether object is being dragged, providing delegate for catching changes in state
    public SelectedChangeDelegate OnSelectedChanged;
    bool _isSelected = false;
    public bool isSelected 
    {
        private set {
            _isSelected = value;
            if (OnSelectedChanged != null) OnSelectedChanged(_isSelected);
        }
        get => _isSelected;
    }

    

    void Start()
    {
        isSelected = false;
    }

    void OnMouseDown()
    {
        // set offsets to where on the screen you clicked.
        
        _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        isSelected = true;
    }

    void OnMouseDrag()
    {
        Vector3 targetDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - _offset;
       
        RigidBody.velocity = targetDirection * DragVelocity;
    }

    void OnMouseUp()
    {
        isSelected = false;
    }
}
