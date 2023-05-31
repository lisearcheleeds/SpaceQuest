using System;
using System.Collections;
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

            yield return actor.LoadActorModel(actorData.ActorSpecVO, actorData.WeaponSpecVOs);
            yield return null;

            onCreated(actor);
        }

        public void DestroyActor()
        {
            Destroy(gameObject);
        }

        public void OnLateUpdate()
        {
            transform.position = ActorData.Position;
            transform.rotation = ActorData.Rotation;
        }

        IEnumerator LoadActorModel(ActorSpecVO actorSpecVO, IWeaponSpecVO[] weaponSpecVOs)
        {
            yield return AssetLoader.LoadAsync<ActorModel>(actorSpecVO, prefab => ActorModel = Instantiate(prefab, transform, false));

            WeaponModels = new WeaponModel[weaponSpecVOs.Length];

            yield return new ParallelCoroutine(weaponSpecVOs.Select((vo, weaponIndex) => LoadWeaponModel(vo, ActorModel, weaponIndex)).ToArray());
        }

        IEnumerator LoadWeaponModel(IWeaponSpecVO weaponSpecVO, ActorModel actorModel, int weaponIndex)
        {
            yield return AssetLoader.LoadAsync<WeaponModel>(weaponSpecVO.AssetPath, prefab => WeaponModels[weaponIndex] = Instantiate(prefab, actorModel.WeaponHolder[weaponIndex], false));
        }
    }
}
