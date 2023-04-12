using System;

namespace AloneSpace
{
    public class PlayerMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.PlayerCommandSetTacticsType.AddListener(PlayerCommandSetTacticsType);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerQuestData[playerInstanceId].SetTacticsType(tacticsType);
        }
    }
}