using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public interface IInteractionObject
    {
        Transform transform { get; }

        string Text { get; }

        InteractionType InteractionType { get; }

        IInteractData InteractData { get; }
    }
}
