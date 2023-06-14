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

        ActorOperationMode prevActorOperationMode;

        public void Initialize(QuestData questData)
        {
            userData = questData.UserData;

            uiManager.Initialize(questData, userData);
            gameObjectUpdater.Initialize(questData);

            areaAmbientController.Initialize(questData);
            cameraController.Initialize();

            MessageBus.Instance.SetOrderUserPlayer.AddListener(SetOrderUserPlayer);
            MessageBus.Instance.SetOrderUserArea.AddListener(SetOrderUserArea);

            MessageBus.Instance.UserCommandSetActorOperationMode.AddListener(UserCommandSetActorOperationMode);

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
        }

        public void Finalize()
        {
            uiManager.Finalize();
            gameObjectUpdater.Finalize();

            areaAmbientController.Finalize();
            cameraController.Finalize();

            MessageBus.Instance.SetOrderUserPlayer.RemoveListener(SetOrderUserPlayer);
            MessageBus.Instance.SetOrderUserArea.RemoveListener(SetOrderUserArea);

            MessageBus.Instance.UserCommandSetActorOperationMode.RemoveListener(UserCommandSetActorOperationMode);

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
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (userData?.PlayerData == null)
            {
                return;
            }

            uiManager.OnLateUpdate();
            gameObjectUpdater.OnLateUpdate(deltaTime);
            areaAmbientController.OnLateUpdate();
            cameraController.OnLateUpdate(userData);

            if (userData.PlayerData.MainActorData.ActorStateData.IsWarping)
            {
                if (userData.PlayerData.MainActorData.AreaId != userData.CurrentAreaData?.AreaId)
                {
                    MessageBus.Instance.SetOrderUserArea.Broadcast(userData.PlayerData.MainActorData.AreaId);
                }
            }

            if (prevActorOperationMode != userData.ActorOperationMode)
            {
                if (userData.ActorOperationMode != ActorOperationMode.Observe)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.Cockpit);
                }
                else
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.Default);
                }
            }

            prevActorOperationMode = userData.ActorOperationMode;
        }

        void SetOrderUserPlayer(Guid playerInstanceId)
        {
            userData.SetPlayerData(MessageBus.Instance.UtilGetPlayerData.Unicast(playerInstanceId));
            MessageBus.Instance.SetUserPlayer.Broadcast(userData.PlayerData);

            MessageBus.Instance.SetOrderUserArea.Broadcast(userData.PlayerData.MainActorData.AreaId);
            MessageBus.Instance.UserCommandSetCameraTrackTarget.Broadcast(userData.PlayerData.MainActorData);
        }

        void SetOrderUserArea(int? areaId)
        {
            userData.SetCurrentAreaData(areaId.HasValue ? MessageBus.Instance.UtilGetAreaData.Unicast(areaId.Value) : null);
            MessageBus.Instance.SetUserArea.Broadcast(userData.CurrentAreaData);
        }

        void UserCommandSetActorOperationMode(ActorOperationMode actorOperationMode)
        {
            userData.SetActorOperationMode(actorOperationMode);
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
            MessageBus.Instance.ActorCommandSetWeaponExecute.Broadcast(userData.PlayerData.MainActorData.InstanceId, isExecute);
        }

        void UserInputReloadWeapon()
        {
            MessageBus.Instance.ActorCommandReloadWeapon.Broadcast(userData.PlayerData.MainActorData.InstanceId);
        }

        void UserInputSetCurrentWeaponGroupIndex(int index)
        {
            MessageBus.Instance.ActorCommandSetCurrentWeaponGroupIndex.Broadcast(userData.PlayerData.MainActorData.InstanceId, index);
        }

        void UserInputForwardBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputBackBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputRightBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputLeftBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputTopBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputBottomBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputPitchBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputRollBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }

        void UserInputYawBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.Broadcast(userData.PlayerData.MainActorData.InstanceId, power);
        }
    }
}
