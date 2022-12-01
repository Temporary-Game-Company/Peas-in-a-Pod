using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image hullIntegrityFill;
    [SerializeField] private Image powerFill;
    [SerializeField] private Image oxygenFill;
    [SerializeField] private Image temperatureFill;

    public void UpdateHUDIntegrity(float integrity) // 0-100
    {
        float scaledIntegrity = integrity / 100f;
        hullIntegrityFill.fillAmount = scaledIntegrity;
        //Debug.Log(scaledIntegrity);
    }
    
    public void UpdateHUDPower(float power) // 0-1
    {
        float scaledPower = power / 100f;
        powerFill.fillAmount = scaledPower;
        //Debug.Log(scaledPower);
    }
    
    public void UpdateHUDOxygen(float oxygen) // 0-1
    {
        float scaledOxygen = oxygen / 100f;
        oxygenFill.fillAmount = scaledOxygen;
        //Debug.Log(scaledOxygen);
    }

    public void UpdateHUDTemp(float temp) // 0-1 30 normal 50 too hot 10 too cold
    {
        float scaledTemp = (1f / 120f) * temp + 0.25f;
        temperatureFill.fillAmount = scaledTemp;
        Debug.Log(scaledTemp);
    }
}