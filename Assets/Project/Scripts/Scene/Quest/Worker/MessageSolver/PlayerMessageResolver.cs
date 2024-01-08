using System;

namespace AloneSpace
{
    public class PlayerMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Player.SetTacticsType.AddListener(PlayerCommandSetTacticsType);
        }

        public void Finalize()
        {
            MessageBus.Instance.Player.SetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerData[playerInstanceId].SetTacticsType(tacticsType);
        }
    }
}