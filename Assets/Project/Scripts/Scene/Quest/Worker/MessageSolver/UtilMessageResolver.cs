using System;
using System.Linq;

namespace AloneSpace
{
    public class UtilMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Util.GetPlayerData.SetListener(UtilGetPlayerData);
            MessageBus.Instance.Util.GetAreaData.SetListener(UtilGetAreaData);
            MessageBus.Instance.Util.GetAreaActorData.SetListener(UtilGetAreaActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Util.GetPlayerData.Clear();
            MessageBus.Instance.Util.GetAreaData.Clear();
            MessageBus.Instance.Util.GetAreaActorData.Clear();
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
