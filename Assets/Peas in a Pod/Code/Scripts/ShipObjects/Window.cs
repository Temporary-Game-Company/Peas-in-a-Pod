using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using TMPro;
// using UnityEditor.UI;
using UnityEngine;

public class Window : MonoBehaviour
{

    public Color openColor;

    public Color closedColor;

    public bool _isOpen = false;

    private SpriteRenderer _spriteRenderer;

    private GameObject _hint;

    private int _unitsInside = 0;

    public ManagerRuntimeSet _managers;

    private ResourceManager _resourceManager;

    private TextMeshProUGUI _hintText;

    public WindowRuntimeSet Windows;
    
    
    
   
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

       

        Transform t = transform.Find("Pop-Up");
        if (t != null)
        {
            _hint = t.gameObject;
            if (_hint)
            {
                _hint.SetActive(false);
                _hintText = _hint.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        if (_isOpen)
        {
            _spriteRenderer.color = openColor;
            if (_hint)
            {
                _hint.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else
        {
            _spriteRenderer.color = closedColor;
            if (_hint)
            {
                _hint.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        Windows.Add(this);
    }

    private void OnDisable()
    {
        
        Windows.Remove(this);
    }


    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (_unitsInside > 0)
        {
            Toggle();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<UnitRTS>() != null)
        {
            _unitsInside++;
            if (_unitsInside > 0)
            {
                if (_hint)
                {
                    _hint.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<UnitRTS>() != null)
        {
            _unitsInside--;
            if (_unitsInside <= 0)
            {
                _hint.SetActive(false);
            }
        }
    }

    public void Toggle()
    {
        if (_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        _isOpen = true;
        if (_spriteRenderer)
        {
            _spriteRenderer.color = openColor;
        }

        if (_resourceManager == null)
        {
            _resourceManager = _managers.Items[0];
        }

        
        if (_hint)
        {
            _hint.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void Close()
    {
        _isOpen = false;
        if (_spriteRenderer)
        {
            _spriteRenderer.color = closedColor;
        }
        if (_resourceManager == null)
        {
            _resourceManager = _managers.Items[0];
        }

        if (_hint)
        {
            _hint.GetComponent<SpriteRenderer>().color = Color.red;
        }

        
        
    }
}
