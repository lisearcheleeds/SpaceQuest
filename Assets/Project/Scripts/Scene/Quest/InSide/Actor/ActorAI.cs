using System;
using System.Linq;
using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class ActorAI
    {
        public ActorAIHandler ActorAIHandler => actorAIHandler;

        ActorAIHandler actorAIHandler;
        
        ActorAIState? currentActorAIState;
        ActorAIState? interruptActorAIState;
        IActorAIState[] actorAIStatePattern;
        
        public ActorAI(IActor actor, ActorData actorData, ICollision actorCollision)
        {
            actorAIHandler = new ActorAIHandler(actor, actorData, actorCollision);

            // FIXME: パターンをActorDataから引く
            actorAIStatePattern = new IActorAIState[]
            {
                new ActorAICheck(),
                new ActorAIIdle(),
                new ActorAIFight(),
                new ActorAIInteract(),
                new ActorAIBeginMove(),
                new ActorAIMoving(),
            };
            
            MessageBus.Instance.PlayerCommandAddInteractItemOrder.AddListener(AddInteractItemOrder);
            MessageBus.Instance.PlayerCommandRemoveInteractItemOrder.AddListener(RemoveInteractItemOrder);
            
            MessageBus.Instance.PlayerCommandSetDestinateAreaIndex.AddListener(PlayerCommandSetDestinateAreaIndex);

            MessageBus.Instance.SubscribeUpdateTargetList.AddListener(SubscribeUpdateTargetList);
            MessageBus.Instance.SubscribeUpdateInteractionObjectList.AddListener(SubscribeUpdateInteractionObjectList);
            
            MessageBus.Instance.NoticeHitCollision.AddListener(NoticeHitCollision);
            MessageBus.Instance.NoticeHitThreat.AddListener(NoticeHitThreat);
        }

        public void OnStart()
        {
            actorAIHandler.ActorPathFinder = new ActorPathFinder();
            currentActorAIState = ActorAIState.Check;
        }

        public void OnDestroy()
        {
            MessageBus.Instance.PlayerCommandAddInteractItemOrder.RemoveListener(AddInteractItemOrder);
            MessageBus.Instance.PlayerCommandRemoveInteractItemOrder.RemoveListener(RemoveInteractItemOrder);
            
            MessageBus.Instance.PlayerCommandSetDestinateAreaIndex.RemoveListener(PlayerCommandSetDestinateAreaIndex);

            MessageBus.Instance.SubscribeUpdateTargetList.RemoveListener(SubscribeUpdateTargetList);
            MessageBus.Instance.SubscribeUpdateInteractionObjectList.RemoveListener(SubscribeUpdateInteractionObjectList);
            
            MessageBus.Instance.NoticeHitCollision.RemoveListener(NoticeHitCollision);
            MessageBus.Instance.NoticeHitThreat.RemoveListener(NoticeHitThreat);
        }

        public void Spawn()
        {
            UpdateRoute();
        }

        public void Update(float deltaTime)
        {
            if (actorAIHandler.ActorData.ActorState == ActorState.Broken)
            {
                return;
            }

            if (!actorAIHandler.Actor.ActorData.IsAlive || !currentActorAIState.HasValue)
            {
                return;
            }

            if (interruptActorAIState.HasValue)
            {
                currentActorAIState = interruptActorAIState;
                interruptActorAIState = null;
            }

            if (actorAIHandler.MainTarget != null && !actorAIHandler.MainTarget.TargetData.IsTargetable)
            {
                actorAIHandler.MainTarget = null;
            }

            var nextActorAIState = actorAIStatePattern.First(x => x.ActorAIState == currentActorAIState).Update(actorAIHandler);
            
            if (actorAIStatePattern.Any(x => x.ActorAIState ==  nextActorAIState))
            {
                currentActorAIState = nextActorAIState;
            }
            else
            {
                currentActorAIState = ActorAIState.Check;
            }
            
            actorAIHandler.HitThreatList.Clear();
        }

        void SubscribeUpdateTargetList(ITarget[] targets)
        {
            actorAIHandler.Targets = targets;

            interruptActorAIState = ActorAIState.Check;
        }

        void NoticeHitCollision(ICollision collision1, ICollision collision2)
        {
            ICollision otherCollision = null;

            if (collision1 == actorAIHandler.ActorCollision)
            {
                otherCollision = collision2;
            }
            
            if (collision2 == actorAIHandler.ActorCollision)
            {
                otherCollision = collision1;
            }

            if (otherCollision == null)
            {
                return;
            }

            var causeDamage = otherCollision as ICauseDamage;
            if (causeDamage == null)
            {
                return;
            }

            if (actorAIHandler.ActorData.InstanceId != otherCollision.PlayerInstanceId)
            {
                MessageBus.Instance.NoticeDamage.Broadcast(causeDamage.WeaponData, causeDamage.ResourceItemVO, actorAIHandler.ActorData);
            }
        }

        void NoticeHitThreat(IThreat threat, ICollision collision)
        {
            if (collision != actorAIHandler.ActorCollision)
            {
                return;
            }

            actorAIHandler.HitThreatList.Add(threat);
        }

        void SubscribeUpdateInteractionObjectList(IInteractionObject[] interacts)
        {
            actorAIHandler.InteractionObjects = interacts;
            UpdateRoute();
        }

        void AddInteractItemOrder(ActorData orderActor, ItemObject itemObject)
        {
            if (orderActor.InstanceId != actorAIHandler.ActorData.InstanceId)
            {
                return;
            }

            actorAIHandler.ActorData.InteractOrder.Add(itemObject.InteractData);
            interruptActorAIState = ActorAIState.Check;
        }

        void RemoveInteractItemOrder(ActorData orderActor, ItemObject itemObject)
        {
            if (orderActor.InstanceId != actorAIHandler.ActorData.InstanceId)
            {
                return;
            }

            actorAIHandler.ActorData.InteractOrder.Remove(itemObject.InteractData);
            interruptActorAIState = ActorAIState.Check;
        }

        void PlayerCommandSetDestinateAreaIndex(Guid playerInstanceId, int? areaIndex)
        {
            if (actorAIHandler.ActorData.PlayerQuestDataInstanceId != playerInstanceId)
            {
                return;
            }

            actorAIHandler.ActorData.SetDestinateAreaIndex(areaIndex);
            UpdateRoute();
        }

        void UpdateRoute()
        {
            // エリア移動周り
            var routeAreas = actorAIHandler.ActorData.GetRouteAreaData().ToArray();
            var firstDirection = routeAreas.FirstOrDefault()?.Direction;
            if (routeAreas.Length == 0 || !firstDirection.HasValue)
            {
                return;
            }

            var nearestAreaDirectionPosition = actorAIHandler.ActorData.GetNearestAreaDirectionPosition(firstDirection.Value);
            actorAIHandler.ActorPathFinder.CalculatePath(actorAIHandler.ActorData.Position, nearestAreaDirectionPosition);

            currentActorAIState = ActorAIState.Check;
        }
    }
}
