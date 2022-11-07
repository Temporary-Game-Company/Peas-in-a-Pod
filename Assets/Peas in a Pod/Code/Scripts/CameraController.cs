using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraController instance;
    
    public Transform followTransform;
    public Transform cameraTransform;
    public float panSpeed = 20f;

    public float fasterSpeed = 50f;

    public Vector3 zoomAmount;

    public float movementTime;

    public Vector3 newPosition;
    public Vector3 newZoom;
    public Vector3 maxZoomDelta;
    public Quaternion newRotation;
    private Vector3 InitZoom;

    public Vector3 dragStartPosition;

    public Vector3 dragCurrentPosition;

    public Vector3 rotateStartPosition;

    public Vector3 rotateCurrentPosition;

    public float rotationAmount;

    public float panBorderThickness = 10f;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;

        newRotation = transform.rotation;

        newZoom = cameraTransform.localPosition;

        InitZoom = cameraTransform.localPosition;

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    private void LateUpdate()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.transform.position;
        }
        else
        {
            HandleMovementInput();
            HandleMouseInput();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            followTransform = null;
        }
        
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * (panSpeed) * (Input.GetKey(KeyCode.LeftShift) ? fasterSpeed : 1));
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward  * -panSpeed * (Input.GetKey(KeyCode.LeftShift) ? fasterSpeed : 1));
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * panSpeed * (Input.GetKey(KeyCode.LeftShift) ? fasterSpeed : 1));
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -panSpeed * (Input.GetKey(KeyCode.LeftShift) ? fasterSpeed : 1));
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        newZoom.x = Math.Clamp(newZoom.x, 0, -maxZoomDelta.x);
        newZoom.y = Math.Clamp(newZoom.y, 0, -maxZoomDelta.y);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    private void HandleMouseInput()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }*/

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 Difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;
            
            newRotation *= Quaternion.Euler(Vector3.up * (-Difference.x/5f));
        }
    }
}
