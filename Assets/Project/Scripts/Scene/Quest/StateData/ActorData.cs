using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorData : ITargetData, IDamageableData, ICollisionData
    {
        // 情報
        public Guid InstanceId { get; }
        public Guid PlayerInstanceId { get; }

        public ActorSpecData ActorSpecData { get; }
        
        public InventoryData[] InventoryDataList { get; }
        public WeaponData[] WeaponData { get; }
        
        // 状態
        public ActorMode ActorMode { get; private set; }
        public ActorState ActorState { get; private set; }
        public ActorCombatMode ActorCombatMode { get; private set; } = ActorCombatMode.Fighter;

        public int? AreaId { get; private set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; } = Quaternion.identity;
        public Vector3 LookAtDirection { get; set; }

        public Vector3 InertiaTensor { get; private set; }
        public Quaternion InertiaTensorRotation { get; private set; } = Quaternion.identity;

        public IPositionData MoveTarget { get; private set; }
        
        public List<ICollisionData> CollidedList = new List<ICollisionData>();

        public bool IsAlive => ActorState == ActorState.Alive;
        public bool IsBroken => ActorState == ActorState.Broken;
        public bool IsCollidable => ActorState == ActorState.Alive;

        public float HitPoint { get; private set; }

        public ActorAIStateData ActorAIStateData { get; } = new ActorAIStateData();
        public CollisionShape CollisionShape { get; }
        
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
                // ワープ開始直後まだAreaに居る時は加速
                if (AreaId.HasValue && AreaId != MoveTarget.AreaId)
                {
                    var areaData = MessageBus.Instance.UtilGetAreaData.Unicast(AreaId.Value);
                    var moveTargetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(MoveTarget.AreaId.Value);

                    // Area内の移動
                    var offset = moveTargetAreaData.StarSystemPosition - areaData.StarSystemPosition;
                    InertiaTensor += offset.normalized * deltaTime * 2.0f;

                    // 範囲外になったらAreaから脱出 一旦1000.0f
                    if (Position.sqrMagnitude > 1000.0f * 1000.0f)
                    {
                        MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(this, null);
                        Position = areaData.StarSystemPosition;
                        InertiaTensor = Vector3.zero;
                    }
                }
                else if (!AreaId.HasValue)
                {
                    // ワープ中Areaの外に居る時の座標
                    var moveTargetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(MoveTarget.AreaId.Value);
                    
                    // Area外の移動
                    var offset = moveTargetAreaData.StarSystemPosition - Position;
                    InertiaTensor = offset.normalized * deltaTime * 2.0f * 30.0f;

                    // 目的地に近くなったらAreaに入る
                    if (offset.sqrMagnitude < InertiaTensor.sqrMagnitude)
                    {
                        // FIXME: 移動先ちゃんと考える
                        MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(this, MoveTarget.AreaId);
                        Position = MoveTarget.Position + MoveTarget.Position.normalized * 1000.0f;
                        InertiaTensor = Vector3.zero;
                    }
                }
                else if (AreaId.HasValue && AreaId == MoveTarget.AreaId)
                {
                    // FIXME: 移動の計算式ちゃんと考える
                    // ワープ終了目的のAreaに居る時は減速
                    // Area内の移動
                    var offset = MoveTarget.Position - Position;
                    InertiaTensor = offset.normalized * deltaTime * 2.0f * 30.0f;

                    // 目的地に近くなったらWarp終了
                    if (offset.sqrMagnitude < InertiaTensor.sqrMagnitude)
                    {
                        Position = MoveTarget.Position;
                        InertiaTensor = Vector3.zero;
                        MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(this, null); 
                    }
                }
            }
            else
            {
                InertiaTensor += Rotation * new Vector3(
                    (ActorAIStateData.RightBoosterPowerRatio - ActorAIStateData.LeftBoosterPowerRatio) * ActorSpecData.SubBoosterPower,
                    (ActorAIStateData.TopBoosterPowerRatio - ActorAIStateData.BottomBoosterPowerRatio) * ActorSpecData.SubBoosterPower,
                    ActorAIStateData.ForwardBoosterPowerRatio * ActorSpecData.MainBoosterPower + -ActorAIStateData.BackBoosterPowerRatio * ActorSpecData.SubBoosterPower);

                if ((ActorSpecData.MaxSpeed * ActorSpecData.MaxSpeed) < InertiaTensor.sqrMagnitude)
                {
                    // 最大速度制限
                    InertiaTensor *= ActorSpecData.MaxSpeed / InertiaTensor.sqrMagnitude;
                }

                InertiaTensorRotation = Quaternion.Euler(
                    ActorAIStateData.PitchBoosterPowerRatio * ActorSpecData.PitchBoosterPower,
                    ActorAIStateData.YawBoosterPowerRatio * ActorSpecData.YawBoosterPower,
                    ActorAIStateData.RollBoosterPowerRatio * ActorSpecData.RollBoosterPower);
            }
            
            Position += InertiaTensor;
            Rotation *= InertiaTensorRotation;
            
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
                ActorMode = ActorMode.ThirdPersonViewpoint;
                MoveTarget = null;
                return;
            }

            // 今どのエリアにも居ない時、もしくは移動先のエリアが違う時ワープ状態とする
            if (AreaId != moveTarget.AreaId)
            {
                ActorMode = ActorMode.Warp;
            }
            else
            {
                ActorMode = ActorMode.ThirdPersonViewpoint;
            }

            MoveTarget = moveTarget;
        }

        public void AddThreat(IThreatData threatData)
        {
            ActorAIStateData.ThreatList.Add(threatData);
        }
        
        public void SetForwardBoosterPowerRatio(float power)
        {
            ActorAIStateData.ForwardBoosterPowerRatio = power;
        }
        
        public void SetBackBoosterPowerRatio(float power)
        {
            ActorAIStateData.BackBoosterPowerRatio = power;
        }
        
        public void SetRightBoosterPowerRatio(float power)
        {
            ActorAIStateData.RightBoosterPowerRatio = power;
        }
        
        public void SetLeftBoosterPowerRatio(float power)
        {
            ActorAIStateData.LeftBoosterPowerRatio = power;
        }
        
        public void SetTopBoosterPowerRatio(float power)
        {
            ActorAIStateData.TopBoosterPowerRatio = power;
        }
        
        public void SetBottomBoosterPowerRatio(float power)
        {
            ActorAIStateData.BottomBoosterPowerRatio = power;
        }
        
        public void SetPitchBoosterPowerRatio(float power)
        {
            ActorAIStateData.PitchBoosterPowerRatio = power;
        }
        
        public void SetRollBoosterPowerRatio(float power)
        {
            ActorAIStateData.RollBoosterPowerRatio = power;
        }
        
        public void SetYawBoosterPowerRatio(float power)
        {
            ActorAIStateData.YawBoosterPowerRatio = power;
        }

        public void SetLookAtDirection(Vector3 lookAt)
        {
            LookAtDirection = lookAt;
        }

        public void SetActorMode(ActorMode actorMode)
        {
            ActorMode = actorMode;
        }

        public void SetActorCombatMode(ActorCombatMode actorCombatMode)
        {
            ActorCombatMode = actorCombatMode;
        }
    }
}
