using System;
using System.Collections;
using AloneSpace;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace AloneSpace
{
    public class Actor : MonoBehaviour
    {
        public Guid InstanceId => ActorData.InstanceId;
        public Guid PlayerInstanceId => ActorData.PlayerInstanceId;

        public ActorData ActorData { get; private set; }
        public ITargetData TargetData => ActorData;

        [SerializeField] ActorModel actorModel;

        /// <summary>
        /// ActorStatusからActorを作成
        /// </summary>
        public static IEnumerator CreateActor(ActorData actorData, Transform parent, Action<Actor> onCreated)
        {
            Actor actor = null;
            yield return AssetLoader.LoadAsync<Actor>(ConstantAssetPath.ActorPathVO, actorPrefab =>
            {
                actor = Instantiate(actorPrefab, actorData.Position, actorData.Rotation, parent);
            });

            actor.ActorData = actorData;
            
            yield return actor.actorModel.LoadActorModel(actorData.ActorSpecData);
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
    }
}
