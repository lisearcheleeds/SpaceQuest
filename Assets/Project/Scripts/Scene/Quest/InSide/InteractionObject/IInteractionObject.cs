using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public interface IInteractionObject
    {
        Transform transform { get; }

        string Text { get; }

        InteractionType InteractionType { get; }

        IInteractData InteractData { get; }
    }
}
