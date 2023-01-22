using System;
using UnityEngine;

namespace AloneSpace
{
    public class MessageController
    {
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UtilGetAreaData.AddListener(UtilGetAreaData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UtilGetAreaData.RemoveListener(UtilGetAreaData);
        }

        void UtilGetAreaData(int areaId, Action<AreaData> callback)
        {
            callback(questData.StarSystemData.GetAreaData(areaId));
        }
    }
}