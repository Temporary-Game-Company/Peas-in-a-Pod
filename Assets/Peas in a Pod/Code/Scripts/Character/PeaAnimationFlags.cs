using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(UnitRTS))]
[RequireComponent(typeof(Animator))]
public class PeaAnimationFlags : MonoBehaviour
{
    private Animator animator;
    UnitRTS peaController;

    // Start is called before the first frame update
    void OnEnable()
    {
        animator = gameObject.GetComponent<Animator>();

        Draggable draggable = gameObject.GetComponent<Draggable>();
        draggable.OnSelectedChanged += OnSelectedChanged;
        draggable.OnDirectionChanged += OnDirectionChanged;

        peaController = gameObject.GetComponent<UnitRTS>();
        peaController.OnWorkingChanged += OnWorkingChanged;
        peaController.OnGroundedChanged += OnGroundedChanged;
        peaController.OnPassedOutChanged += UpdateSleeping;
        peaController.OnRestingChanged += UpdateSleeping;
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

    void OnDirectionChanged(bool value)
    {
        animator.SetBool("FacingRight", value);
    }

    void UpdateSleeping (bool value)
    {
        animator.SetBool("Sleeping", peaController.isPassedOut || peaController.isResting);
    }
}
