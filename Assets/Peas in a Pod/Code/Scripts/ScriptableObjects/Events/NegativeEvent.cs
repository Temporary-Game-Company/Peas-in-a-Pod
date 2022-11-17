using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NegativeEvent", order = 1)]
public class NegativeEvent : ScriptableObject
{
    public EventConfigSO.Difficulty Difficulty;

    public GameObject _toSpawn;
    public EventConfigSO.EventType EventType;

    public Vector3 _loc;


}