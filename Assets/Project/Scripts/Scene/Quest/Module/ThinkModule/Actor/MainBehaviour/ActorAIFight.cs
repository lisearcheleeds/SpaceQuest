using System.Linq;
using UnityEngine;
// ReSharper disable All

namespace AloneSpace
{
    public class ActorAIFight : IActorAIState
    {
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            // ターゲット確認
            if (actorData.ActorStateData.MainTarget == null)
            {
                return ActorAIState.Check;
            }
 
            MessageBus.Instance.Actor.SetLookAtDirection.Broadcast(
                actorData.InstanceId,
                (actorData.ActorStateData.MainTarget.Position - actorData.Position).normalized);
            
            MoveUpdate(actorData);
            WeaponUpdate(actorData);

            return ActorAIState.Fight;
        }

        void MoveUpdate(ActorData actorData)
        {
            // 移動
            var upDot = Vector3.Dot(actorData.ActorStateData.LookAtDirection, actorData.Rotation * Vector3.up);
            var rightDot = Vector3.Dot(actorData.ActorStateData.LookAtDirection, actorData.Rotation * Vector3.right);
            var forwardDot = Vector3.Dot(actorData.ActorStateData.LookAtDirection, actorData.Rotation * Vector3.forward);
            
            // ピッチ量
            var pitchValue = upDot * -4.0f;

            // ロール量
            var rollValue = -0.8f < upDot ? rightDot * -4.0f : rightDot * 4.0f;

            // ヨー量
            var yawValue = rightDot * 4.0f;
            
            if (0.9f < forwardDot)
            {
                // おおよその方向が合致していたら上方向を合わせるRollに切り替えてロールとヨーだけで調整する
                var rollRight = Vector3.Dot(Quaternion.identity * Vector3.up, actorData.Rotation * Vector3.right);
                rollValue = rollRight * -4.0f;
            }

            // FIXME: 毎フレーム減衰するのにThinkのタイミングでしかBroadcastしないので旋回性能が低い
            MessageBus.Instance.Actor.PitchBoosterPowerRatio.Broadcast(actorData.InstanceId, Mathf.Clamp(pitchValue, -1.0f, 1.0f));
            MessageBus.Instance.Actor.YawBoosterPowerRatio.Broadcast(actorData.InstanceId, Mathf.Clamp(yawValue, -1.0f, 1.0f));
            MessageBus.Instance.Actor.RollBoosterPowerRatio.Broadcast(actorData.InstanceId, Mathf.Clamp(rollValue, -1.0f, 1.0f));
            
            MessageBus.Instance.Actor.ForwardBoosterPowerRatio.Broadcast(actorData.InstanceId, 1.0f);
        }

        void WeaponUpdate(ActorData actorData)
        {
            // 武器
            // TODO もうちょっと頭使って二重ループにならないようにする
            for (var i = 0; i < actorData.WeaponDataGroup.Length; i++)
            {
                var targetIndex = (actorData.ActorStateData.CurrentWeaponGroupIndex + i) % actorData.WeaponDataGroup.Length;
                var isUseableGroup = actorData.WeaponDataGroup[targetIndex].Any(weaponId =>
                {
                    return !actorData.WeaponData[weaponId].IsInfinityResource
                           && actorData.WeaponData[weaponId].WeaponStateData.IsExecutable
                           && actorData.WeaponData[weaponId].WeaponStateData.IsTargetInAngle
                           && actorData.WeaponData[weaponId].WeaponStateData.IsTargetInRange;
                });

                if (isUseableGroup)
                {
                    MessageBus.Instance.Actor.SetCurrentWeaponGroupIndex.Broadcast(actorData.InstanceId, targetIndex);
                    MessageBus.Instance.Actor.SetWeaponExecute.Broadcast(actorData.InstanceId, true);
                    return;
                }
            }
            
            for (var i = 0; i < actorData.WeaponDataGroup.Length; i++)
            {
                var targetIndex = (actorData.ActorStateData.CurrentWeaponGroupIndex + i) % actorData.WeaponDataGroup.Length;
                var isUseableGroup = actorData.WeaponDataGroup[targetIndex].Any(weaponId =>
                {
                    return actorData.WeaponData[weaponId].WeaponStateData.IsExecutable
                           && actorData.WeaponData[weaponId].WeaponStateData.IsTargetInAngle
                           && actorData.WeaponData[weaponId].WeaponStateData.IsTargetInRange;
                });

                if (isUseableGroup)
                {
                    MessageBus.Instance.Actor.SetCurrentWeaponGroupIndex.Broadcast(actorData.InstanceId, targetIndex);
                    MessageBus.Instance.Actor.SetWeaponExecute.Broadcast(actorData.InstanceId, true);
                    return;
                }
            }

            MessageBus.Instance.Actor.SetWeaponExecute.Broadcast(actorData.InstanceId, false);
        }
    }
}
