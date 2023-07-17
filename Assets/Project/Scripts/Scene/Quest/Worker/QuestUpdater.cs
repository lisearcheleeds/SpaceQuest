using System.Collections.Generic;
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
            var userDic = new Dictionary<PlayerPropertyKey, IPlayerPropertyValue>();
            userDic.Add(PlayerPropertyKey.UserPlayer, EmptyPlayerPropertyValue.Empty);
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(1, userDic, 1);

            // 他のPlayerを登録
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(5, null, 1);
            var otherPlayerCount = Random.Range(1, 3);
            for (var i = 0; i < otherPlayerCount; i++)
            {
                MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(1, null, 1);
            }
        }

        public void Finalize()
        {
        }

        public void OnUpdate(float deltaTime)
        {
        }
    }
}
