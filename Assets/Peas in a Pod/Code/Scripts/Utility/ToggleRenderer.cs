using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRenderer : MonoBehaviour
{
    [SerializeField] private Renderer ToToggle;

    private void Start()
    {
        // InvokeRepeating calls a method repeatedly at a specified interval
        InvokeRepeating("ToggleScript", 0.6f, 0.6f);
    }

    private void ToggleScript()
    {
        ToToggle.enabled = !ToToggle.enabled;
    }
}
