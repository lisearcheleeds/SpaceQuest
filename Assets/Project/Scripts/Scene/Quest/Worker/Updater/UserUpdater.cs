using System;
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

            uiManager.Initialize(questData, userData);
            gameObjectUpdater.Initialize(questData);

            areaAmbientController.Initialize(questData);
            cameraController.Initialize();

            MessageBus.Instance.SetOrderUserPlayer.AddListener(SetOrderUserPlayer);
            MessageBus.Instance.SetOrderUserArea.AddListener(SetOrderUserArea);

            MessageBus.Instance.UserCommandSetLookAtAngle.AddListener(UserCommandLookAtAngle);
            MessageBus.Instance.UserCommandSetLookAtSpace.AddListener(UserCommandSetLookAtSpace);
            MessageBus.Instance.UserCommandSetLookAtDistance.AddListener(UserCommandSetLookAtDistance);

            MessageBus.Instance.UserInputSetExecuteWeapon.AddListener(UserInputSetExecuteWeapon);
            MessageBus.Instance.UserInputReloadWeapon.AddListener(UserInputReloadWeapon);
            MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.AddListener(UserInputSetCurrentWeaponGroupIndex);

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

            MessageBus.Instance.SetOrderUserPlayer.RemoveListener(SetOrderUserPlayer);
            MessageBus.Instance.SetOrderUserArea.RemoveListener(SetOrderUserArea);

            MessageBus.Instance.UserCommandSetLookAtAngle.RemoveListener(UserCommandLookAtAngle);
            MessageBus.Instance.UserCommandSetLookAtSpace.RemoveListener(UserCommandSetLookAtSpace);
            MessageBus.Instance.UserCommandSetLookAtDistance.RemoveListener(UserCommandSetLookAtDistance);

            MessageBus.Instance.UserInputSetExecuteWeapon.RemoveListener(UserInputSetExecuteWeapon);
            MessageBus.Instance.UserInputReloadWeapon.RemoveListener(UserInputReloadWeapon);
            MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.RemoveListener(UserInputSetCurrentWeaponGroupIndex);

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

            if (userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode == ActorMode.Warp)
            {
                if (userData.PlayerQuestData.MainActorData.AreaId != userData.CurrentAreaData?.AreaId)
                {
                    MessageBus.Instance.SetOrderUserArea.Broadcast(userData.PlayerQuestData.MainActorData.AreaId);
                }
            }

            if (prevObserveActorMode != userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode)
            {
                if (userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode == ActorMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Cockpit);
                }
                else if(userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode != ActorMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Default);
                }
            }

            prevObserveActorMode = userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode;
        }

        void SetOrderUserPlayer(Guid playerInstanceId)
        {
            userData.SetPlayerQuestData(MessageBus.Instance.UtilGetPlayerQuestData.Unicast(playerInstanceId));
            MessageBus.Instance.SetUserPlayer.Broadcast(userData.PlayerQuestData);

            MessageBus.Instance.SetOrderUserArea.Broadcast(userData.PlayerQuestData.MainActorData.AreaId);
            MessageBus.Instance.UserCommandSetCameraTrackTarget.Broadcast(userData.PlayerQuestData.MainActorData);
        }

        void SetOrderUserArea(int? areaId)
        {
            userData.SetCurrentAreaData(areaId.HasValue ? MessageBus.Instance.UtilGetAreaData.Unicast(areaId.Value) : null);
            MessageBus.Instance.SetUserArea.Broadcast(userData.CurrentAreaData);
        }

        void UserCommandLookAtAngle(Vector3 lookAt)
        {
            userData.SetLookAtAngle(lookAt);
        }

        void UserCommandSetLookAtSpace(Quaternion quaternion)
        {
            userData.SetLookAtSpace(quaternion);
        }

        void UserCommandSetLookAtDistance(float distance)
        {
            userData.SetLookAtDistance(distance);
        }

        void UserInputSetExecuteWeapon(bool isExecute)
        {
            MessageBus.Instance.ActorCommandSetWeaponExecute.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, isExecute);
        }

        void UserInputReloadWeapon()
        {
            MessageBus.Instance.ActorCommandReloadWeapon.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId);
        }

        void UserInputSetCurrentWeaponGroupIndex(int index)
        {
            MessageBus.Instance.ActorCommandSetCurrentWeaponGroupIndex.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, index);
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
            if (userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode == ActorMode.ThirdPersonViewpoint)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(userData.PlayerQuestData.MainActorData.InstanceId, ActorMode.Cockpit);
            }
            else if (userData.PlayerQuestData.MainActorData.ActorStateData.ActorMode == ActorMode.Cockpit)
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