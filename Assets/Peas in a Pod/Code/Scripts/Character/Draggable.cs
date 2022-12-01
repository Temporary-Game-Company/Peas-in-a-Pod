using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TemporaryGameCompany;

public delegate void SelectedChangeDelegate(bool newValue);
public delegate void DirectionChangeDelegate(bool newValue);

public class Draggable : MonoBehaviour
{
    private Vector3 _offset;

    [FormerlySerializedAs("RigidBody")] [SerializeField] Rigidbody2D rb2d;
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

    ObstacleRemover _obstacleRemover; // if this object has an obstacle remover attached, put it here

    public DirectionChangeDelegate OnDirectionChanged;
    bool _isFacingRight = false;
    public bool isFacingRight 
    {
        private set {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                if (OnDirectionChanged != null) OnDirectionChanged(_isFacingRight);
            }
        }
        get => _isFacingRight;
    }
    private const float epsilon = 0.01f; // speed required to flip direction

    void Start()
    {
        isSelected = false;
        _obstacleRemover = gameObject.GetComponent<ObstacleRemover>();
    }


    void OnMouseDown()
    {
        // set offsets to where on the screen you clicked.
        if (_obstacleRemover) {
            _obstacleRemover.active = false;
            _obstacleRemover.Unequip();
        }
        _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        isSelected = true;
    }

    void OnMouseDrag()
    {
        Vector3 targetDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - _offset;
       
        rb2d.velocity = targetDirection * DragVelocity;
    }

    void OnMouseUp()
    {
        if (_obstacleRemover) _obstacleRemover.active = true;
        isSelected = false;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rb2d.velocity.x) > epsilon) isFacingRight = (rb2d.velocity.x > 0); 
    }
}
