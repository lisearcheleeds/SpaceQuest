using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class DebugViewer : MonoBehaviour
    {
        [SerializeField] PlayerQuestDataView[] playerQuestDataView;

        QuestData questData;
        
        [Serializable]
        public class PlayerQuestDataView
        {
            [SerializeField] string instanceId;
            [SerializeField] ActorDataView[] actorDataView;

            public PlayerQuestDataView(Guid instanceId, ActorDataView[] actorDataView)
            {
                this.instanceId = instanceId.ToString();
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
            
            MessageBus.Instance.SetObserveArea.AddListener(SetObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetObserveArea.RemoveListener(SetObserveArea);
        }

        void SetObserveArea(int toAreaIndex)
        {
            UpdatePlayerQuestDataView();
        }

        void UpdatePlayerQuestDataView()
        {
            // プレイヤー + ザコ敵分表示される
            playerQuestDataView = questData.PlayerQuestData
                .Select(playerQuestData =>
                {
                    var actorDataList = questData.ActorData.Where(actorData => actorData.PlayerInstanceId == playerQuestData.InstanceId);
                    
                    return new PlayerQuestDataView(
                        playerQuestData.InstanceId,
                        actorDataList.Select(x => new ActorDataView(x.AreaIndex)).ToArray());
                }).ToArray();
        }
    }
}