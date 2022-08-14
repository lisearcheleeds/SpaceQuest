using System.Collections.Generic;

public class KeyInputController : MonoSingleton<KeyInputController>
{
    Stack<List<IKeyInputListener>> listenersStack = new Stack<List<IKeyInputListener>>();

    public void PushListener(IKeyInputListener listener)
    {
        listenersStack.Push(new List<IKeyInputListener>(new[] { listener }));
    }

    public void PopListener()
    {
        if (listenersStack.Count > 1)
        {
            listenersStack.Pop();
        }
    }

    public void AddListener(IKeyInputListener listener)
    {
        listenersStack.Peek().Add(listener);
    }

    public void RemoveListener(IKeyInputListener listener)
    {
        listenersStack.Peek().Remove(listener);
    }

    protected override void OnInitialize()
    {
        listenersStack.Push(new List<IKeyInputListener>());
    }

    void Update()
    {
        var listeners = listenersStack.Peek();

        for (var i = listeners.Count - 1; i > 0; i++)
        {
            if (listeners[i].KeyUpdate())
            {
                break;
            }
        }
    }
}