using System;
using System.Collections.Generic;
using System.Linq;

namespace AloneSpace
{
    /// <summary>
    /// QuestData
    /// Initializeのみで渡される
    /// </summary>
    public class QuestData
    {
        public StarSystemData StarSystemData { get; }

        public UserData UserData { get; }
        public Dictionary<Guid, PlayerQuestData> PlayerQuestData { get; private set; }
        public Dictionary<Guid, ActorData> ActorData { get; private set; }
        public Dictionary<Guid, WeaponEffectData> WeaponEffectData { get; private set; }

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
            UserData = new UserData();
            WeaponEffectData = new Dictionary<Guid, WeaponEffectData>();
        }

        public void SetupPlayerQuestData()
        {
            var (players, actors) = QuestDataUtil.GetRandomPlayerDataList(2, StarSystemData.AreaData);

            PlayerQuestData = players.ToDictionary(kv => kv.InstanceId, kv => kv);
            ActorData = actors.ToDictionary(kv => kv.InstanceId, kv => kv);
        }
    }
}
