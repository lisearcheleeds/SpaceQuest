using System;
using System.Collections.Generic;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAICache
    {
        public Guid PlayerInstanceId { get; private set; }
        public Guid ActorInstanceId { get; private set; }

        public ActorAIState ActorAIState { get; set; }

        public IPosition MoveTarget { get; set; }

        public IInteractData InteractOrder { get; set; }
        public float InteractingTime { get; set; }

        public List<IThreatData> ThreatList { get; } = new List<IThreatData>();
        public ITargetData[] AroundTargets { get; private set; } = Array.Empty<ITargetData>();

        public ITargetData MainTarget { get; set; }

        public void Initialize(Guid playerInstanceId, Guid actorInstanceId)
        {
            PlayerInstanceId = playerInstanceId;
            ActorInstanceId = actorInstanceId;
            
            MessageBus.Instance.PlayerCommandSetInteractOrder.AddListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetMoveTarget.AddListener(PlayerCommandSetMoveTarget);
            MessageBus.Instance.NoticeHitThreat.AddListener(NoticeHitThreat);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetInteractOrder.RemoveListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);
            MessageBus.Instance.NoticeHitThreat.RemoveListener(NoticeHitThreat);
        }

        void PlayerCommandSetInteractOrder(ActorData orderActor, IInteractData interactData)
        {
            if (orderActor.InstanceId == ActorInstanceId)
            {
                InteractOrder = interactData;
            }
        }

        void PlayerCommandSetMoveTarget(Guid playerInstanceId, IPosition moveTarget)
        {
            if (playerInstanceId == PlayerInstanceId)
            {
                MoveTarget = moveTarget;
            }
        }

        void NoticeHitThreat(IThreatData threatData, ICollisionData collisionData)
        {
            if ((collisionData as ActorData)?.InstanceId == ActorInstanceId)
            {
                ThreatList.Add(threatData);
            }
        }
    }
}