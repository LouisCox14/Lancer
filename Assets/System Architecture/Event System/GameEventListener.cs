using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { } // A custom UnityEvent can be used to pass parameters, in this case the caller and any data they want to pass along.

public class GameEventListener : MonoBehaviour
{
    /*
        This class acts as an reciever for GameEvents. Any functions set up in the inspector through UnityEvents will be called when the relevant GameEvent is triggered (if the lister object is active).
    */

    [System.Serializable]
    public struct EventReaction // Struct to link game events and unity events together.
    {
        public GameEvent gameEvent;
        public CustomGameEvent reactions;
    }

    // Here an array of structs is used purely for serialization, and then compiled to a dictionary on Awake() for faster lookup times.
    [SerializeField] private EventReaction[] serializedEventReactions; 
    private Dictionary<GameEvent, CustomGameEvent> eventReactions = new Dictionary<GameEvent, CustomGameEvent>();

    void Awake()
    {
        foreach (EventReaction eventReaction in serializedEventReactions)
        {
            eventReactions.Add(eventReaction.gameEvent, eventReaction.reactions);
        }
    }

    // When an observed event is triggered, invoke the relevant UnityEvent through the dictionary.
    public void OnEventRaised(GameEvent eventRaised, Component sender, object data)
    {
        eventReactions[eventRaised].Invoke(sender, data);
    }

    // Registers as a listener to chosen events on enable so that the event script knows to message this script when the event is invoked.
    void OnEnable()
    {
        foreach (GameEvent gameEvent in eventReactions.Keys)
        {
            gameEvent.RegisterListener(this);
        }
    }

    // Unregisters from events on disable to avoid errors.
    void OnDisable()
    {
        foreach (GameEvent gameEvent in eventReactions.Keys)
        {
            gameEvent.UnregisterListener(this);
        }
    }
}
