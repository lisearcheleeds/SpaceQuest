using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class UserMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.AddListener(UserCommandSetActorOperationMode);

            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.AddListener(UserCommandLookAtAngle);
            MessageBus.Instance.UserInput.UserCommandSetLookAtSpace.AddListener(UserCommandSetLookAtSpace);
            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.AddListener(UserCommandSetLookAtDistance);
            MessageBus.Instance.UserInput.UserCommandSetSpaceMapLookAtAngle.AddListener(UserCommandSetSpaceMapLookAtAngle);
            MessageBus.Instance.UserInput.UserCommandSetSpaceMapLookAtDistance.AddListener(UserCommandSetSpaceMapLookAtDistance);

            MessageBus.Instance.UserInput.UserInputSetExecuteWeapon.AddListener(UserInputSetExecuteWeapon);
            MessageBus.Instance.UserInput.UserInputReloadWeapon.AddListener(UserInputReloadWeapon);
            MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.AddListener(UserInputSetCurrentWeaponGroupIndex);

            MessageBus.Instance.UserInput.UserInputForwardBoosterPowerRatio.AddListener(UserInputForwardBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputBackBoosterPowerRatio.AddListener(UserInputBackBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputRightBoosterPowerRatio.AddListener(UserInputRightBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputLeftBoosterPowerRatio.AddListener(UserInputLeftBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputTopBoosterPowerRatio.AddListener(UserInputTopBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputBottomBoosterPowerRatio.AddListener(UserInputBottomBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputPitchBoosterPowerRatio.AddListener(UserInputPitchBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputRollBoosterPowerRatio.AddListener(UserInputRollBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputYawBoosterPowerRatio.AddListener(UserInputYawBoosterPowerRatio);

            MessageBus.Instance.Player.SetAreaId.AddListener(PlayerCommandSetAreaId);

            MessageBus.Instance.User.SetPlayer.AddListener(SetUserPlayer);
            MessageBus.Instance.User.SetControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.User.SetObserveTarget.AddListener(SetUserObserveTarget);

            MessageBus.Instance.User.SetObserveArea.AddListener(SetUserObserveArea);

            MessageBus.Instance.Data.OnCreatePlayerData.AddListener(OnCreatePlayerData);
            MessageBus.Instance.Data.OnReleasePlayerData.AddListener(OnReleasePlayerData);
            MessageBus.Instance.Data.OnCreateActorData.AddListener(OnCreateActorData);
            MessageBus.Instance.Data.OnReleaseActorData.AddListener(OnReleaseActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.RemoveListener(UserCommandSetActorOperationMode);

            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.RemoveListener(UserCommandLookAtAngle);
            MessageBus.Instance.UserInput.UserCommandSetLookAtSpace.RemoveListener(UserCommandSetLookAtSpace);
            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.RemoveListener(UserCommandSetLookAtDistance);
            MessageBus.Instance.UserInput.UserCommandSetSpaceMapLookAtAngle.RemoveListener(UserCommandSetSpaceMapLookAtAngle);
            MessageBus.Instance.UserInput.UserCommandSetSpaceMapLookAtDistance.RemoveListener(UserCommandSetSpaceMapLookAtDistance);

            MessageBus.Instance.UserInput.UserInputSetExecuteWeapon.RemoveListener(UserInputSetExecuteWeapon);
            MessageBus.Instance.UserInput.UserInputReloadWeapon.RemoveListener(UserInputReloadWeapon);
            MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.RemoveListener(UserInputSetCurrentWeaponGroupIndex);

            MessageBus.Instance.UserInput.UserInputForwardBoosterPowerRatio.RemoveListener(UserInputForwardBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputBackBoosterPowerRatio.RemoveListener(UserInputBackBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputRightBoosterPowerRatio.RemoveListener(UserInputRightBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputLeftBoosterPowerRatio.RemoveListener(UserInputLeftBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputTopBoosterPowerRatio.RemoveListener(UserInputTopBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputBottomBoosterPowerRatio.RemoveListener(UserInputBottomBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputPitchBoosterPowerRatio.RemoveListener(UserInputPitchBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputRollBoosterPowerRatio.RemoveListener(UserInputRollBoosterPowerRatio);
            MessageBus.Instance.UserInput.UserInputYawBoosterPowerRatio.RemoveListener(UserInputYawBoosterPowerRatio);

            MessageBus.Instance.Player.SetAreaId.RemoveListener(PlayerCommandSetAreaId);

            MessageBus.Instance.User.SetPlayer.RemoveListener(SetUserPlayer);
            MessageBus.Instance.User.SetControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.User.SetObserveTarget.RemoveListener(SetUserObserveTarget);

            MessageBus.Instance.User.SetObserveArea.RemoveListener(SetUserObserveArea);

            MessageBus.Instance.Data.OnCreatePlayerData.RemoveListener(OnCreatePlayerData);
            MessageBus.Instance.Data.OnReleasePlayerData.RemoveListener(OnReleasePlayerData);
            MessageBus.Instance.Data.OnCreateActorData.RemoveListener(OnCreateActorData);
            MessageBus.Instance.Data.OnReleaseActorData.RemoveListener(OnReleaseActorData);
        }

        void UserCommandSetActorOperationMode(ActorOperationMode actorOperationMode)
        {
            questData.UserData.SetActorOperationMode(actorOperationMode);

            MessageBus.Instance.UserInput.UserInputSetExecuteWeapon.Broadcast(false);

            switch (actorOperationMode)
            {
                case ActorOperationMode.Observe:
                    InputLayerController.Instance.PushLayer(new ActorOperationObserveInputLayer(questData.UserData));
                    break;
                case ActorOperationMode.ObserveFreeCamera:
                    InputLayerController.Instance.PushLayer(new ActorOperationObserveFreeCameraInputLayer(questData.UserData));
                    break;
                case ActorOperationMode.Cockpit:
                    InputLayerController.Instance.PushLayer(new ActorOperationCockpitInputLayer(questData.UserData));
                    break;
                case ActorOperationMode.CockpitFreeCamera:
                    InputLayerController.Instance.PushLayer(new ActorOperationCockpitFreeCameraInputLayer(questData.UserData));
                    break;
                case ActorOperationMode.Spotter:
                    InputLayerController.Instance.PushLayer(new ActorOperationSpotterInputLayer(questData.UserData));
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    InputLayerController.Instance.PushLayer(new ActorOperationSpotterFreeCameraInputLayer(questData.UserData));
                    break;
            }
        }

        void UserCommandLookAtAngle(Vector3 lookAt)
        {
            questData.UserData.SetLookAtAngle(lookAt);

            if (questData.UserData.ControlActorData != null)
            {
                MessageBus.Instance.Actor.SetLookAtDirection.Broadcast(
                    questData.UserData.ControlActorData.InstanceId,
                    questData.UserData.LookAtSpace * Quaternion.Euler(lookAt) * Vector3.forward);
            }
        }

        void UserCommandSetLookAtSpace(Quaternion quaternion)
        {
            questData.UserData.SetLookAtSpace(quaternion);
        }

        void UserCommandSetLookAtDistance(float distance)
        {
            questData.UserData.SetLookAtDistance(distance);
        }

        void UserCommandSetSpaceMapLookAtAngle(Vector3 lookAt)
        {
            questData.UserData.SetSpaceMapLookAtAngle(lookAt);
        }

        void UserCommandSetSpaceMapLookAtDistance(float distance)
        {
            questData.UserData.SetSpaceMapLookAtDistance(distance);
        }

        void UserInputSetExecuteWeapon(bool isExecute)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.SetWeaponExecute.Broadcast(questData.UserData.ControlActorData.InstanceId, isExecute);
        }

        void UserInputReloadWeapon()
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.ReloadWeapon.Broadcast(questData.UserData.ControlActorData.InstanceId);
        }

        void UserInputSetCurrentWeaponGroupIndex(int index)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.SetCurrentWeaponGroupIndex.Broadcast(questData.UserData.ControlActorData.InstanceId, index);
        }

        void UserInputForwardBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.ForwardBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputBackBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.BackBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputRightBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.RightBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputLeftBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.LeftBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputTopBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.TopBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputBottomBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.BottomBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputPitchBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.PitchBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputRollBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.RollBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputYawBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.Actor.YawBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void PlayerCommandSetAreaId(Guid actorId, int? areaId)
        {
            if (actorId != questData.UserData.ControlActorData?.InstanceId)
            {
                return;
            }

            // ObserveTargetとObserveAreaが違っていたら合わせる
            if (areaId != questData.UserData.ObserveAreaData?.AreaId)
            {
                var nextAreaData = areaId.HasValue ? questData.StarSystemData.GetAreaData(areaId.Value) : null;
                MessageBus.Instance.User.SetObserveArea.Broadcast(nextAreaData);
            }
        }

        void SetUserPlayer(PlayerData userPlayer)
        {
            questData.UserData.SetPlayerData(userPlayer);

            MessageBus.Instance.User.SetControlActor.Broadcast(userPlayer.ActorDataList.FirstOrDefault());
        }

        void SetUserControlActor(ActorData actorData)
        {
            Debug.Log($"Set User Actor\n{actorData?.InstanceId}");

            if (questData.UserData.ControlActorData != null)
            {
                questData.UserData.ControlActorData.ActorStateData.EnableThink = false;
            }

            questData.UserData.SetControlActorData(actorData);

            if (questData.UserData.ControlActorData != null)
            {
                questData.UserData.ControlActorData.ActorStateData.EnableThink = true;
            }

            MessageBus.Instance.User.SetObserveTarget.Broadcast(actorData);
        }

        void SetUserObserveTarget(IPositionData positionData)
        {
            Debug.Log($"Set Observe Target\n{positionData?.InstanceId}");
            questData.UserData.SetObserveTarget(positionData);

            if (questData.UserData.ObserveAreaData?.AreaId != positionData?.AreaId)
            {
                // FIXME: 死んだ時の表示を考える
                // 今はとりあえずControlActorDataがnullだった場合はエリアそのまま
                if (questData.UserData.ControlActorData != null)
                {
                    var nextAreaData = positionData?.AreaId != null ? questData.StarSystemData.GetAreaData(positionData.AreaId.Value) : null;
                    MessageBus.Instance.User.SetObserveArea.Broadcast(nextAreaData);
                }
            }

            MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.Broadcast(positionData);
        }

        void SetUserObserveArea(AreaData areaData)
        {
            Debug.Log($"Set Observe Area\n{areaData?.AreaId}");
            questData.UserData.SetObserveAreaData(areaData);
        }

        void OnCreatePlayerData(PlayerData playerData)
        {
            Debug.Log($"Created Player\n{playerData.InstanceId}");
            if (playerData.PlayerProperty.ContainsKey(PlayerPropertyKey.UserPlayer))
            {
                MessageBus.Instance.User.SetPlayer.Broadcast(playerData);
            }
        }
        
        void OnReleasePlayerData(PlayerData playerData)
        {
            Debug.Log($"Release Player\n{playerData.InstanceId}");
        }
        
        void OnCreateActorData(ActorData actorData)
        {
            Debug.Log($"Created Actor\n{actorData.InstanceId}");
            // ユーザー所有のActorで、まだControlActorDataが無い場合は設定
            if (questData.UserData.PlayerData.ActorDataList.Any(x => x.InstanceId == actorData.InstanceId))
            {
                if (questData.UserData.ControlActorData == null)
                {
                    MessageBus.Instance.User.SetControlActor.Broadcast(actorData);
                }
            }
        }

        void OnReleaseActorData(ActorData actorData)
        {
            if (questData.UserData.ControlActorData?.InstanceId == actorData.InstanceId)
            {
                MessageBus.Instance.User.SetControlActor.Broadcast(null);
            }
        }
    }
}
