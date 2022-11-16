using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EventConfigSO", order = 1)]
public class EventConfigSO : ScriptableObject
{
    public enum Difficulty
    {
        Easy= 0, Medium = 1, Hard = 2, Very_Hard = 3, Boss = 4
        
    }

    public enum EventType
    {
        Food, Power, Alien, Damage
    }

    public EventType type;

    public Difficulty difficulty;
}
