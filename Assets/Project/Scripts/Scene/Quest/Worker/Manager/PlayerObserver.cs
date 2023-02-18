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
            MessageBus.Instance.UserInputDirectionAndRotation.AddListener(UserInputDirectionAndRotation);
            MessageBus.Instance.UserInputSwitchActorMode.AddListener(UserInputSwitchActorMode);
            MessageBus.Instance.UserInputSetActorCombatMode.AddListener(UserInputSetActorCombatMode);
        }

        public void Finalize()
        {
            uiManager.Finalize();
            areaUpdater.Finalize();
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.RemoveListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.RemoveListener(ManagerCommandLoadArea);
            MessageBus.Instance.UserInputDirectionAndRotation.RemoveListener(UserInputDirectionAndRotation);
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
                if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Combat)
                {
                    MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Cockpit);
                }
                else if(observePlayerQuestData.MainActorData.ActorMode != ActorMode.Combat)
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

        void UserInputDirectionAndRotation(Vector3 inputDirection, Vector2 inputRotation)
        {
            // 戦闘
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Combat)
            {
                if (observePlayerQuestData.MainActorData.ActorCombatMode == ActorCombatMode.Fighter)
                {
                    // 戦闘機っぽい動き メインブースターが付いてる体
                    // マウス上下でピッチ
                    // マウス左右でロール
                    // ADでヨー
                    // WSで加減速
                    MessageBus.Instance.ActorCommandMoveOrder.Broadcast(observePlayerQuestData.MainActorData.InstanceId, new Vector3(0, inputDirection.y, inputDirection.z));
                    
                    // 位置情報を軸に変換
                    var rotation = new Vector3(inputRotation.y, inputDirection.x, -inputRotation.x);
                    MessageBus.Instance.ActorCommandRotateOrder.Broadcast(observePlayerQuestData.MainActorData.InstanceId, rotation);
                }
                else
                {
                    // 戦闘ヘリっぽい動き メインブースターが付いてない体
                    // マウスでFPS操作
                    // WASDで平行移動
                    MessageBus.Instance.ActorCommandMoveOrder.Broadcast(observePlayerQuestData.MainActorData.InstanceId, inputDirection);
                    
                    // 位置情報を軸に変換
                    var rotation = new Vector3(inputRotation.y, inputRotation.x, 0);
                    MessageBus.Instance.ActorCommandRotateOrder.Broadcast(observePlayerQuestData.MainActorData.InstanceId, rotation);
                }
            }
        }

        void UserInputSwitchActorMode()
        {
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Standard)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(observePlayerQuestData.MainActorData.InstanceId, ActorMode.Combat);
            }
            else if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Combat)
            {
                MessageBus.Instance.ActorCommandSetActorMode.Broadcast(observePlayerQuestData.MainActorData.InstanceId, ActorMode.Standard);
            }
        }

        void UserInputSetActorCombatMode(ActorCombatMode actorCombatMode)
        {
            MessageBus.Instance.ActorCommandSetActorCombatMode.Broadcast(observePlayerQuestData.MainActorData.InstanceId, actorCombatMode);
        }
    }
}