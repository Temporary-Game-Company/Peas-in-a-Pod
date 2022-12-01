using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 startPoint;

    public Vector3 endPoint;

    private LineRenderer laserLine;

    private float _widthAmt;
    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
		laserLine.startWidth = _widthAmt;
		laserLine.endWidth = _widthAmt + 0.1f;
        laserLine.positionCount = 2;
        laserLine.enabled = true;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        laserLine.gameObject.transform.position = startPoint;
        laserLine.SetPosition(0, startPoint);
        laserLine.SetPosition(1, endPoint);
    }

    public void SetLaserLineWidth(float width)
    {
        _widthAmt = width;
    }
    
    
}
