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
            ExecuteEvent(Event.difficulty, Event.type);
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
            ExecuteEvent(Event.difficulty, Event.type);
        }
    }

    private void ExecuteEvent(EventConfigSO.Difficulty difficulty, EventConfigSO.EventType type)
    {
        List<NegativeEvent> correspondingEvents = new List<NegativeEvent>();
        foreach (NegativeEvent n in NegativeEvents)
        {
            if (n.EventType == type && n.Difficulty == difficulty)
            {
                correspondingEvents.Add(n);
            }
        }

        if (correspondingEvents.Count > 0)
        {
            NegativeEvent chosenEvent = correspondingEvents[Random.Range(0, correspondingEvents.Count)];
            
            Instantiate(chosenEvent._toSpawn, chosenEvent._loc, Quaternion.identity);
            return;
        }

        foreach(Room r in _rooms.Items)
        {
            if (r.affectedEvent.Equals(EventConfigSO.EventType.Food) && !r.usedThisWave)
            {
                r.usedThisWave = true;
                r.SystemDamaged(DamageAmounts[(int)difficulty]);
                return;
            }
        }
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
            ExecuteEvent(Event.difficulty, Event.type);
        }

        StartCoroutine(DoAnEvent(_currentEvent.Cooldown, eventIndex + 1));
    }

    private void DamageSystem(Room toDamage)
    {
        
        toDamage.SystemDamaged(5);
    }
}
