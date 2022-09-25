using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using AloneSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class ActorObjectUpdater : IUpdater
    {
        bool isDirty = false;
        MonoBehaviour coroutineWorker;
        
        List<Actor> actors = new List<Actor>();
        Transform variableParent;

        public void Initialize(Transform variableParent, MonoBehaviour coroutineWorker)
        {
            MessageBus.Instance.SubscribeUpdateAll.AddListener(SubscribeUpdateAll);
            MessageBus.Instance.NoticeBroken.AddListener(NoticeBroken);
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);

            this.variableParent = variableParent;
            this.coroutineWorker = coroutineWorker;
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                MessageBus.Instance.SubscribeUpdateActorList.Broadcast(actors.ToArray());
                isDirty = false;
            }
        }

        public void Finalize()
        {
            MessageBus.Instance.SubscribeUpdateAll.RemoveListener(SubscribeUpdateAll);
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
            MessageBus.Instance.ManagerCommandTransitionActor.RemoveListener(ManagerCommandTransitionActor);
        }
        
        public IEnumerator LoadArea(QuestData questData)
        {
            // LoadArea時に読み込むPlayerDataはRunnnigがTrueなPlayerのみ
            var loadActors = questData.ActorData.Where(x => questData.ObserveAdjacentAreaData.Any(y => y.AreaData.AreaIndex == x.AreaIndex));
           
            // 次のエリアに引き継がないオブジェクトを削除
            foreach (var actor in actors.ToArray())
            {
                if (loadActors.All(loadActor => loadActor.InstanceId != actor.ActorData.InstanceId))
                {
                    DestroyActor(actor);
                }
            }
            
            var coroutines = new List<IEnumerator>();
            
            // 次のエリアで新規生成する必要があるオブジェクトを生成
            foreach (var loadActor in loadActors)
            {
                if (actors.All(actor => loadActor.InstanceId != actor.ActorData.InstanceId))
                {
                    coroutines.Add(CreatePlayerActors(loadActor, false));
                }
            }

            yield return new ParallelCoroutine(coroutines);
            
            // エリアの周辺のオブジェクトの位置調整
            foreach (var actor in actors)
            {
                var areaData = questData.ObserveAdjacentAreaData.First(x => x.AreaData.AreaIndex == actor.ActorData.AreaIndex);
                var offset = areaData.AreaDirection.HasValue
                    ? AreaCellVertex.GetVector(areaData.AreaDirection.Value)
                    : Vector3.zero;

                // FIXME: offsetを適用する
                // actor.transform.position += offset * questData.MapData.AreaSize;
                actor.transform.position += actor.ActorData.Position;
            }
            isDirty = true;
        }

        public void OnLoadedArea()
        {
            isDirty = true;
        }

        void SubscribeUpdateAll()
        {
            isDirty = true;
        }

        IEnumerator CreatePlayerActors(ActorData actorData, bool withSpawn)
        {
            yield return Actor.CreateActor(
                actorData,
                variableParent,
                actor =>
                {
                    actorData.SetActorState(ActorState.Running);
                    actors.Add(actor);
                });

            isDirty = true;
            
            MessageBus.Instance.SubscribeUpdateAll.Broadcast();
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

            isDirty = true;
        }

        void ManagerCommandTransitionActor(ActorData actorData, int fromAreaIndex, int toAreaIndex)
        {
            if (toAreaIndex == actorData.AreaIndex)
            {
                // 追加
                coroutineWorker.StartCoroutine(CreatePlayerActors(actorData, actorData.IsAlive));
                isDirty = true;
                return;
            }

            if (fromAreaIndex == actorData.AreaIndex)
            {
                // 削除
                // 初回はnull
                var actor = actors.FirstOrDefault(x => x.InstanceId == actorData.InstanceId);
                if (actor != null)
                {
                    DestroyActor(actor);
                }
            }
        }

        void DestroyActor(Actor target)
        {
            target.DestroyActor();
            actors.Remove(target);

            isDirty = true;
        }
    }
}
