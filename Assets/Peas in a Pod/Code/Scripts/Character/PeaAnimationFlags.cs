using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(UnitRTS))]
[RequireComponent(typeof(Animator))]
public class PeaAnimationFlags : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        animator = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<Draggable>().OnSelectedChanged += OnSelectedChanged;
        UnitRTS peaController = gameObject.GetComponent<UnitRTS>();
        peaController.OnWorkingChanged += OnWorkingChanged;
        peaController.OnGroundedChanged += OnGroundedChanged;
    }

    void OnSelectedChanged(bool value)
    {
        animator.SetBool("Held", value);
        if (value) animator.SetTrigger("Grab");
    }

    void OnWorkingChanged(bool value)
    {
        animator.SetBool("Working", value);
    }

    void OnGroundedChanged(bool value)
    {
        animator.SetBool("Grounded", value);
    }
}
