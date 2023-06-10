using System;
using System.Linq;

namespace AloneSpace
{
    public class QuestDataMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.UtilGetPlayerQuestData.SetListener(UtilGetPlayerQuestData);
            MessageBus.Instance.UtilGetAreaData.SetListener(UtilGetAreaData);
            MessageBus.Instance.UtilGetAreaActorData.SetListener(UtilGetAreaActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UtilGetPlayerQuestData.Clear();
            MessageBus.Instance.UtilGetAreaData.Clear();
            MessageBus.Instance.UtilGetAreaActorData.Clear();
        }

        PlayerQuestData UtilGetPlayerQuestData(Guid instanceId)
        {
            return questData.PlayerQuestData[instanceId];
        }

        AreaData UtilGetAreaData(int areaId)
        {
            return questData.StarSystemData.GetAreaData(areaId);
        }

        ActorData[] UtilGetAreaActorData(int areaId)
        {
            return questData.ActorData.Values.Where(x => x.AreaId == areaId).ToArray();
        }
    }
}