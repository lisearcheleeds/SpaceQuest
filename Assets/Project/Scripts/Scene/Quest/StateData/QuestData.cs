using System.Collections.Generic;

namespace AloneSpace
{
    /// <summary>
    /// QuestData
    /// Initializeのみで渡される
    /// フィールドとして保持することは禁止（Messageでやりとりすること
    /// </summary>
    public class QuestData
    {
        public StarSystemData StarSystemData { get; }

        // FIXME: Dictionaryにしたい
        public UserData UserData { get; }
        public List<PlayerQuestData> PlayerQuestData { get; } = new List<PlayerQuestData>();
        public List<ActorData> ActorData { get; } = new List<ActorData>();

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
            UserData = new UserData();
        }

        public void SetupPlayerQuestData()
        {
            var (players, actors) = QuestDataUtil.GetRandomPlayerDataList(1, StarSystemData.AreaData);
            PlayerQuestData.AddRange(players);
            ActorData.AddRange(actors);
        }
    }
}
