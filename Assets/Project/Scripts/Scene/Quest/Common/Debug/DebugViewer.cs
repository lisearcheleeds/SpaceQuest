using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class DebugViewer : MonoBehaviour
    {
        [SerializeField] PlayerQuestDataView[] playerQuestDataView;
        [SerializeField] AreaDataView[] areaDataView;

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
        public class AreaDataView
        {
            [SerializeField] string areaId;
            [SerializeField] Vector3 position;

            public AreaDataView(AreaData areaData)
            {
                this.areaId = $"Area {areaData.AreaId}";
                this.position = areaData.Position;
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
            UpdatePlayerQuestDataView();
        }

        public void Finalize()
        {
            MessageBus.Instance.SetObserveArea.RemoveListener(SetObserveArea);
        }

        void SetObserveArea(int toAreaId)
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
                        actorDataList.Select(x => new ActorDataView(x.AreaId)).ToArray());
                }).ToArray();

            areaDataView = questData.StarSystemData.AreaData.Select(areadata =>
            {
                return new AreaDataView(areadata);
            }).ToArray();
        }
    }
}