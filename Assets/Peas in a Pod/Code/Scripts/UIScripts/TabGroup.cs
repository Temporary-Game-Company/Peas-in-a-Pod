using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Apple;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class TabGroup : MonoBehaviour
{

    public List<TabButton> tabs;

    public Sprite tabIdle;

    public Sprite tabHover;
    
    public Sprite tabActive;

    public TabButton selected;

    private Vector3 dest;

    public Vector3 Top;

    public Vector3 Bottom;

    public Vector3 Hovered;

    public float interpspeed;

    private RectTransform _transform;

    public void Subscribe(TabButton button)
    {
        if (tabs == null)
        {
            tabs = new List<TabButton>();
        }
        
        tabs.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        if (selected == null)
        {
            dest = Hovered;
        }
        
        ResetTabs();
        if (selected == null || button != selected)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        if (selected == null)
        {
            dest = Bottom;
        }
        if (button != selected)
        {
            button.background.sprite = tabIdle;
        }
        
    }

    public void Start()
    {
        _transform = GetComponent<RectTransform>();
        Bottom = _transform.localPosition;
        dest = Bottom;
        Hovered = Bottom + new Vector3(0, 50, 0);
        Top = Bottom + new Vector3(0, 500, 0);
        
    }

    public void Update()
    {
        _transform.localPosition = Vector3.Lerp(transform.localPosition, dest, interpspeed);
    }

    public void OnTabSelected(TabButton button)
    {
        dest = Top;
        if (selected == button)
        {
            selected.background.sprite = tabIdle;
            dest = Bottom;
            selected = null;
            return;
        }
        selected = button;
        ResetTabs();
        button.background.sprite = tabActive;
        
    }
    // Start is called before the first frame update
    void ResetTabs()
    {
        foreach(TabButton button in tabs)
        {
            if (selected != null && button == selected)
            {
                continue; 
            }
            button.background.sprite = tabIdle;
        }
    }
}
