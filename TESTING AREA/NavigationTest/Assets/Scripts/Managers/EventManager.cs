using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventInfo {}

public class ColorChangedEventInfo : EventInfo
{
    public ChromaColor newColor;
}

[System.Serializable]
public class CustomEvent : UnityEvent<EventInfo> { }


public class EventManager : MonoBehaviour {

    public enum EventType
    {
        COLOR_RED,
        COLOR_GREEN,
        COLOR_BLUE,
        COLOR_YELLOW,
        COLOR_CHANGED
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
