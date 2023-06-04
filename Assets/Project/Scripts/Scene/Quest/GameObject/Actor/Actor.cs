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
            yield return AssetLoader.LoadAsync<Actor>(ConstantAssetPath.ActorPathVO, actorPrefab =>
            {
                actor = Instantiate(actorPrefab, actorData.Position, actorData.Rotation, parent);
                actor.ActorData = actorData;
            });

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

        static IEnumerator LoadActorModel(ActorData actorData, Actor actor)
        {
            yield return AssetLoader.LoadAsync<ActorModel>(actorData.ActorSpecVO.Path, prefab =>
            {
                actor.ActorModel = Instantiate(prefab, actor.transform, false);
                actorData.SetActorGameObjectHandler(actor.ActorModel.Init(actorData, actorData.CollisionEventModule));
            });
        }

        static IEnumerator LoadWeaponModel(Dictionary<Guid, WeaponData> weaponData, Actor actor)
        {
            actor.WeaponModels = new WeaponModel[weaponData.Count];
            yield return new ParallelCoroutine(weaponData.Values.Select(data =>
            {
                return AssetLoader.LoadAsync<WeaponModel>(
                    data.WeaponSpecVO.AssetPath,
                    prefab =>
                    {
                        actor.WeaponModels[data.WeaponIndex] = Instantiate(prefab, actor.ActorData.ActorGameObjectHandler.WeaponHolders[data.WeaponIndex], false);
                        data.SetWeaponGameObjectHandler(actor.WeaponModels[data.WeaponIndex].Init(data.WeaponHolder));
                    });
            }).ToArray());
        }
    }
}
