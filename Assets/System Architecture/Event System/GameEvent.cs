using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    /*
        This class acts as an emitter for GameEventListeners. Scriptable objects created using this template can be called or listened to from any scene or prefab, and when triggered a message will be sent to all listener components that are observing the relevant object.
    */

    private delegate void eventDelegateHandler(GameEvent eventRaised, Component sender, object data = null);
    private eventDelegateHandler eventDelegate;

    // On Raise(), message all linked listeners.
    public void Raise(Component sender, object data = null)
    {
        eventDelegate?.Invoke(this, sender, data);
    }

    //Adds a listener to the delegate if they aren't already on it.
    public void RegisterListener(GameEventListener listener)
    {
        eventDelegate += listener.OnEventRaised;
    }

    //Removes a listener from the delegate if they are on it.
    public void UnregisterListener(GameEventListener listener)
    {
        eventDelegate -= listener.OnEventRaised;
    }
}
