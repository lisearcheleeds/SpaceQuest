using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IInteractionObject
    {
        Transform transform { get; }
        IInteractData InteractData { get; }
    }
}
