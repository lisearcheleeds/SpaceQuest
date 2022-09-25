using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public interface IMessageBusBroadcaster
    {
    }

    public class MessageBusBroadcaster : IMessageBusBroadcaster
    {
        List<Action> listenerList = new List<Action>();
        List<Action> afterListenerList = new List<Action>();
        
        public void Broadcast()
        {
            foreach (var listener in listenerList)
            {
                listener();
            }

            foreach (var afterListener in afterListenerList)
            {
                afterListener();
            }
        }

        public void AddListener(Action callback)
        {
            listenerList.Add(callback);
        }

        public void RemoveListener(Action callback)
        {
            listenerList.Remove(callback);
        }

        public void AddAfterListener(Action callback)
        {
            afterListenerList.Add(callback);
        }

        public void RemoveAfterListener(Action callback)
        {
            afterListenerList.Remove(callback);
        }

        public void Clear()
        {
            listenerList.Clear();
            afterListenerList.Clear();
        }
    }

    public class MessageBusBroadcaster<T> : IMessageBusBroadcaster
    {
        List<Action<T>> listenerList = new List<Action<T>>();
        List<Action<T>> afterListenerList = new List<Action<T>>();
        
        public void Broadcast(T value)
        {
            foreach (var listener in listenerList)
            {
                listener(value);
            }

            foreach (var afterListener in afterListenerList)
            {
                afterListener(value);
            }
        }

        public void AddListener(Action<T> callback)
        {
            listenerList.Add(callback);
        }

        public void RemoveListener(Action<T> callback)
        {
            listenerList.Remove(callback);
        }

        public void AddAfterListener(Action<T> callback)
        {
            afterListenerList.Add(callback);
        }

        public void RemoveAfterListener(Action<T> callback)
        {
            afterListenerList.Remove(callback);
        }

        public void Clear()
        {
            listenerList.Clear();
            afterListenerList.Clear();
        }
    }
    
    public class MessageBusBroadcaster<T1, T2> : IMessageBusBroadcaster
    {
        List<Action<T1, T2>> listenerList = new List<Action<T1, T2>>();
        List<Action<T1, T2>> afterListenerList = new List<Action<T1, T2>>();
        
        public void Broadcast(T1 value1, T2 value2)
        {
            foreach (var listener in listenerList)
            {
                listener(value1, value2);
            }

            foreach (var afterListener in afterListenerList)
            {
                afterListener(value1, value2);
            }
        }

        public void AddListener(Action<T1, T2> callback)
        {
            listenerList.Add(callback);
        }

        public void RemoveListener(Action<T1, T2> callback)
        {
            listenerList.Remove(callback);
        }

        public void AddAfterListener(Action<T1, T2> callback)
        {
            afterListenerList.Add(callback);
        }

        public void RemoveAfterListener(Action<T1, T2> callback)
        {
            afterListenerList.Remove(callback);
        }

        public void Clear()
        {
            listenerList.Clear();
            afterListenerList.Clear();
        }
    }
    
    public class MessageBusBroadcaster<T1, T2, T3> : IMessageBusBroadcaster
    {
        List<Action<T1, T2, T3>> listenerList = new List<Action<T1, T2, T3>>();
        List<Action<T1, T2, T3>> afterListenerList = new List<Action<T1, T2, T3>>();
        
        public void Broadcast(T1 value1, T2 value2, T3 value3)
        {
            foreach (var listener in listenerList)
            {
                listener(value1, value2, value3);
            }

            foreach (var afterListener in afterListenerList)
            {
                afterListener(value1, value2, value3);
            }
        }

        public void AddListener(Action<T1, T2, T3> callback)
        {
            listenerList.Add(callback);
        }

        public void RemoveListener(Action<T1, T2, T3> callback)
        {
            listenerList.Remove(callback);
        }

        public void AddAfterListener(Action<T1, T2, T3> callback)
        {
            afterListenerList.Add(callback);
        }

        public void RemoveAfterListener(Action<T1, T2, T3> callback)
        {
            afterListenerList.Remove(callback);
        }

        public void Clear()
        {
            listenerList.Clear();
            afterListenerList.Clear();
        }
    }
    
    public class MessageBusBroadcaster<T1, T2, T3, T4> : IMessageBusBroadcaster
    {
        List<Action<T1, T2, T3, T4>> listenerList = new List<Action<T1, T2, T3, T4>>();
        List<Action<T1, T2, T3, T4>> afterListenerList = new List<Action<T1, T2, T3, T4>>();
        
        public void Broadcast(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            foreach (var listener in listenerList)
            {
                listener(value1, value2, value3, value4);
            }

            foreach (var afterListener in afterListenerList)
            {
                afterListener(value1, value2, value3, value4);
            }
        }

        public void AddListener(Action<T1, T2, T3, T4> callback)
        {
            listenerList.Add(callback);
        }

        public void RemoveListener(Action<T1, T2, T3, T4> callback)
        {
            listenerList.Remove(callback);
        }

        public void AddAfterListener(Action<T1, T2, T3, T4> callback)
        {
            afterListenerList.Add(callback);
        }

        public void RemoveAfterListener(Action<T1, T2, T3, T4> callback)
        {
            afterListenerList.Remove(callback);
        }

        public void Clear()
        {
            listenerList.Clear();
            afterListenerList.Clear();
        }
    }
    
    public class MessageBusBroadcaster<T1, T2, T3, T4, T5> : IMessageBusBroadcaster
    {
        List<Action<T1, T2, T3, T4, T5>> listenerList = new List<Action<T1, T2, T3, T4, T5>>();
        List<Action<T1, T2, T3, T4, T5>> afterListenerList = new List<Action<T1, T2, T3, T4, T5>>();
        
        public void Broadcast(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            foreach (var listener in listenerList)
            {
                listener(value1, value2, value3, value4, value5);
            }

            foreach (var afterListener in afterListenerList)
            {
                afterListener(value1, value2, value3, value4, value5);
            }
        }

        public void AddListener(Action<T1, T2, T3, T4, T5> callback)
        {
            listenerList.Add(callback);
        }

        public void RemoveListener(Action<T1, T2, T3, T4, T5> callback)
        {
            listenerList.Remove(callback);
        }

        public void AddAfterListener(Action<T1, T2, T3, T4, T5> callback)
        {
            afterListenerList.Add(callback);
        }

        public void RemoveAfterListener(Action<T1, T2, T3, T4, T5> callback)
        {
            afterListenerList.Remove(callback);
        }

        public void Clear()
        {
            listenerList.Clear();
            afterListenerList.Clear();
        }
    }
}