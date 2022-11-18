using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
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

            transform.localPosition = new Vector3(x, y, originalPos.z);
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void StartShaking(float dur, float m)
    {
        StartCoroutine(Shake(dur, m));
    }
}
