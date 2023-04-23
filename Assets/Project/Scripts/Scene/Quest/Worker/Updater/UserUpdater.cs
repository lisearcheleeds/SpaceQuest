﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace AloneSpace
{
    public class UserUpdater : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] GameObjectUpdater gameObjectUpdater;
        [SerializeField] AreaAmbientController areaAmbientController;
        [SerializeField] CameraController cameraController;

        UserData userData;
        
        ActorMode prevObserveActorMode;
        
        public void Initialize(QuestData questData)
        {
            userData = questData.UserData;
            
            uiManager.Initialize(questData);
            gameObjectUpdater.Initialize(questData);
            
            areaAmbientController.Initialize(questData);
            cameraController.Initialize();
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.AddListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.AddListener(ManagerCommandLoadArea);
            
            MessageBus.Instance.UserCommandSetLookAtAngle.AddListener(UserCommandLookAtAngle);
            MessageBus.Instance.UserCommandSetLookAtSpace.AddListener(UserCommandSetLookAtSpace);
            
            MessageBus.Instance.UserInputSetExecuteWeapon.AddListener(UserInputSetExecuteWeapon);
            MessageBus.Instance.UserInputReloadWeapon.AddListener(UserInputReloadWeapon);
            
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.AddListener(UserInputForwardBoosterPowerRatio);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.AddListener(UserInputBackBoosterPowerRatio);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.AddListener(UserInputRightBoosterPowerRatio);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.AddListener(UserInputLeftBoosterPowerRatio);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.AddListener(UserInputTopBoosterPowerRatio);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.AddListener(UserInputBottomBoosterPowerRatio);
            MessageBus.Instance.UserInputPitchBoosterPowerRatio.AddListener(UserInputPitchBoosterPowerRatio);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.AddListener(UserInputRollBoosterPowerRatio);
            MessageBus.Instance.UserInputYawBoosterPowerRatio.AddListener(UserInputYawBoosterPowerRatio);
            
            MessageBus.Instance.UserInputSwitchActorMode.AddListener(UserInputSwitchActorMode);
            MessageBus.Instance.UserInputSetActorCombatMode.AddListener(UserInputSetActorCombatMode);
        }

        public void Finalize()
        {
            uiManager.Finalize();
            gameObjectUpdater.Finalize();
            areaAmbientController.Finalize();
            cameraController.Finalize();
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.RemoveListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.RemoveListener(ManagerCommandLoadArea);
            
            MessageBus.Instance.UserCommandSetLookAtAngle.RemoveListener(UserCommandLookAtAngle);
            MessageBus.Instance.UserCommandSetLookAtSpace.RemoveListener(UserCommandSetLookAtSpace);
            
            MessageBus.Instance.UserInputSetExecuteWeapon.RemoveListener(UserInputSetExecuteWeapon);
            MessageBus.Instance.UserInputReloadWeapon.RemoveListener(UserInputReloadWeapon);
            
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.RemoveListener(UserInputForwardBoosterPowerRatio);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.RemoveListener(UserInputBackBoosterPowerRatio);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.RemoveListener(UserInputRightBoosterPowerRatio);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.RemoveListener(UserInputLeftBoosterPowerRatio);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.RemoveListener(UserInputTopBoosterPowerRatio);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.RemoveListener(UserInputBottomBoosterPowerRatio);
            MessageBus.Instance.UserInputPitchBoosterPowerRatio.RemoveListener(UserInputPitchBoosterPowerRatio);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.RemoveListener(UserInputRollBoosterPowerRatio);
            MessageBus.Instance.UserInputYawBoosterPowerRatio.RemoveListener(UserInputYawBoosterPowerRatio);
            
            MessageBus.Instance.UserInputSwitchActorMode.RemoveListener(UserInputSwitchActorMode);
            MessageBus.Instance.UserInputSetActorCombatMode.RemoveListener(UserInputSetActorCombatMode);
        }
        
        public void OnLateUpdate()
        {
            if (userData?.PlayerQuestData == null)
            {
                return;
            }

            uiManager.OnLateUpdate();
            gameObjectUpdater.OnLateUpdate();
            areaAmbientController.OnLateUpdate();
            cameraController.OnLateUpdate(userData);

            if (userData.PlayerQuestData.MainActorData.ActorMode == ActorMode.Warp)
            {
                if (userData.PlayerQuestData.MainActorData.AreaId != userData.CurrentAreaData?.AreaId)
                {
                    MessageBus.Instance.ManagerCommandLoadArea.Broadcast(userData.PlayerQuestData.MainActorData.AreaId);
                }
            }

            if (prevObserveActorMode != userData.PlayerQuestData.MainActorData.ActorMode)
            {
                if (userData.PlayerQuestData.MainActorData.ActorMode == ActorMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Cockpit);
                }
                else if(userData.PlayerQuestData.MainActorData.ActorMode != ActorMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Default);
                }
            }

            prevObserveActorMode = userData.PlayerQuestData.MainActorData.ActorMode;
        }
        
        void ManagerCommandSetObservePlayer(Guid playerInstanceId)
        {
            userData.SetPlayerQuestData(MessageBus.Instance.UtilGetPlayerQuestData.Unicast(playerInstanceId));
            
            uiManager.SetObservePlayerQuestData(userData);
            gameObjectUpdater.SetObservePlayerQuestData(userData.PlayerQuestData);
            
            MessageBus.Instance.ManagerCommandLoadArea.Broadcast(userData.PlayerQuestData.MainActorData.AreaId);
            MessageBus.Instance.UserCommandSetCameraTrackTarget.Broadcast(userData.PlayerQuestData.MainActorData);
        }

        void ManagerCommandLoadArea(int? areaId)
        {
            userData.SetCurrentAreaData(areaId.HasValue ? MessageBus.Instance.UtilGetAreaData.Unicast(areaId.Value) : null);
            
            uiManager.SetObserveAreaData(userData.CurrentAreaData);
            gameObjectUpdater.SetObserveAreaData(userData.CurrentAreaData);
            areaAmbientController.SetObserveAreaData(userData.CurrentAreaData);
        }

        void UserCommandLookAtAngle(Vector3 lookAt)
        {
            userData.SetLookAtAngle(lookAt);
        }
        
        void UserCommandSetLookAtSpace(Quaternion quaternion)
        {
            userData.SetLookAtSpace(quaternion);
        }

        void UserInputSetExecuteWeapon(bool isExecute)
        {
            MessageBus.Instance.ActorCommandSetWeaponExecute.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, isExecute);
        }

        void UserInputReloadWeapon()
        {
            MessageBus.Instance.ActorCommandReloadWeapon.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId);
        }

        void UserInputForwardBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputBackBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputRightBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputLeftBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputTopBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputBottomBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputPitchBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputRollBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputYawBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, power);
        }

        void UserInputSwitchActorMode()
        {
            if (userData.PlayerQuestData.MainActorData.ActorMode == ActorMode.ThirdPersonViewpoint)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, ActorMode.Cockpit);
            }
            else if (userData.PlayerQuestData.MainActorData.ActorMode == ActorMode.Cockpit)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, ActorMode.ThirdPersonViewpoint);
            }
        }

        void UserInputSetActorCombatMode(ActorCombatMode actorCombatMode)
        {
            MessageBus.Instance.ActorCommandSetActorCombatMode.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, actorCombatMode);
        }
    }
}