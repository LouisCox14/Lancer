using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace ReturnEvents
{
    public class EventArgsGeneric<Type> : EventArgs
    {
        public List<Type> returnData { get; set; }

        public EventArgsGeneric()
        {
            returnData = new List<Type>();
        }
    }

    public class ReturnEvent<T>
    {
        public delegate void eventDelegateHandler(Component sender, object context, EventArgsGeneric<T> e);
        public eventDelegateHandler handler;

        public List<T> Invoke(Component sender, object context)
        {
            EventArgsGeneric<T> eventArgs = new EventArgsGeneric<T>();
            handler?.Invoke(sender, context, eventArgs);
            return eventArgs.returnData;
        }
    }
}