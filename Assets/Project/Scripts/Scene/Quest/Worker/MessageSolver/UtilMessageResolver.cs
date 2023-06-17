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

            MessageBus.Instance.UtilGetPlayerData.SetListener(UtilGetPlayerData);
            MessageBus.Instance.UtilGetAreaData.SetListener(UtilGetAreaData);
            MessageBus.Instance.UtilGetAreaActorData.SetListener(UtilGetAreaActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UtilGetPlayerData.Clear();
            MessageBus.Instance.UtilGetAreaData.Clear();
            MessageBus.Instance.UtilGetAreaActorData.Clear();
        }

        PlayerData UtilGetPlayerData(Guid instanceId)
        {
            return questData.PlayerData[instanceId];
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
