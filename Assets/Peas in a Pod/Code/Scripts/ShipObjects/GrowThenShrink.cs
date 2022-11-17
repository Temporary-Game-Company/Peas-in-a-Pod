using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowThenShrink : MonoBehaviour
{
    // Start is called before the first frame update

    public float StartSize;

    public float MaxSize;

    public float TimeUp;

    public float TimeDown;

    public float MinSize;

    private bool bGoingUp;

    private Vector3 max;

    private Vector3 min;

    private Transform _parentTransform;
    void Start()
    {
        _parentTransform = GetComponent<Transform>();
        _parentTransform.localScale = new Vector3(StartSize, StartSize, StartSize);
        bGoingUp = true;
        max = new Vector3(MaxSize, MaxSize, MaxSize);
        min = new Vector3(MinSize, MinSize, MinSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (max.x - _parentTransform.localScale.x < 0.01)
        {
            bGoingUp = false;
        }
        if (bGoingUp)
        {
            _parentTransform.localScale = Vector3.Lerp(_parentTransform.localScale,
                max, Time.deltaTime / TimeUp);
        }
        else
        {
            _parentTransform.localScale = Vector3.Lerp(_parentTransform.localScale, min,
                Time.deltaTime / TimeDown);
        }
    }
}
