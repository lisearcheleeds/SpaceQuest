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
            
            MessageBus.Instance.UtilGetStarSystemPosition.AddListener(UtilGetStarSystemPosition);
            MessageBus.Instance.UtilGetOffsetStarSystemPosition.AddListener(UtilGetOffsetStarSystemPosition);
            MessageBus.Instance.UtilGetNearestAreaData.AddListener(UtilGetNearestAreaData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UtilGetStarSystemPosition.RemoveListener(UtilGetStarSystemPosition);
            MessageBus.Instance.UtilGetOffsetStarSystemPosition.RemoveListener(UtilGetOffsetStarSystemPosition);
            MessageBus.Instance.UtilGetNearestAreaData.RemoveListener(UtilGetNearestAreaData);
        }

        void UtilGetStarSystemPosition(IPositionData positionData, Action<Vector3> callback)
        {
            callback(questData.StarSystemData.GetStarSystemPosition(positionData));
        }

        void UtilGetOffsetStarSystemPosition(IPositionData from, IPositionData to, Action<Vector3> callback)
        {
            callback(questData.StarSystemData.GetOffsetStarSystemPosition(from, to));
        }

        void UtilGetNearestAreaData(IPositionData positionData, Action<AreaData> callback)
        {
            callback(questData.StarSystemData.GetNearestAreaData(positionData));
        }
    }
}