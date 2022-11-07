using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    // ReSharper disable Unity.PerformanceAnalysis
    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();
        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            UnitRTS r = go.GetComponent<UnitRTS>();
            if (r != null)
            {
                r.SetSelectedVisible(true);
            }
            else
            {
                Debug.Log("Not a unit!");
            }
            Debug.Log(go + " selected!");
        }
    }

    public void Deselect(int id)
    {
        selectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                UnitRTS r = pair.Value.GetComponent<UnitRTS>();
                if (r != null)
                {
                    r.SetSelectedVisible(false);
                }
            }
        }
        selectedTable.Clear();
        
    }

    public void MoveUnits(Vector3 Loc)
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                UnitRTS r = pair.Value.GetComponent<UnitRTS>();
                if (r != null)
                {
                    r.MoveTo(Loc);
                }
            }
        }
    }

    public void MoveUnits(Selectable Loc)
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                UnitRTS r = pair.Value.GetComponent<UnitRTS>();
                if (r != null)
                {
                    r.MoveTo(Loc.GetNearbyLocation());
                }
            }
        }
    }
}
