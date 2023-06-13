using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class QuestUpdater
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void OnStart()
        {
            // UserのPlayerを登録
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(1, 1);

            // TODO: 何か考える
            MessageBus.Instance.SetOrderUserPlayer.Broadcast(questData.PlayerQuestData.First().Key);

            // 他のPlayerを登録
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(5, 1);
            var otherPlayerCount = Random.Range(1, 3);
            for (int i = 0; i < otherPlayerCount; i++)
            {
                MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(1, 1);
            }
        }

        public void Finalize()
        {
        }

        public void OnLateUpdate(float deltaTime)
        {
        }
    }
}
