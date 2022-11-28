using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 startPoint;

    public Vector3 endPoint;

    private LineRenderer laserLine;
    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(.2f, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        laserLine.SetPosition(0, startPoint);
        laserLine.SetPosition(1, endPoint);
    }
}
