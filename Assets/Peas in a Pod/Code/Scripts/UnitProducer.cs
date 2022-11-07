using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitProducer : MonoBehaviour
{
    // Start is called before the first frame update
    public int spawnTime = 3;
    public List<GameObject> SpawnableUnits;
    private GameObject spawningArea;

    public bool SpawnsAutomatically = false;
    

    
    
    void Start()
    {
        Transform t = transform.Find("SpawningArea");

        if (t != null)
        {
            spawningArea = transform.gameObject;
        }

        if (SpawnsAutomatically)
        {
            StartCoroutine(startSpawning());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnUnit()
    {
        if (SpawnableUnits.Count > 0)
        {
            Vector3 pos;
            if (spawningArea != null)
            {
                BoxCollider b = spawningArea.GetComponent<BoxCollider>();
                if (b != null)
                {
                    pos = b.bounds.ClosestPoint(new Vector3(0, 0, 0));
                    GameObject g = Instantiate(SpawnableUnits[0]);
                    
                    if (g != null)
                    {
                        g.transform.position = pos;
                    }
                }
            }
        }
    }
    
    public void spawnUnit(int num)
    {
        if (SpawnableUnits.Count >= num)
        {
            Vector3 pos;
            if (spawningArea != null)
            {
                BoxCollider b = spawningArea.GetComponent<BoxCollider>();
                if (b != null)
                {
                    pos = b.bounds.ClosestPoint(new Vector3(0, 0, 0));
                    GameObject g = Instantiate(SpawnableUnits[num]);
                    if (g != null)
                    {
                        g.transform.position = pos;
                    }
                }
            }
        }
    }

    public void UnitDestroyed()
    {
        
    }

    IEnumerator startSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnUnit();
        }
    }
}
