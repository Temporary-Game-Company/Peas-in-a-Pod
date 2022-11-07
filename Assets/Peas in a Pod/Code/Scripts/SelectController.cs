using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectController : MonoBehaviour
{
    private Vector3 startPosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(UtilsClass.GetMouseWorldPosition() + " " + startPosition);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());
            foreach(Collider2D c in collider2DArray)
            {
                Debug.Log(c);
            }

            if (collider2DArray.Length == 0)
            {
                Debug.Log("0");
            }
           
        }
        
    }
}
