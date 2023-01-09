using System.Collections.Generic;

namespace AloneSpace
{
    public class QuestData
    {
        public StarSystemData StarSystemData { get; }

        public List<PlayerQuestData> PlayerQuestData { get; } = new List<PlayerQuestData>();
        public List<ActorData> ActorData { get; } = new List<ActorData>();

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
        }

        public void SetupPlayerQuestData()
        {
            var (players, actors) = QuestDataUtil.GetRandomPlayerDataList(1, StarSystemData.AreaData);
            PlayerQuestData.AddRange(players);
            ActorData.AddRange(actors);
        }
    }
}
