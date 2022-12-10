using System;
using System.Linq;
using System.Collections.Generic;

namespace AloneSpace
{
    public class QuestData
    {
        public StarSystemData StarSystemData { get; }

        public List<PlayerQuestData> PlayerQuestData { get; } = new List<PlayerQuestData>();
        public List<ActorData> ActorData { get; } = new List<ActorData>();

        public PlayerQuestData ObservePlayerQuestData { get; private set; }
        public AreaData ObserveAreaData { get; private set; }

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
        }

        public void Initialize()
        {
            var (players, actors) = QuestDataUtil.GetRandomPlayerDataList(10, StarSystemData.AreaData.Length);
            PlayerQuestData.AddRange(players);
            ActorData.AddRange(actors);
        }

        public void SetObservePlayer(Guid playerInstanceId)
        {
            ObservePlayerQuestData = PlayerQuestData.First(x => x.InstanceId == playerInstanceId);
        }

        public void SetObserveArea(int observeAreaIndex)
        {
            ObserveAreaData = StarSystemData.AreaData[observeAreaIndex];
        }

        public void AddPlayerQuestData(PlayerQuestData playerQuestData)
        {
            PlayerQuestData.Add(playerQuestData);
        }

        public void AddActorData(ActorData actorData)
        {
            ActorData.Add(actorData);
        }

        public void RemoveActorData(ActorData actorData)
        {
            ActorData.Remove(actorData);
        }

        public ActorData[] GetObservePlayerActors()
        {
            return ActorData.Where(x => ObservePlayerQuestData.InstanceId == x.PlayerInstanceId).ToArray();
        }
    }
}
