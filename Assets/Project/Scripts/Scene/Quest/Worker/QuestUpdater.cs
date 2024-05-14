using System.Collections;
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

        public IEnumerator OnStart()
        {
            var areaData = questData.StarSystemData.AreaData;
            for (var i = 0; i < areaData.Length; i++)
            {
                // 雑ランダムアイテム
                var randomItemDataCount = Random.Range(3, 10);
                for (var itemIndex = 0; itemIndex < randomItemDataCount; itemIndex++)
                {
                    var itemData = new ItemData(new ItemVO(itemIndex), 1);
                    var position = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f));
                    MessageBus.Instance.Data.CreateItemInteractData.Broadcast(itemData, areaData[i].AreaId, position, Quaternion.identity);
                }

                // 自分/他エリアへの接続
                for (var t = 0; t < areaData.Length; t++)
                {
                    MessageBus.Instance.Data.CreateAreaInteractData.Broadcast(areaData[t], areaData[i].StarSystemPosition);
                }
            }

            yield return null;

            // UserのPlayerを登録
            var userDic = new Dictionary<PlayerPropertyKey, IPlayerPropertyValue>();
            userDic.Add(PlayerPropertyKey.UserPlayer, EmptyPlayerPropertyValue.Empty);
            MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(1, userDic, 1);

            yield return null;

            // 他のPlayerを登録
            MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(5, null, 1);
            var otherPlayerCount = Random.Range(1, 3);
            for (var i = 0; i < otherPlayerCount; i++)
            {
                MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.Broadcast(1, null, 1);
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
