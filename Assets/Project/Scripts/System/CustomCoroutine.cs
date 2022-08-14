using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ParallelCoroutine : IEnumerator
{
    public void Reset()
    {
        throw new NotSupportedException();
    }

    List<FlattenCoroutine> coroutines;

    public object Current => null;

    public ParallelCoroutine(params IEnumerator[] targets)
    {
        coroutines = targets.Select(x => new FlattenCoroutine(x)).ToList();
    }

    public ParallelCoroutine(ICollection<IEnumerator> targets)
    {
        coroutines = targets.Select(x => new FlattenCoroutine(x)).ToList();
    }

    public bool MoveNext()
    {
        for (var i = 0; i < coroutines.Count; i++)
        {
            if (!coroutines[i].MoveNext())
            {
                coroutines.RemoveAt(i);
                --i;
            }
        }

        return coroutines.Count > 0;
    }
}

sealed class FlattenCoroutine : IEnumerator
{
    readonly IEnumerator main;
    IEnumerator yieldInstructionCoroutine;

    public FlattenCoroutine(IEnumerator coroutine)
    {
        main = coroutine.Flatten();
    }

    public object Current => yieldInstructionCoroutine == null ? main.Current : yieldInstructionCoroutine.Current;

    public bool MoveNext()
    {
        if (yieldInstructionCoroutine != null)
        {
            return true;
        }

        var result = main.MoveNext();
        if (result && main.Current is YieldInstruction)
        {
            yieldInstructionCoroutine = WaitYieldInstruction(main.Current as YieldInstruction);
            Scheduler.RunCoroutine(yieldInstructionCoroutine);
        }

        return result;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    IEnumerator WaitYieldInstruction(YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
        yieldInstructionCoroutine = null;
    }
}

public static class CoroutineExtensions
{
    public static IEnumerator Flatten(this IEnumerator coroutine)
    {
        var stack = new Stack<IEnumerator>();
        stack.Push(coroutine);
        while (stack.Count > 0)
        {
            var itr = stack.Peek();
            if (!itr.MoveNext())
            {
                stack.Pop();
            }
            else
            {
                var current = itr.Current;
                if (current == null)
                {
                    yield return null;
                }
                else if (current is YieldInstruction)
                {
                    yield return current;
                }
                else if (current is IEnumerator)
                {
                    stack.Push(current as IEnumerator);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}