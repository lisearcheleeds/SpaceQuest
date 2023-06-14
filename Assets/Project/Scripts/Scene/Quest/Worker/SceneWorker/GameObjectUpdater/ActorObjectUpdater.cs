using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace AloneSpace
{
    public class ActorObjectUpdater
    {
        QuestData questData;

        MonoBehaviour coroutineWorker;
        Coroutine currentCoroutine;
        Transform variableParent;

        PlayerData observePlayerData;
        AreaData observeAreaData;
        bool isDirty;

        List<Actor> actors = new List<Actor>();

        public void Initialize(QuestData questData, Transform variableParent, MonoBehaviour coroutineWorker)
        {
            this.questData = questData;
            this.variableParent = variableParent;
            this.coroutineWorker = coroutineWorker;

            MessageBus.Instance.SetDirtyActorObjectList.AddListener(SetDirtyActorObjectList);

            MessageBus.Instance.CreatedActorData.AddListener(AddedActorData);
            MessageBus.Instance.ReleasedActorData.AddListener(RemovedActorData);

            MessageBus.Instance.SetUserPlayer.AddListener(SetUserPlayer);
            MessageBus.Instance.SetUserArea.AddListener(SetUserArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetDirtyActorObjectList.RemoveListener(SetDirtyActorObjectList);

            MessageBus.Instance.CreatedActorData.RemoveListener(AddedActorData);
            MessageBus.Instance.ReleasedActorData.RemoveListener(RemovedActorData);

            MessageBus.Instance.SetUserPlayer.RemoveListener(SetUserPlayer);
            MessageBus.Instance.SetUserArea.RemoveListener(SetUserArea);
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                isDirty = false;

                if (currentCoroutine != null)
                {
                    coroutineWorker.StopCoroutine(currentCoroutine);
                }

                currentCoroutine = coroutineWorker.StartCoroutine(Refresh());
            }

            foreach (var actor in actors)
            {
                actor.OnLateUpdate();
            }
        }

        void SetUserPlayer(PlayerData playerData)
        {
            this.observePlayerData = playerData;
            SetDirtyActorObjectList();
        }

        void SetUserArea(AreaData areaData)
        {
            this.observeAreaData = areaData;
            SetDirtyActorObjectList();
        }

        IEnumerator Refresh()
        {
            // ObserveのMainActorDataもしくは現在のエリア内のActorを表示
            // ワープ中のActorを表示するため
            var actorDataList = questData.ActorData.Values
                .Where(actorData => observePlayerData?.MainActorData?.InstanceId == actorData.InstanceId || (actorData.AreaId.HasValue && actorData.AreaId == observeAreaData?.AreaId));

            // オブジェクトを削除
            foreach (var actor in actors.ToArray())
            {
                if (actorDataList.All(loadActor => loadActor.InstanceId != actor.ActorData.InstanceId))
                {
                    DestroyActor(actor);
                }
            }

            if (observeAreaData == null)
            {
                yield break;
            }

            var coroutines = new List<IEnumerator>();

            // オブジェクトを生成
            foreach (var actorData in actorDataList)
            {
                if (actors.All(actor => actorData.InstanceId != actor.ActorData.InstanceId))
                {
                    coroutines.Add(CreatePlayerActor(actorData));
                }
            }

            yield return new ParallelCoroutine(coroutines);
            currentCoroutine = null;
        }

        IEnumerator CreatePlayerActor(ActorData actorData)
        {
            yield return Actor.CreateActor(
                actorData,
                variableParent,
                actor => actors.Add(actor));
        }

        void DestroyActor(Actor target)
        {
            target.DestroyActor();
            actors.Remove(target);
        }

        void SetDirtyActorObjectList()
        {
            isDirty = true;
        }

        void AddedActorData(ActorData actorData)
        {
            if (actorData.AreaId == observeAreaData?.AreaId)
            {
                // 多分タイミング的に問題ないはず
                coroutineWorker.StartCoroutine(CreatePlayerActor(actorData));
            }
        }

        void RemovedActorData(ActorData actorData)
        {
            if (actorData.AreaId == observeAreaData?.AreaId)
            {
                DestroyActor(actors.First(x => x.ActorData.InstanceId == actorData.InstanceId));
            }
        }
    }
}
