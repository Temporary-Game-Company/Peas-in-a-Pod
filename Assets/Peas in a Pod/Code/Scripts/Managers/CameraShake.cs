using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Vector3 _startingPosition;

    public Vector3 _weaponsLocation;

    private Vector3 _lastLocation;

    public float InterpTime = 1f;

    
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            magnitude -= (Time.deltaTime / duration) * magnitude;
            elapsed += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void StartShaking(float dur, float m)
    {
        StartCoroutine(Shake(dur, m));
    }

    public IEnumerator GoToLoc(Vector3 loc)
    {
        float timeElapsed = 0;
        Vector3 addPerSecond = (loc - transform.localPosition) / InterpTime;
        _lastLocation = transform.localPosition;
        do
        {
            timeElapsed += Time.deltaTime;
            transform.localPosition += addPerSecond * Time.deltaTime;
            yield return null;
        } while (timeElapsed < InterpTime);

        transform.localPosition = loc;
    }

    public IEnumerator BackToStart()
    {
        float timeElapsed = 0;
        Vector3 addPerSecond = (_startingPosition - transform.localPosition) / InterpTime;
        
        do
        {
            timeElapsed += Time.deltaTime;
            transform.localPosition += addPerSecond * Time.deltaTime;
            yield return null;
        } while (timeElapsed < InterpTime);

        transform.localPosition = _startingPosition;
    }
    
    
    
    
}
