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
            
            MessageBus.Instance.UtilGetAreaData.SetListener(UtilGetAreaData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UtilGetAreaData.SetListener(null);
        }

        AreaData UtilGetAreaData(int areaId)
        {
            return questData.StarSystemData.GetAreaData(areaId);
        }
    }
}