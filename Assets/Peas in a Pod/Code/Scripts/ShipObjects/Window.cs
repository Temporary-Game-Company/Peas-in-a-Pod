using System;
using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using TMPro;
// using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


public class Window : MonoBehaviour
{

    public Sprite openSprite;

    public Sprite closedSprite;

    public bool _isOpen = false;

    private SpriteRenderer _spriteRenderer;

    private GameObject _hint;

    private int _unitsInside = 0;

    public ManagerRuntimeSet _managers;

    private ResourceManager _resourceManager;

    private TextMeshProUGUI _hintText;

    public WindowRuntimeSet Windows;

    public Image _FillImage;
    
    
    
    
   
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
            _spriteRenderer.sprite = openSprite;
            if (_hint)
            {
                _hint.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else
        {
            _spriteRenderer.sprite = closedSprite;
            if (_hint)
            {
                _hint.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        Windows.Add(this);
        if (_FillImage)
        {
            _FillImage.fillAmount = 0f;
        }
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
            if (_unitsInside == 1)
            {
                

                if (_FillImage != null)
                {
                    StartCoroutine(ToggleAnimation());
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
                
                if (_FillImage)
                {
                    _FillImage.fillAmount = 0f;
                }
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

    private IEnumerator ToggleAnimation()
    {
        
        while (_FillImage.fillAmount != 1f)
        {
            _FillImage.fillAmount += 1f * Time.deltaTime;
            if (_unitsInside == 0)
            {
                if (_FillImage)
                {
                    _FillImage.fillAmount = 0f;
                    break;
                }
            }
            yield return null;
        }

        if (_unitsInside > 0)
        {
            _FillImage.fillAmount = 0f;
            Toggle();
        }
        
        
    }

    private void Open()
    {
        _isOpen = true;
        if (_spriteRenderer)
        {
            _spriteRenderer.sprite = openSprite;
        }

        if (_resourceManager == null)
        {
            _resourceManager = _managers.Items[0];
        }

        
        if (_hint)
        {
            _hint.GetComponent<SpriteRenderer>().color = Color.green;
        }

        if (_FillImage)
        {
            _FillImage.color = Color.red;
        }
    }

    private void Close()
    {
        _isOpen = false;
        if (_spriteRenderer)
        {
            _spriteRenderer.sprite = closedSprite;
        }
        if (_resourceManager == null)
        {
            _resourceManager = _managers.Items[0];
        }

        if (_hint)
        {
            _hint.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (_FillImage)
        {
            _FillImage.color = Color.green;
        }

        
        
    }
}
