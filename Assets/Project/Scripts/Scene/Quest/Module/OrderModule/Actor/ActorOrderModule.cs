using System;
using System.Collections.Generic;
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
            return actorData.IsReleased;
        }

        void UpdateDamage()
        {
            if (0 < actorData.ActorStateData.EnduranceValue)
            {
                foreach (var currentDamageEventData in actorData.ActorStateData.CurrentDamageEventDataList)
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

            actorData.ActorStateData.CurrentDamageEventDataList.Clear();
        }

        void UpdateTarget()
        {
            if (!actorData.AreaId.HasValue)
            {
                return;
            }

            if (actorData.ActorStateData.MainTarget == null)
            {
                return;
            }

            if (actorData.ActorStateData.MainTarget is ActorData otherActorData)
            {
                if (otherActorData.IsReleased)
                {
                    MessageBus.Instance.ActorCommandSetMainTarget.Broadcast(actorData.InstanceId, null);
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
                actorData.MovingModule.SetMovementVelocity(actorData.MovingModule.MovementVelocity + offset.normalized * 2.0f);

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
                actorData.MovingModule.SetMovementVelocity(offset.normalized * 2.0f * 30.0f);

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
                actorData.MovingModule.SetMovementVelocity(offset.normalized * 2.0f * 30.0f);

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

            // 移動
            // ベースにする場合は1秒単位に戻す
            var movementVelocityMagnitude = actorData.MovingModule.MovementVelocity.magnitude;
            var prevMovementVelocityDelta = Vector3.zero;
            if (movementVelocityMagnitude != 0)
            {
                var prevMovementVelocityAttenuationRatio = Mathf.Max(0, movementVelocityMagnitude - actorData.ActorSpecVO.SpeedAttenuation) / movementVelocityMagnitude;
                var prevMovementVelocity = actorData.MovingModule.MovementVelocity * prevMovementVelocityAttenuationRatio;
                prevMovementVelocityDelta = prevMovementVelocity;
            }

            // 入力を反映
            var nextMovementVelocity = prevMovementVelocityDelta + actorData.Rotation
                * new Vector3(
                    (actorData.ActorStateData.RightBoosterPowerRatio - actorData.ActorStateData.LeftBoosterPowerRatio) * actorData.ActorSpecVO.SubBoosterPower,
                    (actorData.ActorStateData.TopBoosterPowerRatio - actorData.ActorStateData.BottomBoosterPowerRatio) * actorData.ActorSpecVO.SubBoosterPower,
                    actorData.ActorStateData.ForwardBoosterPowerRatio * actorData.ActorSpecVO.MainBoosterPower + -actorData.ActorStateData.BackBoosterPowerRatio * actorData.ActorSpecVO.SubBoosterPower);

            // 最大速度制限
            if ((actorData.ActorSpecVO.MaxSpeed * actorData.ActorSpecVO.MaxSpeed) < nextMovementVelocity.sqrMagnitude)
            {
                nextMovementVelocity *= actorData.ActorSpecVO.MaxSpeed / nextMovementVelocity.magnitude;
            }

            actorData.MovingModule.SetMovementVelocity(nextMovementVelocity);

            // 回転
            actorData.ActorStateData.PitchBoosterPowerRatio *= 1.0f - (actorData.ActorSpecVO.RotateAttenuation / actorData.ActorSpecVO.PitchRotatePower);
            actorData.ActorStateData.YawBoosterPowerRatio *= 1.0f - (actorData.ActorSpecVO.RotateAttenuation / actorData.ActorSpecVO.YawRotatePower);
            actorData.ActorStateData.RollBoosterPowerRatio *= 1.0f - (actorData.ActorSpecVO.RotateAttenuation / actorData.ActorSpecVO.RollRotatePower);
            actorData.MovingModule.SetQuaternionVelocityRHS(
                Quaternion.Euler(new Vector3(
                    actorData.ActorStateData.PitchBoosterPowerRatio * actorData.ActorSpecVO.PitchRotatePower,
                    actorData.ActorStateData.YawBoosterPowerRatio * actorData.ActorSpecVO.YawRotatePower,
                    actorData.ActorStateData.RollBoosterPowerRatio * actorData.ActorSpecVO.RollRotatePower) * deltaTime));
        }

        void UpdateInteract(float deltaTime)
        {
            if (actorData.ActorStateData.InteractOrderStateList.Count == 0)
            {
                return;
            }

            var completeInteractDataList = new List<IInteractData>();
            foreach (var interactOrderState in actorData.ActorStateData.InteractOrderStateList)
            {
                if (interactOrderState.InteractData.IsInteractionRange(actorData))
                {
                    // Interact開始
                    if (interactOrderState.InteractData is ItemInteractData)
                    {
                        // アイテムを引き寄せる
                        interactOrderState.InteractData.MovingModule.SetMovementVelocity(interactOrderState.InteractData.MovingModule.MovementVelocity * 0.99f);
                    }

                    interactOrderState.Time += deltaTime;
                    interactOrderState.InProgress = true;
                    interactOrderState.ProgressRatio = Mathf.Clamp01(interactOrderState.Time / interactOrderState.InteractData.InteractTime);
                }
                else
                {
                    if (interactOrderState.InteractData is ItemInteractData)
                    {
                        interactOrderState.InProgress = true;

                        // アイテムを引き寄せる
                        var distance = (actorData.Position - interactOrderState.InteractData.Position).magnitude;
                        interactOrderState.InteractData.MovingModule.SetMovementVelocity((actorData.Position - interactOrderState.InteractData.Position).normalized * Mathf.Min(distance, 100.0f));
                    }
                    else
                    {
                        interactOrderState.InProgress = false;
                    }

                    interactOrderState.Time = 0;
                    interactOrderState.ProgressRatio = Mathf.Clamp01(interactOrderState.Time / interactOrderState.InteractData.InteractTime);
                }

                if (interactOrderState.Time < interactOrderState.InteractData.InteractTime)
                {
                    continue;
                }

                // インタラクト終了
                completeInteractDataList.Add(interactOrderState.InteractData);
            }

            foreach (var completeInteractData in completeInteractDataList)
            {
                switch (completeInteractData)
                {
                    case ItemInteractData itemInteractData:
                        MessageBus.Instance.ManagerCommandPickItem.Broadcast(actorData.InventoryData, itemInteractData);
                        break;
                    case InventoryInteractData inventoryInteractData:
                        // ユーザー操作待ち 相手のインベントリをUIでOpenする
                        throw new NotImplementedException();
                    case AreaInteractData areaInteractData:
                        MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(actorData.InstanceId, areaInteractData);
                        break;
                }

                MessageBus.Instance.PlayerCommandRemoveInteractOrder.Broadcast(actorData.InstanceId, completeInteractData);
            }
        }
    }
}
