﻿using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BrokenActorInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;

        public Guid InstanceId { get; }

        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
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
            Rotation = actorData.Rotation;
        }
        
        public Vector3 GetClosestPoint(IPositionData positionData)
        {
            return Position;
        }

        public bool IsInteractionRange(IPositionData positionData)
        {
            return (positionData.Position - Position).magnitude < InteractionRange;
        }

        public void SetAreaId(int? areaId)
        {
            AreaId = areaId;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
        }
    }
}