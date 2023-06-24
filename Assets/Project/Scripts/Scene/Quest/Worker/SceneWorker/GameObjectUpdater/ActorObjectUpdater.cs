using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ActorObjectUpdater
    {
        QuestData questData;

        MonoBehaviour coroutineWorker;
        Transform variableParent;

        IPositionData userObserveTarget;
        AreaData observeArea;
        bool isDirty;

        List<Actor> currentActorList = new List<Actor>();
        Dictionary<Guid, Coroutine> loadingActors = new Dictionary<Guid, Coroutine>();

        public void Initialize(QuestData questData, Transform variableParent, MonoBehaviour coroutineWorker)
        {
            this.questData = questData;
            this.variableParent = variableParent;
            this.coroutineWorker = coroutineWorker;

            MessageBus.Instance.SetDirtyActorObjectList.AddListener(SetDirtyActorObjectList);

            MessageBus.Instance.CreatedActorData.AddListener(CreatedActorData);
            MessageBus.Instance.ReleasedActorData.AddListener(ReleasedActorData);

            MessageBus.Instance.SetUserObserveTarget.AddListener(SetUserObserveTarget);
            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetDirtyActorObjectList.RemoveListener(SetDirtyActorObjectList);

            MessageBus.Instance.CreatedActorData.RemoveListener(CreatedActorData);
            MessageBus.Instance.ReleasedActorData.RemoveListener(ReleasedActorData);

            MessageBus.Instance.SetUserObserveTarget.RemoveListener(SetUserObserveTarget);
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }

            foreach (var currentActor in currentActorList)
            {
                currentActor.OnLateUpdate();
            }
        }

        void SetUserObserveTarget(IPositionData userObserveTarget)
        {
            this.userObserveTarget = userObserveTarget;
            SetDirtyActorObjectList();
        }

        void SetUserObserveArea(AreaData areaData)
        {
            observeArea = areaData;
            SetDirtyActorObjectList();
        }

        void Refresh()
        {
            ActorData[] shouldActorDataList;

            // TODO: エリア外をAreaIdの組み合わせで定義する
            if (observeArea == null)
            {
                // ワープ中はActor自身のみ表示
                // 0 or 1つしか取れないけど
                var userObserveActorTarget = userObserveTarget as ActorData;
                shouldActorDataList = questData.ActorData.Values.Where(actorData => actorData.InstanceId == userObserveActorTarget?.InstanceId).ToArray();
            }
            else
            {
                // 現在のエリア内のすべてのActor
                shouldActorDataList = questData.ActorData.Values.Where(actorData => actorData.AreaId == observeArea?.AreaId).ToArray();
            }

            // オブジェクトを削除
            // 現在のActorのうち、actorDataListに無いものを削除
            foreach (var currentActor in currentActorList.ToArray())
            {
                if (shouldActorDataList.All(shouldActorData => shouldActorData.InstanceId != currentActor.ActorData.InstanceId))
                {
                    DestroyActor(currentActor);
                }
            }

            // オブジェクトを生成
            // actorDataListのうち、現在のActorに無いものを生成
            foreach (var shouldActorData in shouldActorDataList)
            {
                if (currentActorList.All(currentActor => shouldActorData.InstanceId != currentActor.ActorData.InstanceId))
                {
                    if (!loadingActors.ContainsKey(shouldActorData.InstanceId))
                    {
                        CreateActor(shouldActorData);
                    }
                }
            }
        }

        void CreateActor(ActorData actorData)
        {
            loadingActors.Add(
                actorData.InstanceId,
                coroutineWorker.StartCoroutine(
                Actor.CreateActor(
                    actorData,
                    variableParent,
                    actor =>
                    {
                        currentActorList.Add(actor);
                        loadingActors.Remove(actorData.InstanceId);
                    })));
        }

        void DestroyActor(Actor target)
        {
            target.DestroyActor();
            currentActorList.Remove(target);
        }

        void SetDirtyActorObjectList()
        {
            isDirty = true;
        }

        void CreatedActorData(ActorData actorData)
        {
            if (actorData.AreaId == observeArea?.AreaId)
            {
                if (currentActorList.All(currentActor => actorData.InstanceId != currentActor.ActorData.InstanceId))
                {
                    if (!loadingActors.ContainsKey(actorData.InstanceId))
                    {
                        CreateActor(actorData);
                    }
                }
            }
        }

        void ReleasedActorData(ActorData actorData)
        {
            if (actorData.AreaId == observeArea?.AreaId)
            {
                DestroyActor(currentActorList.First(currentActor => currentActor.ActorData.InstanceId == actorData.InstanceId));
            }
        }
    }
}
