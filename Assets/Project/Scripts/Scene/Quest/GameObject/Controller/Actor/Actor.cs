using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class Actor : MonoBehaviour
    {
        public ActorData ActorData { get; private set; }

        public ActorModel ActorModel { get; private set; }
        public WeaponModel[] WeaponModels { get; private set; }

        /// <summary>
        /// ActorStatusからActorを作成
        /// </summary>
        public static IEnumerator CreateActor(ActorData actorData, Transform parent, Action<Actor> onCreated)
        {
            Actor actor = null;
            yield return LoadActor(actorData, parent, LoadActor => actor = LoadActor);
            yield return LoadActorModel(actorData, actor);
            yield return LoadWeaponModel(actorData.WeaponData, actor);
            yield return null;

            onCreated(actor);
        }

        public void DestroyActor()
        {
            ActorData.SetActorGameObjectHandler(null);
            Destroy(gameObject);
        }

        public void OnLateUpdate()
        {
            transform.position = ActorData.Position;
            transform.rotation = ActorData.Rotation;
        }

        static IEnumerator LoadActor(ActorData actorData, Transform parent, Action<Actor> onComplete)
        {
            var isComplete = false;
            AssetLoader.Instance.LoadAsyncCache<Actor>(ConstantAssetPath.ActorPathVO, actorPrefab =>
            {
                var actor = Instantiate(actorPrefab, actorData.Position, actorData.Rotation, parent);
                actor.ActorData = actorData;

                onComplete(actor);
                isComplete = true;
            });

            yield return new WaitWhile(() => !isComplete);
        }

        static IEnumerator LoadActorModel(ActorData actorData, Actor actor)
        {
            var isComplete = false;
            AssetLoader.Instance.LoadAsyncCache<ActorModel>(actorData.ActorSpecVO.Path, prefab =>
            {
                actor.ActorModel = Instantiate(prefab, actor.transform, false);
                actorData.SetActorGameObjectHandler(actor.ActorModel.Init(actorData, actorData.CollisionEventModule));
                isComplete = true;
            });

            yield return new WaitWhile(() => !isComplete);
        }

        static IEnumerator LoadWeaponModel(Dictionary<Guid, WeaponData> weaponData, Actor actor)
        {
            var loadCounter = 0;
            actor.WeaponModels = new WeaponModel[weaponData.Count];

            foreach (var data in weaponData.Values)
            {
                AssetLoader.Instance.LoadAsyncCache<WeaponModel>(
                    data.WeaponSpecVO.Path,
                    prefab =>
                    {
                        actor.WeaponModels[data.WeaponIndex] = Instantiate(prefab, actor.ActorData.ActorGameObjectHandler.WeaponHolders[data.WeaponIndex], false);
                        data.SetWeaponGameObjectHandler(actor.WeaponModels[data.WeaponIndex].Init(data.WeaponHolder));
                        loadCounter++;
                    });
            }

            yield return new WaitWhile(() => loadCounter != weaponData.Count);
        }
    }
}
