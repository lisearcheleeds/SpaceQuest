using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BrokenActorInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;

        public Guid InstanceId { get; }

        public int? AreaId { get; }
        
        public Vector3 Position { get; private set; }
        public string Text => ActorData.InstanceId.ToString();
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.NearPosition;
        
        public ActorData ActorData { get; }

        public BrokenActorInteractData(ActorData actorData)
        {
            InstanceId = Guid.NewGuid();

            AreaId = actorData.AreaId;
            ActorData = actorData;
            Position = actorData.Position;
        }
        
        public Vector3 GetClosestPoint(IPositionData positionData)
        {
            return Position;
        }

        public bool IsInteractionRange(IPositionData positionData)
        {
            return (positionData.Position - Position).magnitude < InteractionRange;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }
    }
}