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
    
        StartCoroutine(DoAnEvent(RandomEvent()));
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

        List<Room> correspondingRooms = new List<Room>();
        foreach(Room r in _rooms.Items)
        {
            if (r.affectedEvent.Equals(type) && !r.usedThisWave)
            {
                correspondingRooms.Add(r);
            }
        }
        
        if (correspondingRooms.Count > 0)
        {
            Room chosenRoom = correspondingRooms[Random.Range(0, correspondingRooms.Count)];
            
            chosenRoom.usedThisWave = true;
            chosenRoom.SystemDamaged(DamageAmounts[(int)difficulty]);
            return;
        }
    }

    IEnumerator DoAnEvent(EventPair currentEvent)
    {
        foreach (Room r in _rooms.Items)
        {
            r.usedThisWave = false;
        }
        for(int i = 0; i < currentEvent.Events.Count; i++)
        {
            EventConfigSO Event = currentEvent.Events[i];
            ExecuteEvent(Event.difficulty, Event.type);
        }

        yield return new WaitForSeconds(currentEvent.Cooldown);
        StartCoroutine(DoAnEvent(RandomEvent()));
    }

    private EventPair RandomEvent()
    {
        return Wave.Events[Random.Range(0, Wave.Events.Count)];
    }

    private void DamageSystem(Room toDamage)
    {
        
        toDamage.SystemDamaged(5);
    }
}
