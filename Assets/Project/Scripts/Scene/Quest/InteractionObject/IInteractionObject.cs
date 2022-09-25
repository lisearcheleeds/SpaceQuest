using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IInteractionObject
    {
        Transform transform { get; }
        InteractionType InteractionType { get; }
        IInteractData InteractData { get; }
    }
}
