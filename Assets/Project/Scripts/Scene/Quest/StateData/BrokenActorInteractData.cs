using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BrokenActorInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;

        public Guid InstanceId { get; }

        public int AreaIndex { get; }
        public Vector3 Position { get; private set; }
        public string Text => ActorData.InstanceId.ToString();
        public float InteractTime => 3.0f;
        
        public ActorData ActorData { get; }

        public BrokenActorInteractData(ActorData actorData, Vector3 position)
        {
            InstanceId = Guid.NewGuid();

            AreaIndex = actorData.AreaIndex;
            ActorData = actorData;
            Position = position;
        }
        
        public Vector3 GetClosestPoint(IPosition position)
        {
            return Position;
        }

        public bool IsInteractionRange(IPosition position)
        {
            return (position.Position - Position).magnitude < InteractionRange;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }
    }
}