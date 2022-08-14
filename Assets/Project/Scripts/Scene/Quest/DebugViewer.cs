using System;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class DebugViewer : MonoBehaviour
    {
        [SerializeField] PlayerQuestDataView[] playerQuestDataView;

        QuestData questData;
        
        [Serializable]
        public class PlayerQuestDataView
        {
            [SerializeField] int destinateArea;
            [SerializeField] ActorDataView[] actorDataView;

            public PlayerQuestDataView(int destinateArea, ActorDataView[] actorDataView)
            {
                this.destinateArea = destinateArea;
                this.actorDataView = actorDataView;
            }
        }
        
        [Serializable]
        public class ActorDataView
        {
            [SerializeField] int currentArea;
            
            public ActorDataView(int currentArea)
            {
                this.currentArea = currentArea;
            }
        }
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);
        }

        public void Finalize()
        {
            MessageBus.Instance.ManagerCommandTransitionActor.RemoveListener(ManagerCommandTransitionActor);
        }

        void ManagerCommandTransitionActor(ActorData actorData, int areaId)
        {
            UpdatePlayerQuestDataView();
        }

        void UpdatePlayerQuestDataView()
        {
            // プレイヤー + ザコ敵分表示される
            playerQuestDataView = questData.PlayerQuestData
                .Select(playerQuestData =>
                {
                    var actorDataList = questData.ActorData.Where(actorData => actorData.PlayerQuestDataInstanceId == playerQuestData.InstanceId);
                    
                    return new PlayerQuestDataView(
                        playerQuestData.DestinateAreaIndex.HasValue ? playerQuestData.DestinateAreaIndex.Value : -1,
                        actorDataList.Select(x => new ActorDataView(x.CurrentAreaIndex)).ToArray());
                }).ToArray();
        }
    }
}