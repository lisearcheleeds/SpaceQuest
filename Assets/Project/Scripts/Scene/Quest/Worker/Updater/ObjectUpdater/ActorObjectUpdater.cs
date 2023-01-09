using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace AloneSpace
{
    public class ActorObjectUpdater : IUpdater
    {
        QuestData questData;
        
        MonoBehaviour coroutineWorker;
        Coroutine currentCoroutine;
        Transform variableParent;
        AreaData currentAreaData;
        bool isDirty;
        
        List<Actor> actors = new List<Actor>();

        public void Initialize(QuestData questData, Transform variableParent, MonoBehaviour coroutineWorker)
        {
            this.questData = questData;
            this.variableParent = variableParent;
            this.coroutineWorker = coroutineWorker;
            
            MessageBus.Instance.SetDirtyActorObjectList.AddListener(SetDirtyActorObjectList);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetDirtyActorObjectList.RemoveListener(SetDirtyActorObjectList);
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
        
        public void SetObserveAreaData(AreaData areaData)
        {
            this.currentAreaData = areaData;
            SetDirtyActorObjectList();
        }

        IEnumerator Refresh()
        {
            if (currentAreaData == null)
            {
                yield break;
            }

            var actorDataList = questData.ActorData.Where(actorData => actorData.AreaId == currentAreaData.AreaId);

            // オブジェクトを削除
            foreach (var actor in actors.ToArray())
            {
                if (actorDataList.All(loadActor => loadActor.InstanceId != actor.ActorData.InstanceId))
                {
                    DestroyActor(actor);
                }
            }
            
            var coroutines = new List<IEnumerator>();
            
            // オブジェクトを生成
            foreach (var actorData in actorDataList)
            {
                if (actors.All(actor => actorData.InstanceId != actor.ActorData.InstanceId))
                {
                    coroutines.Add(CreatePlayerActors(actorData));
                }
            }

            yield return new ParallelCoroutine(coroutines);
            currentCoroutine = null;
        }

        IEnumerator CreatePlayerActors(ActorData actorData)
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
    }
}
