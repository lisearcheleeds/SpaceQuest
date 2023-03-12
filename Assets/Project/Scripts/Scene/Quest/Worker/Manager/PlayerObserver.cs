using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class PlayerObserver : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] AreaUpdater areaUpdater;

        PlayerQuestData observePlayerQuestData;
        AreaData currentAreaData;
        
        QuestData questData;

        ActorMode prevObserveActorMode;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            uiManager.Initialize(questData);
            areaUpdater.Initialize(questData);
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.AddListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.AddListener(ManagerCommandLoadArea);
            
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.AddListener(UserInputForwardBoosterPowerRatio);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.AddListener(UserInputBackBoosterPowerRatio);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.AddListener(UserInputRightBoosterPowerRatio);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.AddListener(UserInputLeftBoosterPowerRatio);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.AddListener(UserInputTopBoosterPowerRatio);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.AddListener(UserInputBottomBoosterPowerRatio);
            MessageBus.Instance.UserInputPitchBoosterPowerRatio.AddListener(UserInputPitchBoosterPowerRatio);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.AddListener(UserInputRollBoosterPowerRatio);
            MessageBus.Instance.UserInputYawBoosterPowerRatio.AddListener(UserInputYawBoosterPowerRatio);
            
            MessageBus.Instance.UserInputRotateLookAtDirection.AddListener(UserInputLookAtDirectionRotate);
            MessageBus.Instance.UserInputRotateToLookAtDirection.AddListener(UserInputRotateToLookAtDirection);
            
            MessageBus.Instance.UserInputSwitchActorMode.AddListener(UserInputSwitchActorMode);
            MessageBus.Instance.UserInputSetActorCombatMode.AddListener(UserInputSetActorCombatMode);
        }

        public void Finalize()
        {
            uiManager.Finalize();
            areaUpdater.Finalize();
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.RemoveListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.RemoveListener(ManagerCommandLoadArea);
            
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.RemoveListener(UserInputForwardBoosterPowerRatio);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.RemoveListener(UserInputBackBoosterPowerRatio);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.RemoveListener(UserInputRightBoosterPowerRatio);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.RemoveListener(UserInputLeftBoosterPowerRatio);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.RemoveListener(UserInputTopBoosterPowerRatio);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.RemoveListener(UserInputBottomBoosterPowerRatio);
            MessageBus.Instance.UserInputPitchBoosterPowerRatio.RemoveListener(UserInputPitchBoosterPowerRatio);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.RemoveListener(UserInputRollBoosterPowerRatio);
            MessageBus.Instance.UserInputYawBoosterPowerRatio.RemoveListener(UserInputYawBoosterPowerRatio);
            
            MessageBus.Instance.UserInputRotateLookAtDirection.RemoveListener(UserInputLookAtDirectionRotate);
            MessageBus.Instance.UserInputRotateToLookAtDirection.RemoveListener(UserInputRotateToLookAtDirection);
            
            MessageBus.Instance.UserInputSwitchActorMode.RemoveListener(UserInputSwitchActorMode);
            MessageBus.Instance.UserInputSetActorCombatMode.RemoveListener(UserInputSetActorCombatMode);
        }
        
        public void OnLateUpdate()
        {
            if (observePlayerQuestData == null)
            {
                return;
            }

            uiManager.OnLateUpdate();
            areaUpdater.OnLateUpdate();

            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Warp)
            {
                if (observePlayerQuestData.MainActorData.AreaId != currentAreaData?.AreaId)
                {
                    MessageBus.Instance.ManagerCommandLoadArea.Broadcast(observePlayerQuestData.MainActorData.AreaId);
                }
            }

            if (prevObserveActorMode != observePlayerQuestData.MainActorData.ActorMode)
            {
                if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Cockpit);
                }
                else if(observePlayerQuestData.MainActorData.ActorMode != ActorMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Default);
                }
            }

            prevObserveActorMode = observePlayerQuestData.MainActorData.ActorMode;
        }
        
        void ManagerCommandSetObservePlayer(Guid playerInstanceId)
        {
            observePlayerQuestData = questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId);
            
            uiManager.SetObservePlayerQuestData(observePlayerQuestData);
            areaUpdater.SetObservePlayerQuestData(observePlayerQuestData);
            
            MessageBus.Instance.ManagerCommandLoadArea.Broadcast(observePlayerQuestData.MainActorData.AreaId);
            MessageBus.Instance.UserCommandSetCameraTrackTarget.Broadcast(observePlayerQuestData.MainActorData);
        }

        void ManagerCommandLoadArea(int? areaId)
        {
            currentAreaData = questData.StarSystemData.AreaData.FirstOrDefault(x => x.AreaId == areaId);
            
            uiManager.SetObserveAreaData(currentAreaData);
            areaUpdater.SetObserveAreaData(currentAreaData);
        }
        
        void UserInputForwardBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputBackBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputRightBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputLeftBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputTopBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputBottomBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputPitchBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputRollBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }
        
        void UserInputYawBoosterPowerRatio(float power)
        {
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.Broadcast(observePlayerQuestData.MainActorData.InstanceId, power);
        }

        void UserInputLookAtDirectionRotate(Vector2 mouseInputDelta)
        {
            // 右クリック前：カメラは自由
            // 右クリック中：カメラは自由、機体の回転が追従
            // 右クリック後：機体の回転にカメラ空間がリセット
            var localLookAtAngle = Quaternion.AngleAxis(-mouseInputDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseInputDelta.x, Vector3.up)
                                   * observePlayerQuestData.MainActorData.LookAt;
            
            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseInputDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = localLookAtAngle.y + mouseInputDelta.x;
            localLookAtAngle.z = 0;
            
            MessageBus.Instance.ActorCommandLookAt.Broadcast(observePlayerQuestData.MainActorData.InstanceId, localLookAtAngle);
        }

        void UserInputRotateToLookAtDirection(bool isRotateToLookAtDirection)
        {
            MessageBus.Instance.ActorCommandRotateToLookAt.Broadcast(observePlayerQuestData.MainActorData.InstanceId, isRotateToLookAtDirection);
        }

        void UserInputSwitchActorMode()
        {
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.ThirdPersonViewpoint)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(observePlayerQuestData.MainActorData.InstanceId, ActorMode.Cockpit);
            }
            else if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Cockpit)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(observePlayerQuestData.MainActorData.InstanceId, ActorMode.ThirdPersonViewpoint);
            }
        }

        void UserInputSetActorCombatMode(ActorCombatMode actorCombatMode)
        {
            MessageBus.Instance.ActorCommandSetActorCombatMode.Broadcast(observePlayerQuestData.MainActorData.InstanceId, actorCombatMode);
        }
    }
}