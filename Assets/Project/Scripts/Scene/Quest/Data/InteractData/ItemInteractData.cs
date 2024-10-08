﻿using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ItemInteractData : IInteractData
    {
        static readonly float InteractionRange = 30.0f;

        public Guid InstanceId { get; }

        public MovingModule MovingModule { get; private set; }

        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public string Text => ItemData.ItemVO.Name;
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.Angle;

        public ItemData ItemData { get; }

        public ItemInteractData(ItemData itemData, int areaId, Vector3 position, Quaternion rotation)
        {
            InstanceId = Guid.NewGuid();

            MovingModule = new MovingModule(this);

            ItemData = itemData;
            AreaId = areaId;
            Position = position;
            Rotation = rotation;
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

        public virtual void ActivateModules()
        {
            MovingModule.ActivateModule();
        }

        public virtual void DeactivateModules()
        {
            MovingModule.DeactivateModule();

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
            MovingModule = null;
        }
    }
}
