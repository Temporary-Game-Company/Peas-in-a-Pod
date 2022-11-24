using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update

    public bool bIsPossessed;
    public Transform loc;

    public GameObject _toInstantiate;
    
    

    private Quaternion originalRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsPossessed)
        {
            
        }

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        transform.up = mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_toInstantiate, loc);
        }
    }

    public void Possessed()
    {
        bIsPossessed = true;
    }
    
}
