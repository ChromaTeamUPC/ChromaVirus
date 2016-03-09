using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventInfo
{
    //Events that need no extra info, can use this object to improve performance
    public static EventInfo emptyInfo = new EventInfo();
}

public class PlayerDamagedEventInfo : EventInfo
{
    public int damage;
    public int currentHealth;
}

public class ColorEventInfo : EventInfo
{
    public ChromaColor newColor;
}

public class CameraEventInfo : EventInfo
{
    public GameObject newCamera;
}

[System.Serializable]
public class CustomEvent : UnityEvent<EventInfo> { }


public class EventManager : MonoBehaviour {

    public enum EventType
    {
        PLAYER_SPAWNED,
        PLAYER_DAMAGED,
        PLAYER_DIED,
        ENEMY_SPAWNED,
        ENEMY_DIED,
        COLOR_CHANGED,
        CAMERA_CHANGED
    }

    private Dictionary<EventType, CustomEvent> eventDictionary;

    void Awake()
    {
        eventDictionary = new Dictionary<EventType, CustomEvent>();
    }

    public void StartListening(EventType eventType, UnityAction<EventInfo> listener)
    {
        CustomEvent thisEvent = null;
        if(eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new CustomEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventType, thisEvent);
        }
    }

    public void StopListening(EventType eventType, UnityAction<EventInfo> listener)
    {
        CustomEvent thisEvent = null;
        if(eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void TriggerEvent(EventType eventType, EventInfo eventInfo)
    {
        CustomEvent thisEvent = null;
        if(eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke(eventInfo);
        }
    }

}
