using System;

namespace AloneSpace
{
    public interface IMessageBusUnicaster
    {
    }

    public class MessageBusUnicaster<TResult> : IMessageBusUnicaster
    {
        Func<TResult> listener;
        
        public TResult Unicast()
        {
            return listener();
        }

        public void SetListener(Func<TResult> callback)
        {
            listener = callback;
        }

        public void Clear()
        {
            listener = null;
        }
    }
    
    public class MessageBusUnicaster<T, TResult> : IMessageBusUnicaster
    {
        Func<T, TResult> listener;
        
        public TResult Unicast(T value)
        {
            return listener(value);
        }

        public void SetListener(Func<T, TResult> callback)
        {
            listener = callback;
        }

        public void Clear()
        {
            listener = null;
        }
    }
    
    public class MessageBusUnicaster<T1, T2, TResult> : IMessageBusUnicaster
    {
        Func<T1, T2, TResult> listener;
        
        public TResult Unicast(T1 value1, T2 value2)
        {
            return listener(value1, value2);
        }

        public void SetListener(Func<T1, T2, TResult> callback)
        {
            listener = callback;
        }

        public void Clear()
        {
            listener = null;
        }
    }
    
    public class MessageBusUnicaster<T1, T2, T3, TResult> : IMessageBusUnicaster
    {
        Func<T1, T2, T3, TResult> listener;
        
        public TResult Unicast(T1 value1, T2 value2, T3 value3)
        {
            return listener(value1, value2, value3);
        }

        public void SetListener(Func<T1, T2, T3, TResult> callback)
        {
            listener = callback;
        }

        public void Clear()
        {
            listener = null;
        }
    }
    
    public class MessageBusUnicaster<T1, T2, T3, T4, TResult> : IMessageBusUnicaster
    {
        Func<T1, T2, T3, T4, TResult> listener;
        
        public TResult Unicast(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            return listener(value1, value2, value3, value4);
        }

        public void SetListener(Func<T1, T2, T3, T4, TResult> callback)
        {
            listener = callback;
        }

        public void Clear()
        {
            listener = null;
        }
    }
}