using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorData : ITargetData, IDamageableData, ICollisionData
    {
        public Guid InstanceId { get; }
        public Guid PlayerInstanceId { get; }

        public ActorMode ActorMode { get; private set; }
        public ActorState ActorState { get; private set; }

        public int? AreaId { get; private set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
        public IPositionData MoveTarget { get; private set; }

        public ActorSpecData ActorSpecData { get; }
        public InventoryData[] InventoryDataList { get; }
        
        public List<ICollisionData> CollidedList = new List<ICollisionData>();
        
        public WeaponData[] WeaponData { get; }

        public bool IsAlive => ActorState == ActorState.Alive;
        public bool IsBroken => ActorState == ActorState.Broken;
        public bool IsCollidable => ActorState == ActorState.Alive;

        public float HitPoint { get; private set; }

        public ActorAIStateData ActorAIStateData { get; } = new ActorAIStateData();
        public CollisionShape CollisionShape { get; }
        public Vector3 MoveDelta { get; }

        public ActorData(ActorSpecData actorSpecData, Guid playerInstanceId)
        {
            InstanceId = Guid.NewGuid();
            
            ActorSpecData = actorSpecData;
            PlayerInstanceId = playerInstanceId;

            InventoryDataList = actorSpecData.ActorPartsExclusiveInventoryParameterVOs
                .Select(vo => new InventoryData(vo.CapacityWidth, vo.CapacityHeight)).ToArray();
            
            WeaponData = ActorSpecData.ActorPartsWeaponParameterVOs.Select(x => AloneSpace.WeaponData.CreateData(playerInstanceId, InstanceId, this, x)).ToArray();
            
            CollisionShape = new CollisionShapeSphere(this, 3.0f);
            HitPoint = ActorSpecData.Endurance;
        }

        public void Update(float deltaTime)
        {
            // 移動チェック
            if (ActorMode == ActorMode.Warp)
            {
                // Warp中はStarSystem座標
                MessageBus.Instance.UtilGetOffsetStarSystemPosition.Broadcast(this, MoveTarget, offsetStarSystemPosition =>
                {
                    Position = Position + offsetStarSystemPosition.normalized;

                    if (offsetStarSystemPosition.magnitude < 1.0f)
                    {
                        Position = MoveTarget.Position;
                        MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(this, MoveTarget.AreaId.Value);
                        MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(this, null);
                    }
                });
            }
            else
            {
                // Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            }

            // 衝突チェック
            foreach (var collide in CollidedList)
            {
                var causeDamage = (collide as ICauseDamageData);
                if (causeDamage != null)
                {
                    MessageBus.Instance.NoticeDamage.Broadcast(causeDamage, this);
                }
            }
            
            CollidedList.Clear();
        }

        public void SetActorState(ActorState actorState)
        {
            ActorState = actorState;
        }

        public void OnCollision(ICollisionData collision)
        {
            CollidedList.Add(collision);
        }

        public void OnDamage(DamageData damageData)
        {
            HitPoint -= damageData.DamageValue;
            
            if (HitPoint <= 0 && ActorState != ActorState.Broken)
            {
                SetActorState(ActorState.Broken);
            }
        }

        public void SetInteractOrder(IInteractData interactData)
        {
            ActorAIStateData.InteractOrder = interactData;
        }

        public void SetAreaId(int? areaId)
        {
            AreaId = areaId;
        }

        public void SetMoveTarget(IPositionData moveTarget)
        {
            if (moveTarget == null)
            {
                ActorMode = ActorMode.General;
                MoveTarget = null;
                return;
            }

            // 今どのエリアにも居ない時、もしくは移動先のエリアが違う時ワープ状態とする
            if (!AreaId.HasValue || (AreaId.Value != moveTarget.AreaId.Value))
            {
                ActorMode = ActorMode.Warp;
                
                MessageBus.Instance.UtilGetStarSystemPosition.Broadcast(this, starSystemPosition => Position = starSystemPosition);
                MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(this, null);
            }
            else
            {
                ActorMode = ActorMode.General;
            }

            MoveTarget = moveTarget;
        }

        public void AddThreat(IThreatData threatData)
        {
            ActorAIStateData.ThreatList.Add(threatData);
        }
    }
}
