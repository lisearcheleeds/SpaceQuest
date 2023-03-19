using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class MessageController
    {
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UtilGetPlayerQuestData.SetListener(UtilGetPlayerQuestData);
            MessageBus.Instance.UtilGetAreaData.SetListener(UtilGetAreaData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UtilGetAreaData.Clear();
        }
        
        PlayerQuestData UtilGetPlayerQuestData(Guid instanceId)
        {
            return questData.PlayerQuestData.FirstOrDefault(playerQuestData => playerQuestData.InstanceId == instanceId);
        }

        AreaData UtilGetAreaData(int areaId)
        {
            return questData.StarSystemData.GetAreaData(areaId);
        }
    }
}