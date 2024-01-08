using System;

namespace AloneSpace
{
    public class PlayerMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Temp.PlayerCommandSetTacticsType.AddListener(PlayerCommandSetTacticsType);
        }

        public void Finalize()
        {
            MessageBus.Instance.Temp.PlayerCommandSetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerData[playerInstanceId].SetTacticsType(tacticsType);
        }
    }
}