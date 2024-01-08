using System;
using System.Linq;

namespace AloneSpace
{
    public class GraphicEffectMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Actor.OnAddInteractOrder.AddListener(OnAddInteractOrder);
            MessageBus.Instance.Actor.OnRemoveInteractOrder.AddListener(OnRemoveInteractOrder);
            
            MessageBus.Instance.Temp.NoticeDamageEventData.AddListener(NoticeDamageEventData);
            MessageBus.Instance.Temp.NoticeBrokenActorEventData.AddListener(NoticeBrokenActorEventData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Actor.OnAddInteractOrder.RemoveListener(OnAddInteractOrder);
            MessageBus.Instance.Actor.OnRemoveInteractOrder.RemoveListener(OnRemoveInteractOrder);
            
            MessageBus.Instance.Temp.NoticeDamageEventData.RemoveListener(NoticeDamageEventData);
            MessageBus.Instance.Temp.NoticeBrokenActorEventData.RemoveListener(NoticeBrokenActorEventData);
        }

        void OnAddInteractOrder(Guid actorId, InteractOrderState interactOrderState)
        {
            if (interactOrderState.PullItemGraphicEffectHandler == null)
            {
                interactOrderState.PullItemGraphicEffectHandler = new PullItemGraphicEffectHandler(questData.ActorData[actorId], interactOrderState.InteractData);
                MessageBus.Instance.Data.SpawnGraphicEffect.Broadcast(new GraphicEffectSpecVO(30001), interactOrderState.PullItemGraphicEffectHandler);
            }
        }

        void OnRemoveInteractOrder(Guid actorId, InteractOrderState interactOrderState)
        {
            interactOrderState.PullItemGraphicEffectHandler.Abandon();
            interactOrderState.PullItemGraphicEffectHandler = null;
        }

        void NoticeDamageEventData(DamageEventData damageEventData)
        {
        }

        void NoticeBrokenActorEventData(BrokenActorEventData brokenActorEventData)
        {
            MessageBus.Instance.Data.SpawnGraphicEffect.Broadcast(
                brokenActorEventData.BrokenActorData.ActorSpecVO.BrokenActorGraphicEffectSpecVO,
                new BrokenActorGraphicEffectHandler(
                    brokenActorEventData.BrokenActorData,
                    brokenActorEventData.BrokenActorData.ActorSpecVO.BrokenActorSmokeGraphicEffectSpecVO,
                    brokenActorEventData.BrokenActorData.MovingModule.MovementVelocity));
        }
    }
}
