using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Player.SetAreaId.AddListener(PlayerCommandSetAreaId);
            MessageBus.Instance.Player.SetMoveTarget.AddListener(PlayerCommandSetMoveTarget);

            MessageBus.Instance.Actor.AddInteractOrder.AddListener(PlayerCommandAddInteractOrder);
            MessageBus.Instance.Actor.RemoveInteractOrder.AddListener(PlayerCommandRemoveInteractOrder);
            
            MessageBus.Instance.Actor.SetWeaponExecute.AddListener(ActorCommandSetWeaponExecute);
            MessageBus.Instance.Actor.ReloadWeapon.AddListener(ActorCommandReloadWeapon);
            MessageBus.Instance.Actor.SetCurrentWeaponGroupIndex.AddListener(ActorCommandSetCurrentWeaponGroupIndex);

            MessageBus.Instance.Actor.ForwardBoosterPowerRatio.AddListener(ActorCommandForwardBoosterPowerRatio);
            MessageBus.Instance.Actor.BackBoosterPowerRatio.AddListener(ActorCommandBackBoosterPowerRatio);
            MessageBus.Instance.Actor.RightBoosterPowerRatio.AddListener(ActorCommandRightBoosterPowerRatio);
            MessageBus.Instance.Actor.LeftBoosterPowerRatio.AddListener(ActorCommandLeftBoosterPowerRatio);
            MessageBus.Instance.Actor.TopBoosterPowerRatio.AddListener(ActorCommandTopBoosterPowerRatio);
            MessageBus.Instance.Actor.BottomBoosterPowerRatio.AddListener(ActorCommandBottomBoosterPowerRatio);
            MessageBus.Instance.Actor.PitchBoosterPowerRatio.AddListener(ActorCommandPitchBoosterPowerRatio);
            MessageBus.Instance.Actor.RollBoosterPowerRatio.AddListener(ActorCommandRollBoosterPowerRatio);
            MessageBus.Instance.Actor.YawBoosterPowerRatio.AddListener(ActorCommandYawBoosterPowerRatio);

            MessageBus.Instance.Actor.SetLookAtDirection.AddListener(ActorCommandSetLookAtDirection);

            MessageBus.Instance.Actor.SetMainTarget.AddListener(ActorCommandSetMainTarget);

            MessageBus.Instance.Temp.NoticeDamageEventData.AddListener(NoticeDamageEventData);
            MessageBus.Instance.Temp.NoticeBrokenActorEventData.AddListener(NoticeBrokenActorEventData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Player.SetAreaId.RemoveListener(PlayerCommandSetAreaId);
            MessageBus.Instance.Player.SetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);

            MessageBus.Instance.Actor.AddInteractOrder.RemoveListener(PlayerCommandAddInteractOrder);
            MessageBus.Instance.Actor.RemoveInteractOrder.RemoveListener(PlayerCommandRemoveInteractOrder);
            
            MessageBus.Instance.Actor.SetWeaponExecute.RemoveListener(ActorCommandSetWeaponExecute);
            MessageBus.Instance.Actor.ReloadWeapon.RemoveListener(ActorCommandReloadWeapon);
            MessageBus.Instance.Actor.SetCurrentWeaponGroupIndex.RemoveListener(ActorCommandSetCurrentWeaponGroupIndex);

            MessageBus.Instance.Actor.ForwardBoosterPowerRatio.RemoveListener(ActorCommandForwardBoosterPowerRatio);
            MessageBus.Instance.Actor.BackBoosterPowerRatio.RemoveListener(ActorCommandBackBoosterPowerRatio);
            MessageBus.Instance.Actor.RightBoosterPowerRatio.RemoveListener(ActorCommandRightBoosterPowerRatio);
            MessageBus.Instance.Actor.LeftBoosterPowerRatio.RemoveListener(ActorCommandLeftBoosterPowerRatio);
            MessageBus.Instance.Actor.TopBoosterPowerRatio.RemoveListener(ActorCommandTopBoosterPowerRatio);
            MessageBus.Instance.Actor.BottomBoosterPowerRatio.RemoveListener(ActorCommandBottomBoosterPowerRatio);
            MessageBus.Instance.Actor.PitchBoosterPowerRatio.RemoveListener(ActorCommandPitchBoosterPowerRatio);
            MessageBus.Instance.Actor.RollBoosterPowerRatio.RemoveListener(ActorCommandRollBoosterPowerRatio);
            MessageBus.Instance.Actor.YawBoosterPowerRatio.RemoveListener(ActorCommandYawBoosterPowerRatio);

            MessageBus.Instance.Actor.SetLookAtDirection.RemoveListener(ActorCommandSetLookAtDirection);

            MessageBus.Instance.Actor.SetMainTarget.RemoveListener(ActorCommandSetMainTarget);

            MessageBus.Instance.Temp.NoticeDamageEventData.RemoveListener(NoticeDamageEventData);
            MessageBus.Instance.Temp.NoticeBrokenActorEventData.RemoveListener(NoticeBrokenActorEventData);
        }

        void PlayerCommandAddInteractOrder(Guid actorId, IInteractData interactData)
        {
            var targetActorData = questData.ActorData[actorId];
            if (targetActorData.ActorStateData.InteractOrderStateList.Any(x => x.InteractData.InstanceId == interactData.InstanceId))
            {
                return;
            }

            var state = new InteractOrderState(interactData);
            targetActorData.AddInteractOrder(state);
            
            MessageBus.Instance.Actor.OnAddInteractOrder.Broadcast(actorId, state);
        }

        void PlayerCommandRemoveInteractOrder(Guid actorId, IInteractData interactData)
        {
            var targetActorData = questData.ActorData[actorId];
            var state = targetActorData.ActorStateData.InteractOrderStateList.FirstOrDefault(x => x.InteractData.InstanceId == interactData.InstanceId);
            if (state == null)
            {
                return;
            }

            targetActorData.RemoveInteractOrder(state);
            MessageBus.Instance.Actor.OnRemoveInteractOrder.Broadcast(actorId, state);
        }

        void PlayerCommandSetAreaId(Guid actorId, int? areaId)
        {
            questData.ActorData[actorId].SetAreaId(areaId);
        }

        void PlayerCommandSetMoveTarget(Guid actorId, IPositionData moveTarget)
        {
            var targetActorData = questData.ActorData[actorId];
            if (moveTarget == null)
            {
                targetActorData.ActorStateData.IsWarping = false;
                targetActorData.ActorStateData.MoveTarget = null;
                return;
            }

            // 今どのエリアにも居ない時、もしくは移動先のエリアが違う時ワープ状態とする
            if (targetActorData.AreaId != moveTarget.AreaId)
            {
                targetActorData.ActorStateData.IsWarping = true;
            }
            else
            {
                targetActorData.ActorStateData.IsWarping = false;
            }

            targetActorData.SetMoveTarget(moveTarget);
        }

        void ActorCommandSetWeaponExecute(Guid actorId, bool isExecute)
        {
            questData.ActorData[actorId].SetWeaponExecute(isExecute);
        }

        void ActorCommandReloadWeapon(Guid actorId)
        {
            questData.ActorData[actorId].ReloadWeapon();
        }

        void ActorCommandForwardBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetForwardBoosterPowerRatio(power);
        }

        void ActorCommandBackBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetBackBoosterPowerRatio(power);
        }

        void ActorCommandRightBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetRightBoosterPowerRatio(power);
        }

        void ActorCommandLeftBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetLeftBoosterPowerRatio(power);
        }

        void ActorCommandTopBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetTopBoosterPowerRatio(power);
        }

        void ActorCommandBottomBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetBottomBoosterPowerRatio(power);
        }

        void ActorCommandPitchBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetPitchBoosterPowerRatio(power);
        }

        void ActorCommandRollBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetRollBoosterPowerRatio(power);
        }

        void ActorCommandYawBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData[actorId].SetYawBoosterPowerRatio(power);
        }

        void ActorCommandSetLookAtDirection(Guid actorId, Vector3 lookAt)
        {
            questData.ActorData[actorId].SetLookAtDirection(lookAt);
        }

        void ActorCommandSetCurrentWeaponGroupIndex(Guid actorId, int index)
        {
            questData.ActorData[actorId].SetCurrentWeaponGroupIndex(index);
        }

        void ActorCommandSetMainTarget(Guid actorId, IPositionData target)
        {
            questData.ActorData[actorId].SetMainTarget(target);
        }

        void NoticeDamageEventData(DamageEventData damageEventData)
        {
            // TODO: foreachで全部のActorに知らせたい（特殊効果のために）
            questData.ActorData[damageEventData.DamagedActorData.InstanceId].AddDamageEventData(damageEventData);
        }

        void NoticeBrokenActorEventData(BrokenActorEventData brokenActorEventData)
        {
            MessageBus.Instance.Data.ReleaseActorData.Broadcast(brokenActorEventData.BrokenActorData);
        }
    }
}
