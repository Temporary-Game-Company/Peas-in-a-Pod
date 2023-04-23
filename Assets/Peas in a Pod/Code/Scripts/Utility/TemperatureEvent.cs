using System.Diagnostics;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TemporaryGameCompany;

public class TemperatureEvent : MonoBehaviour
{
    [SerializeField] private GameEvent TempEvent;
    [SerializeField] private Volume Volume;
    [SerializeField] private GameEventListener EventListener;

    [SerializeField] private int Duration;
    // [SerializeField] private float TemperatureChange;
    // [SerializeField] private FloatReference Temperature;

    // Start is called before the first frame update
    void Start()
    {
        Volume.weight = 0;

        // Raise event to let other temperature objects to disable.
        EventListener.enabled = false;
        TempEvent.Raise();
        EventListener.enabled = true;

        FadeIn();
    }

    private async void FadeIn()
    {
        while (Volume.weight < 1)
        {
            Volume.weight += 0.05f;
            await Task.Delay(100);
        }
        Volume.weight = 1;


        await Task.Delay(Duration);
        TurnOff();
    }

    public async void TurnOff()
    {
        while (Volume.weight > 0)
        {
            Volume.weight -= 0.05f;
            await Task.Delay(100);
        }

        if (this != null)
        {
            Destroy(gameObject);
        }
    }
    
}
