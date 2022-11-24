using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Vector3 _pointTowards;

    private void Update()
    {
        Vector3 pointAt = _pointTowards - transform.localPosition;
        transform.right = pointAt;
    }
}
