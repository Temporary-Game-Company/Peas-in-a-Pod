using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class EventPair
{
    public List<EventConfigSO> Events;

    public float Cooldown;
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WaveConfigSO", order = 1)]
public class WaveConfigSO : ScriptableObject
{

    
    public List<EventPair> Events;

    

    public bool bLoops;


}