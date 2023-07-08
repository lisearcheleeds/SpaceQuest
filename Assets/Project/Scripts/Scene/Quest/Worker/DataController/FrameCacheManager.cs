using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace AloneSpace
{
    public class FrameCacheManager
    {
        QuestData questData;

        Dictionary<Guid, ActorRelationData[]> actorRelationDataCache = new Dictionary<Guid, ActorRelationData[]>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.GetFrameCacheActorRelationData.SetListener(GetFrameCacheActorRelationData);
        }

        public void Finalize()
        {
            MessageBus.Instance.GetFrameCacheActorRelationData.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            // FIXME: 重いしGC走る
            var actorDataDividedArea = questData.ActorData.GroupBy(actorData => actorData.Value.AreaId).Select(x => x.Select(y => y.Value));

            foreach (var currentAreaActorDataList in actorDataDividedArea)
            {
                var count = currentAreaActorDataList.Count() - 1;
                foreach (var fromActor in currentAreaActorDataList)
                {
                    var fromActorInstanceId = fromActor.InstanceId;

                    // 自身を除いた個数
                    if (!actorRelationDataCache.ContainsKey(fromActorInstanceId) || actorRelationDataCache[fromActorInstanceId].Length != count)
                    {
                        actorRelationDataCache[fromActorInstanceId] = new ActorRelationData[count];
                    }

                    var currentCache = actorRelationDataCache[fromActorInstanceId];

                    var t = 0;
                    foreach (var t1 in currentAreaActorDataList)
                    {
                        if (fromActorInstanceId != t1.InstanceId)
                        {
                            currentCache[t].Update(fromActor, t1);
                            t++;
                        }
                    }
                }
            }
        }

        ReadOnlyArray<ActorRelationData> GetFrameCacheActorRelationData(Guid actorId)
        {
            if (actorId == default)
            {
                return Array.Empty<ActorRelationData>();
            }

            return actorRelationDataCache[actorId];
        }
    }
}
