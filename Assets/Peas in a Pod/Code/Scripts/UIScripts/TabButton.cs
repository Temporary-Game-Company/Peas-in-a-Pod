using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public Image background;

    public TabGroup tabGroup;
    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (tabGroup)
        {
            tabGroup.OnTabSelected(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tabGroup)
        {
            tabGroup.OnTabExit(this);
        }
    }
}
