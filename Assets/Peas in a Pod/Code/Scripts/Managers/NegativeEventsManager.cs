using System.Collections;
using System.Collections.Generic;
using TemporaryGameCompany;
using UnityEngine;

public class NegativeEventsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public RoomRuntimeSet _rooms;

    public WaveConfigSO Wave;

    private EventPair _currentEvent;

    public List<float> DamageAmounts;

    public List<NegativeEvent> NegativeEvents;

    public float _timeBetweenEvents = 10;
    void Start()
    {
        StartCoroutine(startWaves());
    }

    IEnumerator startWaves()
    {
        yield return new WaitForSeconds(0.2f);
        DoFirstEvent();
    }

    private void DoFirstEvent()
    {
        foreach (Room r in _rooms.Items)
        {
            r.usedThisWave = false;
        }
        _currentEvent = Wave.Events[0];
        for(int i = 0; i < _currentEvent.Events.Count; i++)
        {
            EventConfigSO Event = _currentEvent.Events[i];
            EventConfigSO.Difficulty d = Event.difficulty;
            if (Event.type.Equals(EventConfigSO.EventType.Food))
            {
                ExecuteFoodEvent(d);
            }else if (Event.type.Equals(EventConfigSO.EventType.Alien))
            {
                ExecuteAlienEvent(d);
            }else if(Event.type.Equals(EventConfigSO.EventType.Damage)){
                ExecuteDamageEvent(d);
            }else if (Event.type.Equals(EventConfigSO.EventType.Power))
            {
                ExecutePowerEvent(d);
            }
        }

        
        StartCoroutine(DoAnEvent(_currentEvent.Cooldown, 1));
    }

    private void NextEvent(int i)
    {
        if (i == Wave.Events.Count)
        {
            // For now just loop but this can be a win condition
            i = 0;
        }
        foreach (Room r in _rooms.Items)
        {
            r.usedThisWave = false;
        }
        _currentEvent = Wave.Events[i];
        for(int j = 0; j < _currentEvent.Events.Count; j++)
        {
            EventConfigSO Event = _currentEvent.Events[j];
            EventConfigSO.Difficulty d = Event.difficulty;
            if (Event.type.Equals(EventConfigSO.EventType.Food))
            {
                ExecuteFoodEvent(d);
            }else if (Event.type.Equals(EventConfigSO.EventType.Alien))
            {
                ExecuteAlienEvent(d);
            }else if(Event.type.Equals(EventConfigSO.EventType.Damage)){
                ExecuteDamageEvent(d);
            }else if (Event.type.Equals(EventConfigSO.EventType.Power))
            {
                ExecutePowerEvent(d);
            }
        }
    }

    private void ExecuteFoodEvent(EventConfigSO.Difficulty difficulty)
    {
        

        foreach (NegativeEvent n in NegativeEvents)
        {
            if (n.EventType == EventConfigSO.EventType.Food && n.Difficulty == difficulty)
            {
                Instantiate(n._toSpawn, n._loc, Quaternion.identity);
            }
        }
        foreach(Room r in _rooms.Items)
        {
            if (r.affectedEvent.Equals(EventConfigSO.EventType.Food) && !r.usedThisWave)
            {
                r.usedThisWave = true;
                
                if (difficulty == EventConfigSO.Difficulty.Easy)
                {
                    r.SystemDamaged(DamageAmounts[0]);
                }else if (difficulty == EventConfigSO.Difficulty.Medium)
                {
                    r.SystemDamaged(DamageAmounts[1]);
                }else if (difficulty == EventConfigSO.Difficulty.Hard)
                {
                    r.SystemDamaged(DamageAmounts[2]);
                }else if (difficulty == EventConfigSO.Difficulty.Very_Hard)
                {
                    r.SystemDamaged(DamageAmounts[3]);
                }else if (difficulty == EventConfigSO.Difficulty.Boss)
                {
                    r.SystemDamaged(DamageAmounts[4]);
                }

                return;
            }
        }
    }

    private void ExecuteDamageEvent(EventConfigSO.Difficulty difficulty)
    {
       
        foreach (NegativeEvent n in NegativeEvents)
        {
            if (n.EventType == EventConfigSO.EventType.Damage && n.Difficulty == difficulty)
            {
               
                Instantiate(n._toSpawn, n._loc, Quaternion.identity);
                
            }
        }
        foreach(Room r in _rooms.Items)
        {
            if (r.affectedEvent.Equals(EventConfigSO.EventType.Damage) && !r.usedThisWave)
            {
                r.usedThisWave = true;
                if (difficulty == EventConfigSO.Difficulty.Easy)
                {
                    r.SystemDamaged(DamageAmounts[0]);
                }else if (difficulty == EventConfigSO.Difficulty.Medium)
                {
                    r.SystemDamaged(DamageAmounts[1]);
                }else if (difficulty == EventConfigSO.Difficulty.Hard)
                {
                    r.SystemDamaged(DamageAmounts[2]);
                }else if (difficulty == EventConfigSO.Difficulty.Very_Hard)
                {
                    r.SystemDamaged(DamageAmounts[3]);
                }else if (difficulty == EventConfigSO.Difficulty.Boss)
                {
                    r.SystemDamaged(DamageAmounts[4]);
                }

                return;
            }
        }
    }
    
    private void ExecutePowerEvent(EventConfigSO.Difficulty difficulty)
    {
        foreach (NegativeEvent n in NegativeEvents)
        {
            if (n.EventType == EventConfigSO.EventType.Power && n.Difficulty == difficulty)
            {
               
                Instantiate(n._toSpawn, n._loc, Quaternion.identity);
                
            }
        }
        foreach(Room r in _rooms.Items)
        {
            if (r.affectedEvent.Equals(EventConfigSO.EventType.Power) && !r.usedThisWave)
            {
                r.usedThisWave = true;
                
                if (difficulty == EventConfigSO.Difficulty.Easy)
                {
                    r.SystemDamaged(DamageAmounts[0]);
                }else if (difficulty == EventConfigSO.Difficulty.Medium)
                {
                    r.SystemDamaged(DamageAmounts[1]);
                }else if (difficulty == EventConfigSO.Difficulty.Hard)
                {
                    r.SystemDamaged(DamageAmounts[2]);
                }else if (difficulty == EventConfigSO.Difficulty.Very_Hard)
                {
                    r.SystemDamaged(DamageAmounts[3]);
                }else if (difficulty == EventConfigSO.Difficulty.Boss)
                {
                    r.SystemDamaged(DamageAmounts[4]);
                }

                return;
            }
        }
    }
    
    private void ExecuteAlienEvent(EventConfigSO.Difficulty difficulty)
    {
       
        foreach (NegativeEvent n in NegativeEvents)
        {
            if (n.EventType == EventConfigSO.EventType.Alien && n.Difficulty == difficulty)
            {
                Instantiate(n._toSpawn, n._loc, Quaternion.identity);
            }
        }
        foreach(Room r in _rooms.Items)
        {
            if (r.affectedEvent.Equals(EventConfigSO.EventType.Alien) && !r.usedThisWave)
            {
                r.usedThisWave = true;
                if (difficulty == EventConfigSO.Difficulty.Easy)
                {
                    r.SystemDamaged(DamageAmounts[0]);
                }else if (difficulty == EventConfigSO.Difficulty.Medium)
                {
                    r.SystemDamaged(DamageAmounts[1]);
                }else if (difficulty == EventConfigSO.Difficulty.Hard)
                {
                    r.SystemDamaged(DamageAmounts[2]);
                }else if (difficulty == EventConfigSO.Difficulty.Very_Hard)
                {
                    r.SystemDamaged(DamageAmounts[3]);
                }else if (difficulty == EventConfigSO.Difficulty.Boss)
                {
                    r.SystemDamaged(DamageAmounts[4]);
                }

                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoAnEvent(float TimeToWait, int eventIndex)
    {
        yield return new WaitForSeconds(TimeToWait);
        if (eventIndex == Wave.Events.Count)
        {
            // For now just loop but this can be a win condition
            eventIndex = 0;
        }
        foreach (Room r in _rooms.Items)
        {
            r.usedThisWave = false;
        }
        _currentEvent = Wave.Events[eventIndex];
        for(int i = 0; i < _currentEvent.Events.Count; i++)
        {
            EventConfigSO Event = _currentEvent.Events[i];
            EventConfigSO.Difficulty d = Event.difficulty;
            if (Event.type.Equals(EventConfigSO.EventType.Food))
            {
                ExecuteFoodEvent(d);
            }else if (Event.type.Equals(EventConfigSO.EventType.Alien))
            {
                ExecuteAlienEvent(d);
            }else if(Event.type.Equals(EventConfigSO.EventType.Damage)){
                ExecuteDamageEvent(d);
            }else if (Event.type.Equals(EventConfigSO.EventType.Power))
            {
                ExecutePowerEvent(d);
            }
        }

        StartCoroutine(DoAnEvent(_currentEvent.Cooldown, eventIndex + 1));
    }

    private void DamageSystem(Room toDamage)
    {
        
        toDamage.SystemDamaged(5);
    }
}
