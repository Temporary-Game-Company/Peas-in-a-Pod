using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] GameObject planet;
    [SerializeField] Vector2 spawnDelay; // x is min, y is max
    [SerializeField] Vector3 startPosition;

    void OnEnable() {
        StartCoroutine("SpawnPlanets");
    }

    void OnDisable() {
        StopCoroutine("SpawnPlanets");
    }

    private IEnumerator SpawnPlanets() {
        /*
        creates a planet every x to y seconds
        planets are responsible for destroying themselves
        */
        while (true) {
            yield return new WaitForSeconds(Random.Range(spawnDelay.x, spawnDelay.y));
            GameObject.Instantiate(planet, startPosition, new Quaternion(), transform);
        }
    }

}
