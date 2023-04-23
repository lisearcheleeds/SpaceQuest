using System;
using System.Collections.Generic;
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
            
            MessageBus.Instance.PlayerCommandSetInteractOrder.AddListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.AddListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.AddListener(PlayerCommandSetMoveTarget);
            
            MessageBus.Instance.ActorCommandSetWeaponExecute.AddListener(ActorCommandSetWeaponExecute);
            MessageBus.Instance.ActorCommandReloadWeapon.AddListener(ActorCommandReloadWeapon);
            
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
            
            MessageBus.Instance.ActorCommandSetActorMode.AddListener(ActorCommandSetActorMode);
            MessageBus.Instance.ActorCommandSetActorCombatMode.AddListener(ActorCommandSetActorCombatMode);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetInteractOrder.RemoveListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.RemoveListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);
            
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
            
            MessageBus.Instance.ActorCommandSetActorMode.RemoveListener(ActorCommandSetActorMode);
            MessageBus.Instance.ActorCommandSetActorCombatMode.RemoveListener(ActorCommandSetActorCombatMode);
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
        
        void ActorCommandSetActorMode(Guid actorId, ActorMode actorMode)
        {
            questData.ActorData[actorId].SetActorMode(actorMode);
        }

        void ActorCommandSetActorCombatMode(Guid actorId, ActorCombatMode actorCombatMode)
        {
            questData.ActorData[actorId].SetActorCombatMode(actorCombatMode);
        }
    }
}