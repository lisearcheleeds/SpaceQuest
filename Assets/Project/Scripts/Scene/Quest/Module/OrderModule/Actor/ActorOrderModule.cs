using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorOrderModule : IOrderModule
    {
        ActorData actorData;

        public ActorOrderModule(ActorData actorData)
        {
            this.actorData = actorData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterOrderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterOrderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            if (CheckRelease())
            {
                return;
            }

            UpdateDamage();
            UpdateTarget();
            UpdateWeapon();
            UpdateWarp(deltaTime);
            UpdateMove(deltaTime);
            UpdateInteract(deltaTime);
        }

        bool CheckRelease()
        {
            if (actorData.IsReleased)
            {
                MessageBus.Instance.ReleaseActorData.Broadcast(actorData);
                return true;
            }

            return false;
        }

        void UpdateDamage()
        {
            if (0 < actorData.ActorStateData.EnduranceValue)
            {
                foreach (var currentDamageEventData in actorData.ActorStateData.CurrentDamageEventData)
                {
                    var decayDamageValue = currentDamageEventData.EffectedDamageValue;

                    var shieldExcessDamage = Mathf.Abs(Mathf.Min(0, actorData.ActorStateData.ShieldValue - decayDamageValue));
                    var shieldDamage = Mathf.Abs(Mathf.Max(actorData.ActorStateData.ShieldValue, decayDamageValue));

                    var afterShieldValue = Mathf.Max(0, actorData.ActorStateData.ShieldValue - decayDamageValue);
                    var afterEnduranceValue = Mathf.Max(0, actorData.ActorStateData.EnduranceValue - shieldExcessDamage);

                    var overKillDamage = Mathf.Abs(Mathf.Min(0, actorData.ActorStateData.EnduranceValue - shieldExcessDamage));

                    var isLethalDamage = afterEnduranceValue <= 0;

                    actorData.ActorStateData.ShieldValue = afterShieldValue;
                    actorData.ActorStateData.EnduranceValue = afterEnduranceValue;

                    actorData.ActorStateData.DamageEventHistoryDataList.Add(new DamageEventHistoryData(
                        currentDamageEventData,
                        decayDamageValue,
                        shieldDamage,
                        shieldExcessDamage,
                        overKillDamage,
                        isLethalDamage));

                    if (isLethalDamage)
                    {
                        MessageBus.Instance.NoticeBrokenActorEventData.Broadcast(new BrokenActorEventData(actorData, currentDamageEventData));
                        break;
                    }
                }
            }

            actorData.ActorStateData.CurrentDamageEventData.Clear();
        }

        void UpdateTarget()
        {
            if (actorData.AreaId.HasValue)
            {
                // なんか1秒に数回までとかにする
                var aroundActors = MessageBus.Instance.UtilGetAreaActorData.Unicast(actorData.AreaId.Value);
                if (!aroundActors.SequenceEqual(actorData.ActorStateData.AroundTargets))
                {
                    // 一致してなかったら更新
                    MessageBus.Instance.ActorCommandSetAroundTargets.Broadcast(actorData.InstanceId, aroundActors);
                }

                // 向いてる方向に一番近いターゲットをメインに
                var nextMainTarget = actorData.ActorStateData.AroundTargets
                    .OrderByDescending(target => Vector3.Dot(actorData.ActorStateData.LookAtDirection, (target.Position - actorData.Position).normalized))
                    .FirstOrDefault(target => target.InstanceId != actorData.InstanceId);
                if (actorData.ActorStateData.MainTarget?.InstanceId != nextMainTarget?.InstanceId)
                {
                    MessageBus.Instance.ActorCommandSetMainTarget.Broadcast(actorData.InstanceId, nextMainTarget);
                }
            }
        }

        void UpdateWeapon()
        {
            foreach (var weaponData in actorData.WeaponData.Values)
            {
                weaponData.SetLookAtDirection(actorData.ActorStateData.LookAtDirection);
                weaponData.SetTargetData(actorData.ActorStateData.MainTarget);
            }
        }

        void UpdateWarp(float deltaTime)
        {
            if (!actorData.ActorStateData.IsWarping)
            {
                return;
            }

            // ワープ開始直後まだAreaに居る時は加速
            if (actorData.AreaId.HasValue && actorData.AreaId != actorData.ActorStateData.MoveTarget.AreaId)
            {
                var areaData = MessageBus.Instance.UtilGetAreaData.Unicast(actorData.AreaId.Value);
                var moveTargetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(actorData.ActorStateData.MoveTarget.AreaId.Value);

                // Area内の移動
                var offset = moveTargetAreaData.StarSystemPosition - areaData.StarSystemPosition;
                actorData.MovingModule.SetMovementVelocity(actorData.MovingModule.MovementVelocity + offset.normalized * deltaTime * 2.0f);

                // 範囲外になったらAreaから脱出 一旦1000.0f
                if (actorData.Position.sqrMagnitude > 1000.0f * 1000.0f)
                {
                    MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(actorData.InstanceId, null);
                    actorData.SetPosition(areaData.StarSystemPosition);
                    actorData.MovingModule.SetMovementVelocity(Vector3.zero);
                }

                return;
            }

            if (!actorData.AreaId.HasValue)
            {
                // ワープ中Areaの外に居る時の座標
                var moveTargetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(actorData.ActorStateData.MoveTarget.AreaId.Value);

                // Area外の移動
                var offset = moveTargetAreaData.StarSystemPosition - actorData.Position;
                actorData.MovingModule.SetMovementVelocity(offset.normalized * deltaTime * 2.0f * 30.0f);

                // 目的地に近くなったらAreaに入る
                if (offset.sqrMagnitude < actorData.MovingModule.MovementVelocity.sqrMagnitude)
                {
                    // FIXME: 移動先ちゃんと考える
                    MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(actorData.InstanceId, actorData.ActorStateData.MoveTarget.AreaId);
                    actorData.SetPosition(actorData.ActorStateData.MoveTarget.Position + actorData.ActorStateData.MoveTarget.Position.normalized * 1000.0f);
                    actorData.MovingModule.SetMovementVelocity(Vector3.zero);
                }

                return;
            }

            if (actorData.AreaId.HasValue && actorData.AreaId == actorData.ActorStateData.MoveTarget.AreaId)
            {
                // FIXME: 移動の計算式ちゃんと考える
                // ワープ終了目的のAreaに居る時は減速
                // Area内の移動
                var offset = actorData.ActorStateData.MoveTarget.Position - actorData.Position;
                actorData.MovingModule.SetMovementVelocity(offset.normalized * deltaTime * 2.0f * 30.0f);

                // 目的地に近くなったらWarp終了
                if (offset.sqrMagnitude < actorData.MovingModule.MovementVelocity.sqrMagnitude)
                {
                    actorData.SetPosition(actorData.ActorStateData.MoveTarget.Position);
                    actorData.MovingModule.SetMovementVelocity(Vector3.zero);
                    MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(actorData.InstanceId, null);
                }
            }
        }

        void UpdateMove(float deltaTime)
        {
            if (actorData.ActorStateData.IsWarping)
            {
                return;
            }

            // ベースにする場合は1秒単位に戻す
            var prevMovementVelocity = actorData.MovingModule.MovementVelocity / deltaTime;
            var nextMovementVelocity = prevMovementVelocity + actorData.Rotation
                * new Vector3(
                    (actorData.ActorStateData.RightBoosterPowerRatio - actorData.ActorStateData.LeftBoosterPowerRatio) * actorData.ActorSpecVO.SubBoosterPower,
                    (actorData.ActorStateData.TopBoosterPowerRatio - actorData.ActorStateData.BottomBoosterPowerRatio) * actorData.ActorSpecVO.SubBoosterPower,
                    actorData.ActorStateData.ForwardBoosterPowerRatio * actorData.ActorSpecVO.MainBoosterPower + -actorData.ActorStateData.BackBoosterPowerRatio * actorData.ActorSpecVO.SubBoosterPower);

            // 最大速度制限
            if ((actorData.ActorSpecVO.MaxSpeed * actorData.ActorSpecVO.MaxSpeed) < nextMovementVelocity.sqrMagnitude)
            {
                nextMovementVelocity *= actorData.ActorSpecVO.MaxSpeed / prevMovementVelocity.magnitude;
            }

            actorData.MovingModule.SetMovementVelocity(nextMovementVelocity * deltaTime);
            actorData.MovingModule.SetQuaternionVelocityRHS(
                Quaternion.Euler(new Vector3(
                    actorData.ActorStateData.PitchBoosterPowerRatio * actorData.ActorSpecVO.PitchRotatePower,
                    actorData.ActorStateData.YawBoosterPowerRatio * actorData.ActorSpecVO.YawRotatePower,
                    actorData.ActorStateData.RollBoosterPowerRatio * actorData.ActorSpecVO.RollRotatePower) * deltaTime));
        }

        void UpdateInteract(float deltaTime)
        {
            if (actorData.ActorStateData.InteractOrder == null)
            {
                actorData.ActorStateData.CurrentInteractingTime = 0;
                return;
            }

            if (actorData.ActorStateData.InteractOrder.IsInteractionRange(actorData))
            {
                actorData.ActorStateData.CurrentInteractingTime += deltaTime;
            }
            else
            {
                actorData.ActorStateData.CurrentInteractingTime = 0;
                return;
            }

            if (actorData.ActorStateData.CurrentInteractingTime < actorData.ActorStateData.InteractOrder.InteractTime)
            {
                return;
            }

            // インタラクト終了
            switch (actorData.ActorStateData.InteractOrder)
            {
                case ItemInteractData itemInteractData:
                    MessageBus.Instance.ManagerCommandPickItem.Broadcast(actorData.InventoryData, itemInteractData);
                    break;
                case BrokenActorInteractData brokenActorInteractData:
                    throw new NotImplementedException();
                case InventoryInteractData inventoryInteractData:
                    // ユーザー操作待ち 相手のインベントリをUIでOpenする
                    throw new NotImplementedException();
                case AreaInteractData areaInteractData:
                    MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(actorData.InstanceId, areaInteractData);
                    break;
            }

            actorData.ActorStateData.InteractOrder = null;
        }
    }
}
