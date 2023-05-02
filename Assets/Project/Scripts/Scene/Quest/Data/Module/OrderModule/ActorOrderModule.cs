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
            foreach (var weaponData in actorData.WeaponData.Values)
            {
                weaponData.SetLookAtDirection(actorData.ActorStateData.LookAtDirection);
                weaponData.SetTargetData(actorData.ActorStateData.MainTarget);
            }
            
            // 移動チェック
            if (actorData.ActorStateData.ActorMode == ActorMode.Warp)
            {
                // ワープ開始直後まだAreaに居る時は加速
                if (actorData.AreaId.HasValue && actorData.AreaId != actorData.ActorStateData.MoveTarget.AreaId)
                {
                    var areaData = MessageBus.Instance.UtilGetAreaData.Unicast(actorData.AreaId.Value);
                    var moveTargetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(actorData.ActorStateData.MoveTarget.AreaId.Value);

                    // Area内の移動
                    var offset = moveTargetAreaData.StarSystemPosition - areaData.StarSystemPosition;
                    actorData.MovingModule.InertiaTensor += offset.normalized * deltaTime * 2.0f;

                    // 範囲外になったらAreaから脱出 一旦1000.0f
                    if (actorData.Position.sqrMagnitude > 1000.0f * 1000.0f)
                    {
                        MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(actorData, null);
                        actorData.SetPosition(areaData.StarSystemPosition);
                        actorData.MovingModule.InertiaTensor = Vector3.zero;
                    }
                }
                else if (!actorData.AreaId.HasValue)
                {
                    // ワープ中Areaの外に居る時の座標
                    var moveTargetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(actorData.ActorStateData.MoveTarget.AreaId.Value);
                    
                    // Area外の移動
                    var offset = moveTargetAreaData.StarSystemPosition - actorData.Position;
                    actorData.MovingModule.InertiaTensor = offset.normalized * deltaTime * 2.0f * 30.0f;

                    // 目的地に近くなったらAreaに入る
                    if (offset.sqrMagnitude < actorData.MovingModule.InertiaTensor.sqrMagnitude)
                    {
                        // FIXME: 移動先ちゃんと考える
                        MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(actorData, actorData.ActorStateData.MoveTarget.AreaId);
                        actorData.SetPosition(actorData.ActorStateData.MoveTarget.Position + actorData.ActorStateData.MoveTarget.Position.normalized * 1000.0f);
                        actorData.MovingModule.InertiaTensor = Vector3.zero;
                    }
                }
                else if (actorData.AreaId.HasValue && actorData.AreaId == actorData.ActorStateData.MoveTarget.AreaId)
                {
                    // FIXME: 移動の計算式ちゃんと考える
                    // ワープ終了目的のAreaに居る時は減速
                    // Area内の移動
                    var offset = actorData.ActorStateData.MoveTarget.Position - actorData.Position;
                    actorData.MovingModule.InertiaTensor = offset.normalized * deltaTime * 2.0f * 30.0f;

                    // 目的地に近くなったらWarp終了
                    if (offset.sqrMagnitude < actorData.MovingModule.InertiaTensor.sqrMagnitude)
                    {
                        actorData.SetPosition(actorData.ActorStateData.MoveTarget.Position);
                        actorData.MovingModule.InertiaTensor = Vector3.zero;
                        MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(actorData, null); 
                    }
                }
            }
            else
            {
                actorData.MovingModule.InertiaTensor += actorData.Rotation * new Vector3(
                    (actorData.ActorStateData.RightBoosterPowerRatio - actorData.ActorStateData.LeftBoosterPowerRatio) * actorData.ActorSpecData.SubBoosterPower,
                    (actorData.ActorStateData.TopBoosterPowerRatio - actorData.ActorStateData.BottomBoosterPowerRatio) * actorData.ActorSpecData.SubBoosterPower,
                    actorData.ActorStateData.ForwardBoosterPowerRatio * actorData.ActorSpecData.MainBoosterPower + -actorData.ActorStateData.BackBoosterPowerRatio * actorData.ActorSpecData.SubBoosterPower);

                if ((actorData.ActorSpecData.MaxSpeed * actorData.ActorSpecData.MaxSpeed) < actorData.MovingModule.InertiaTensor.sqrMagnitude)
                {
                    // 最大速度制限
                    actorData.MovingModule.InertiaTensor *= actorData.ActorSpecData.MaxSpeed / actorData.MovingModule.InertiaTensor.sqrMagnitude;
                }

                actorData.MovingModule.InertiaTensorRotation = Quaternion.Euler(
                    actorData.ActorStateData.PitchBoosterPowerRatio * actorData.ActorSpecData.PitchBoosterPower,
                    actorData.ActorStateData.YawBoosterPowerRatio * actorData.ActorSpecData.YawBoosterPower,
                    actorData.ActorStateData.RollBoosterPowerRatio * actorData.ActorSpecData.RollBoosterPower);
            }
        }
    }
}