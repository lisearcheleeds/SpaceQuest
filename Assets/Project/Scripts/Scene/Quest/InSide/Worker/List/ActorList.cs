using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoboQuest.Quest.InSide
{
    public class ActorList
    {
        bool isDirty = false;
        int areaIndex;
        AreaTransitionObject[] areaTransitionObjects;
        MonoBehaviour coroutineWorker;
        
        List<Actor> actors = new List<Actor>();
        
        public void Initialize()
        {
            MessageBus.Instance.ManagerCommandArriveActor.AddListener(OnArriveActor);
            MessageBus.Instance.ManagerCommandLeaveActor.AddListener(OnLeaveActor);
            MessageBus.Instance.SubscribeUpdateAll.AddListener(SubscribeUpdateAll);
            MessageBus.Instance.NoticeBroken.AddListener(NoticeBroken);
        }

        public void LateUpdate()
        {
            if (isDirty)
            {
                MessageBus.Instance.SubscribeUpdateActorList.Broadcast(actors.ToArray());
                isDirty = false;
            }
        }

        public void Finalize()
        {
            MessageBus.Instance.ManagerCommandArriveActor.RemoveListener(OnArriveActor);
            MessageBus.Instance.ManagerCommandLeaveActor.RemoveListener(OnLeaveActor);
            MessageBus.Instance.SubscribeUpdateAll.RemoveListener(SubscribeUpdateAll);
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
        }

        public void ResetArea()
        {
            foreach (var actor in actors.ToArray())
            {
                DestroyActor(actor);
            }

            isDirty = true;
        }
        
        public IEnumerator LoadArea(QuestData questData, int areaIndex, MonoBehaviour coroutineWorker)
        {
            this.areaIndex = areaIndex;
            this.coroutineWorker = coroutineWorker;
            
            // LoadArea時に読み込むPlayerDataはRunnnigがTrueなPlayerのみ
            var loadActors = questData.ActorData.Where(x => x.CurrentAreaIndex == areaIndex && x.IsAlive).ToArray();
            var coroutines = new List<IEnumerator>();

            foreach (var loadActor in loadActors)
            {
                if (loadActor.ActorState != ActorState.Running)
                {
                    continue;
                }

                coroutines.Add(CreatePlayerActors(loadActor, false));
            }

            yield return new ParallelCoroutine(coroutines.ToArray());
            isDirty = true;
        }

        public void OnLoadedArea()
        {
            foreach (var actor in actors)
            {
                actor.Spawn();
            }
            
            isDirty = true;
        }

        void OnArriveActor(ActorData actorData)
        {
            if (areaIndex == actorData.CurrentAreaIndex)
            {
                coroutineWorker.StartCoroutine(CreatePlayerActors(actorData, actorData.IsAlive));
            }
        }

        void OnLeaveActor(ActorData actorData)
        {
            if (areaIndex == actorData.CurrentAreaIndex)
            {
                // 初回はnull
                var actor = actors.FirstOrDefault(x => x.InstanceId == actorData.InstanceId);
                if (actor != null)
                {
                    DestroyActor(actor);
                }
            }

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
                actorBase =>
                {
                    // 既に存在している場合はこのままSpawn
                    if (withSpawn)
                    {
                        actorBase.Spawn();
                    }

                    actors.Add(actorBase);
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

        void DestroyActor(Actor target)
        {
            target.DestroyActor();
            actors.Remove(target);
        }
    }
}
