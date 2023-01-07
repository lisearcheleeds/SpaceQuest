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

        public ActorState ActorState { get; private set; }

        public int AreaId { get; private set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
        public ActorSpecData ActorSpecData { get; }
        public InventoryData[] InventoryDataList { get; }
        public List<ICollisionData> CollidedList = new List<ICollisionData>();
        
        public WeaponData[] WeaponData { get; }

        public bool IsAlive => ActorState == ActorState.Running;
        public bool IsBroken => ActorState == ActorState.Broken;
        public bool IsCollidable => ActorState == ActorState.Running;

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

        public void SetActorState(ActorState actorState)
        {
            ActorState = actorState;
        }
        
        public void SetAreaId(int areaId)
        {
            AreaId = areaId;
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

        public void Update(float deltaTime)
        {
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

        public void SetInteractOrder(IInteractData interactData)
        {
            ActorAIStateData.InteractOrder = interactData;
        }

        public void SetMoveTarget(IPosition moveTarget)
        {
            ActorAIStateData.MoveTarget = moveTarget;
        }

        public void AddThreat(IThreatData threatData)
        {
            ActorAIStateData.ThreatList.Add(threatData);
        }
    }
}
