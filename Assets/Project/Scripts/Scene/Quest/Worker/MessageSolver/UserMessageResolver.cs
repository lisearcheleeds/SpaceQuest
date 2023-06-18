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

            MessageBus.Instance.PlayerCommandSetAreaId.AddListener(PlayerCommandSetAreaId);

            MessageBus.Instance.SetUserPlayer.AddListener(SetUserPlayer);
            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveTarget.AddListener(SetUserObserveTarget);

            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);

            MessageBus.Instance.CreatedPlayerData.AddListener(CreatedPlayerData);
            MessageBus.Instance.CreatedActorData.AddListener(CreatedActorData);
            MessageBus.Instance.ReleasedActorData.AddListener(ReleasedActorData);
        }

        public void Finalize()
        {
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

            MessageBus.Instance.PlayerCommandSetAreaId.RemoveListener(PlayerCommandSetAreaId);

            MessageBus.Instance.SetUserPlayer.RemoveListener(SetUserPlayer);
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveTarget.RemoveListener(SetUserObserveTarget);

            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);

            MessageBus.Instance.CreatedPlayerData.RemoveListener(CreatedPlayerData);
            MessageBus.Instance.CreatedActorData.RemoveListener(CreatedActorData);
            MessageBus.Instance.ReleasedActorData.RemoveListener(ReleasedActorData);
        }

        void UserCommandSetActorOperationMode(ActorOperationMode actorOperationMode)
        {
            questData.UserData.SetActorOperationMode(actorOperationMode);
        }

        void UserCommandLookAtAngle(Vector3 lookAt)
        {
            questData.UserData.SetLookAtAngle(lookAt);

            if (questData.UserData.ControlActorData != null)
            {
                MessageBus.Instance.ActorCommandSetLookAtDirection.Broadcast(
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

        void UserInputSetExecuteWeapon(bool isExecute)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandSetWeaponExecute.Broadcast(questData.UserData.ControlActorData.InstanceId, isExecute);
        }

        void UserInputReloadWeapon()
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandReloadWeapon.Broadcast(questData.UserData.ControlActorData.InstanceId);
        }

        void UserInputSetCurrentWeaponGroupIndex(int index)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandSetCurrentWeaponGroupIndex.Broadcast(questData.UserData.ControlActorData.InstanceId, index);
        }

        void UserInputForwardBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputBackBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputRightBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputLeftBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputTopBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputBottomBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputPitchBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputRollBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
        }

        void UserInputYawBoosterPowerRatio(float power)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.Broadcast(questData.UserData.ControlActorData.InstanceId, power);
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
                MessageBus.Instance.SetUserObserveArea.Broadcast(nextAreaData);
            }
        }

        void SetUserPlayer(PlayerData userPlayer)
        {
            questData.UserData.SetPlayerData(userPlayer);

            MessageBus.Instance.SetUserControlActor.Broadcast(userPlayer.ActorDataList.FirstOrDefault());
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

            MessageBus.Instance.SetUserObserveTarget.Broadcast(actorData);
        }

        void SetUserObserveTarget(IPositionData positionData)
        {
            Debug.Log($"Set Observe Target\n{positionData?.InstanceId}");
            questData.UserData.SetObserveTarget(positionData);

            if (questData.UserData.ObserveAreaData?.AreaId != positionData?.AreaId)
            {
                var nextAreaData = positionData?.AreaId != null ? questData.StarSystemData.GetAreaData(positionData.AreaId.Value) : null;
                MessageBus.Instance.SetUserObserveArea.Broadcast(nextAreaData);
            }

            MessageBus.Instance.UserCommandSetCameraTrackTarget.Broadcast(positionData);
        }

        void SetUserObserveArea(AreaData areaData)
        {
            Debug.Log($"Set Observe Area\n{areaData?.AreaId}");
            questData.UserData.SetObserveAreaData(areaData);
        }

        void CreatedPlayerData(PlayerData playerData)
        {
            Debug.Log($"Created Player\n{playerData.InstanceId}");
            if (playerData.PlayerProperty.ContainsKey(PlayerPropertyKey.UserPlayer))
            {
                MessageBus.Instance.SetUserPlayer.Broadcast(playerData);
            }
        }

        void CreatedActorData(ActorData actorData)
        {
            Debug.Log($"Created Actor\n{actorData.InstanceId}");
            // ユーザー所有のActorで、まだControlActorDataが無い場合は設定
            if (questData.UserData.PlayerData.ActorDataList.Any(x => x.InstanceId == actorData.InstanceId))
            {
                if (questData.UserData.ControlActorData == null)
                {
                    MessageBus.Instance.SetUserControlActor.Broadcast(actorData);
                }
            }
        }

        void ReleasedActorData(ActorData actorData)
        {
            if (questData.UserData.PlayerData.ActorDataList.Any(x => x.InstanceId == actorData.InstanceId))
            {
                if (questData.UserData.ControlActorData?.InstanceId == actorData.InstanceId)
                {
                    MessageBus.Instance.SetUserControlActor.Broadcast(null);
                }
            }
        }
    }
}
