using System;
using UnityEngine;

namespace AloneSpace
{
    public class ActorMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.PlayerCommandSetInteractOrder.AddListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.AddListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.AddListener(PlayerCommandSetMoveTarget);

            MessageBus.Instance.ActorCommandSetWeaponExecute.AddListener(ActorCommandSetWeaponExecute);
            MessageBus.Instance.ActorCommandReloadWeapon.AddListener(ActorCommandReloadWeapon);
            MessageBus.Instance.ActorCommandSetCurrentWeaponGroupIndex.AddListener(ActorCommandSetCurrentWeaponGroupIndex);

            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.AddListener(ActorCommandForwardBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.AddListener(ActorCommandBackBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.AddListener(ActorCommandRightBoosterPowerRatio);
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.AddListener(ActorCommandLeftBoosterPowerRatio);
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.AddListener(ActorCommandTopBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.AddListener(ActorCommandBottomBoosterPowerRatio);
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.AddListener(ActorCommandPitchBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.AddListener(ActorCommandRollBoosterPowerRatio);
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.AddListener(ActorCommandYawBoosterPowerRatio);

            MessageBus.Instance.ActorCommandSetLookAtDirection.AddListener(ActorCommandSetLookAtDirection);

            MessageBus.Instance.ActorCommandSetMainTarget.AddListener(ActorCommandSetMainTarget);
            MessageBus.Instance.ActorCommandSetAroundTargets.AddListener(ActorCommandSetAroundTargets);

            MessageBus.Instance.NoticeDamageEventData.AddListener(NoticeDamageEventData);
            MessageBus.Instance.NoticeBrokenActorEventData.AddListener(NoticeBrokenActorEventData);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetInteractOrder.RemoveListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.RemoveListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);

            MessageBus.Instance.ActorCommandSetWeaponExecute.RemoveListener(ActorCommandSetWeaponExecute);
            MessageBus.Instance.ActorCommandReloadWeapon.RemoveListener(ActorCommandReloadWeapon);
            MessageBus.Instance.ActorCommandSetCurrentWeaponGroupIndex.RemoveListener(ActorCommandSetCurrentWeaponGroupIndex);

            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.RemoveListener(ActorCommandForwardBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.RemoveListener(ActorCommandBackBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.RemoveListener(ActorCommandRightBoosterPowerRatio);
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.RemoveListener(ActorCommandLeftBoosterPowerRatio);
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.RemoveListener(ActorCommandTopBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.RemoveListener(ActorCommandBottomBoosterPowerRatio);
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.RemoveListener(ActorCommandPitchBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.RemoveListener(ActorCommandRollBoosterPowerRatio);
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.RemoveListener(ActorCommandYawBoosterPowerRatio);

            MessageBus.Instance.ActorCommandSetLookAtDirection.RemoveListener(ActorCommandSetLookAtDirection);

            MessageBus.Instance.ActorCommandSetMainTarget.RemoveListener(ActorCommandSetMainTarget);
            MessageBus.Instance.ActorCommandSetAroundTargets.RemoveListener(ActorCommandSetAroundTargets);

            MessageBus.Instance.NoticeDamageEventData.RemoveListener(NoticeDamageEventData);
            MessageBus.Instance.NoticeBrokenActorEventData.RemoveListener(NoticeBrokenActorEventData);
        }

        void PlayerCommandSetInteractOrder(ActorData orderActor, IInteractData interactData)
        {
            orderActor.SetInteractOrder(interactData);
        }

        void PlayerCommandSetAreaId(ActorData orderActor, int? areaId)
        {
            orderActor.SetAreaId(areaId);
            MessageBus.Instance.SetDirtyActorObjectList.Broadcast();
        }

        void PlayerCommandSetMoveTarget(ActorData orderActor, IPositionData moveTarget)
        {
            orderActor.SetMoveTarget(moveTarget);
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

        void ActorCommandSetAroundTargets(Guid actorId, IPositionData[] targets)
        {
            questData.ActorData[actorId].SetAroundTargets(targets);
        }

        void NoticeDamageEventData(DamageEventData damageEventData)
        {
            // TODO: foreachで全部のActorに知らせたい（特殊効果のために）
            questData.ActorData[damageEventData.DamagedActorData.InstanceId].AddDamageEventData(damageEventData);
        }

        void NoticeBrokenActorEventData(BrokenActorEventData brokenActorEventData)
        {
            // TODO: foreachで全部のActorに知らせたい（特殊効果のために）
            questData.ActorData[brokenActorEventData.BrokenActorData.InstanceId].Release();

            MessageBus.Instance.SpawnGraphicEffect.Broadcast(
                brokenActorEventData.BrokenActorData.ActorSpecVO.BrokenActorGraphicEffectSpecVO,
                new BrokenActorGraphicEffectHandler(brokenActorEventData.BrokenActorData));
        }
    }
}
