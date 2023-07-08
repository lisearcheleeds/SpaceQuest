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
            var actorDataDividedArea = questData.ActorData.GroupBy(actorData => actorData.Value.AreaId, actorData => actorData.Value);
            foreach (var currentAreaActorDataList in actorDataDividedArea)
            {
                foreach (var fromActor in currentAreaActorDataList)
                {
                    // FIXME: 明らかにキャッシュしたほうがGC的に優しい気がするがめっちゃ遅くなる
                    actorRelationDataCache[fromActor.InstanceId] = currentAreaActorDataList
                        .Where(otherActor => otherActor.InstanceId != fromActor.InstanceId)
                        .Select(otherActor => new ActorRelationData(fromActor, otherActor)).ToArray();
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
