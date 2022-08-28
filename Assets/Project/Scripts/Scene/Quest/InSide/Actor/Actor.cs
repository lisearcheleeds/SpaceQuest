using System;
using System.Collections;
using RoboQuest;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace AloneSpace.InSide
{
    public class Actor : MonoBehaviour, IActor, ITarget, ICollision
    {
        public Guid InstanceId => ActorData.InstanceId;
        public Guid PlayerInstanceId => ActorData.PlayerQuestDataInstanceId;

        public ActorData ActorData => actorAI.ActorAIHandler.ActorData;

        public ITargetData TargetData => ActorData;
        public Vector3 MoveDelta { get; private set; }
        
        public bool IsCollidable => ActorData.ActorState == ActorState.Running;
        public CollisionShape CollisionShape { get; } = new CollisionShapeSphere(Vector3.zero, 3.0f);

        [SerializeField] ActorModel actorModel;

        [SerializeField] Rigidbody rigidbody;
        [SerializeField] ActorDebugger actorDebugger;
        
        ActorAI actorAI;

        Vector3 prevPosition;

        public void Spawn()
        {
            actorAI.Spawn();
        }

        /// <summary>
        /// ActorStatusからActorを作成
        /// </summary>
        public static IEnumerator CreateActor(ActorData actorData, Action<Actor> onCreated)
        {
            Actor actor = null;
            yield return AssetLoader.LoadAsync<Actor>(ConstantAssetPath.ActorPathVO, actorPrefab =>
            {
                actor = Instantiate(actorPrefab, actorData.Position, actorData.Rotation);
            });

            actor.actorAI = new ActorAI(actor, actorData, actor);
            actor.actorDebugger.Setup(actor.actorAI);
            
            yield return actor.actorModel.LoadActorModel(actorData.ActorSpecData);
            yield return null;
            
            onCreated(actor);
        }

        public void DestroyActor()
        {
            Destroy(gameObject);
        }

        void Start()
        {
            actorAI.OnStart();
            CollisionShape.Position = transform.position;
            
            MessageBus.Instance.SendTarget.Broadcast(this, true);
            MessageBus.Instance.SendCollision.Broadcast(this, true);
            MessageBus.Instance.SendIntuition.Broadcast(this, true);
        }

        void OnDestroy()
        {
            actorAI.OnDestroy();
            
            MessageBus.Instance.SendTarget.Broadcast(this, false);
            MessageBus.Instance.SendCollision.Broadcast(this, false);
            MessageBus.Instance.SendIntuition.Broadcast(this, false);
        }

        void Update()
        {
            if (ActorData.ActorState != ActorState.Running)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            
            MoveDelta = transform.position - prevPosition;
            prevPosition = transform.position;
            
            actorAI.ActorAIHandler.Velocity = rigidbody.velocity;
            ActorData.SetPosition(transform.position);
            ActorData.SetRotation(transform.rotation);
            
            actorAI.Update(deltaTime);

            if (ActorData.ActorState != ActorState.Running)
            {
                return;
            }

            // 移動
            if (actorAI.ActorAIHandler.RequestMove != default)
            {
                var velocityDirection = Vector3.Lerp(
                    actorAI.ActorAIHandler.ActorData.Rotation * Vector3.forward,
                    actorAI.ActorAIHandler.RequestMove.normalized,
                    ActorData.ActorSpecData.DirectionMovingModify);
                rigidbody.velocity = velocityDirection * ActorData.ActorSpecData.MovingSpeed * actorAI.ActorAIHandler.Throttle;
                
                actorAI.ActorAIHandler.RequestMove = default;
            }

            // クイック移動
            if (actorAI.ActorAIHandler.RequestQuickMove != default)
            {
                rigidbody.velocity = actorAI.ActorAIHandler.RequestQuickMove.normalized * ActorData.ActorSpecData.QuickMovingSpeed;
                actorAI.ActorAIHandler.RequestQuickMove = default;
            }
            
            // 回転
            if (actorAI.ActorAIHandler.RequestRotate != default)
            {
                rigidbody.angularVelocity = actorAI.ActorAIHandler.RequestRotate.normalized * ActorData.ActorSpecData.RotateSpeed;
                actorAI.ActorAIHandler.RequestRotate = default;
            }
            
            // クイック回転
            if (actorAI.ActorAIHandler.RequestQuickRotate != default)
            {
                rigidbody.angularVelocity = actorAI.ActorAIHandler.RequestQuickRotate * ActorData.ActorSpecData.QuickRotateSpeed;
                actorAI.ActorAIHandler.RequestQuickRotate = default;
            }

            CollisionShape.Position = transform.position;
        }
        
        void LateUpdate()
        {
            // FIXME: 無くしたい
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }
}
