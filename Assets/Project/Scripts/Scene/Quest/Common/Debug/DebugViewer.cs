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
                this.areaId = $"Area {areaData?.AreaId}";
                this.position = areaData?.StarSystemPosition ?? Vector3.zero;
            }
        }
        
        [Serializable]
        public class ActorDataView
        {
            [SerializeField] int? currentAreaId;
            
            public ActorDataView(int? currentAreaId)
            {
                this.currentAreaId = currentAreaId;
            }
        }
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }

        [ContextMenu("BreakPoint")]
        void BreakPoint()
        {
            Debug.Log("BreakPoint");
        }
    }
}