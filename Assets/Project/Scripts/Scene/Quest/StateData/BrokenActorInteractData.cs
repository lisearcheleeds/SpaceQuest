using System;
using UnityEngine;

namespace AloneSpace
{
    public class BrokenActorInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;

        public Guid InstanceId { get; }

        public int AreaIndex { get; }
        public Vector3 Position { get; set; }
        public string Text => ActorData.InstanceId.ToString();
        public float InteractTime => 3.0f;
        
        public ActorData ActorData { get; }

        public BrokenActorInteractData(ActorData actorData, Vector3 position)
        {
            InstanceId = Guid.NewGuid();

            AreaIndex = actorData.CurrentAreaIndex;
            ActorData = actorData;
            Position = position;
        }
        
        public Vector3 GetClosestPoint(Vector3 position)
        {
            return Position;
        }

        public bool IsInteractionRange(Vector3 position)
        {
            return (position - Position).magnitude < InteractionRange;
        }
    }
}