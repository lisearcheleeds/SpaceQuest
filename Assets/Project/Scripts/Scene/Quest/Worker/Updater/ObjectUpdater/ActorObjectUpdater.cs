using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace AloneSpace
{
    public class ActorObjectUpdater : IUpdater
    {
        MonoBehaviour coroutineWorker;
        
        QuestData questData;
        List<Actor> actors = new List<Actor>();
        Transform variableParent;

        public void Initialize(QuestData questData, Transform variableParent, MonoBehaviour coroutineWorker)
        {
            MessageBus.Instance.NoticeBroken.AddListener(NoticeBroken);
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);

            this.questData = questData;
            this.variableParent = variableParent;
            this.coroutineWorker = coroutineWorker;
        }

        public void Finalize()
        {
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
            MessageBus.Instance.ManagerCommandTransitionActor.RemoveListener(ManagerCommandTransitionActor);
        }

        public void OnLateUpdate()
        {
        }
        
        public IEnumerator LoadArea()
        {
            return RefreshInteractObject();
        }

        public IEnumerator RefreshInteractObject()
        {
            var actorDataList = questData.ActorData.Where(actorData => actorData.AreaId == questData.ObserveAreaData.AreaId);

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
            
            // エリアの周辺のオブジェクトの位置調整
            foreach (var actor in actors)
            {
                actor.transform.position += actor.ActorData.Position;
            }
        }

        IEnumerator CreatePlayerActors(ActorData actorData)
        {
            yield return Actor.CreateActor(
                actorData,
                variableParent,
                actor =>
                {
                    actorData.SetActorState(ActorState.Running);
                    actors.Add(actor);
                });
        }

        void NoticeBroken(IDamageableData damageableData)
        {
            if (!(damageableData is ActorData actorData))
            {
                return;
            }

            foreach (var actor in actors)
            {
                if (actor.InstanceId == actorData.InstanceId)
                {
                    DestroyActor(actor);
                    break;
                }
            }
        }

        void ManagerCommandTransitionActor(ActorData actorData, int fromAreaId, int toAreaId)
        {
            coroutineWorker.StartCoroutine(RefreshInteractObject());
        }

        void DestroyActor(Actor target)
        {
            target.DestroyActor();
            actors.Remove(target);
        }
    }
}
