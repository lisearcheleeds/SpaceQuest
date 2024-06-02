using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace.UI
{
    public class DamageView : MonoBehaviour
    {
        [SerializeField] RectTransform damageViewCellParent;
        [SerializeField] DamageViewCell damageViewCellPrefab;

        List<DamageViewCell> cellCache = new List<DamageViewCell>();

        Guid userInstanceId;
        
        public void Initialize()
        {
            MessageBus.Instance.Temp.NoticeDamageEventData.AddListener(NoticeDamageEventData);
            MessageBus.Instance.User.SetPlayer.AddListener(SetPlayer);
        }

        public void Finalize()
        {
            MessageBus.Instance.Temp.NoticeDamageEventData.RemoveListener(NoticeDamageEventData);
            MessageBus.Instance.User.SetPlayer.RemoveListener(SetPlayer);
        }

        public void OnUpdate()
        {
        }

        void NoticeDamageEventData(DamageEventData damageEventData)
        {
            if (damageEventData.WeaponEffectData.PlayerInstanceId != userInstanceId)
            {
                return;
            }

            var cell = cellCache.FirstOrDefault(cell => !cell.IsActive);

            if (cell == null)
            {
                cell = Instantiate(damageViewCellPrefab, damageViewCellParent);
                cellCache.Add(cell);
            }

            var canvasPoint = MessageBus.Instance.Util.GetWorldToCanvasPoint.Unicast(
                CameraType.NearCamera,
                damageEventData.DamagedActorData.Position,
                damageViewCellParent);

            var damagePosition = (canvasPoint ?? Vector3.zero) + Random.insideUnitSphere * 20.0f;
            
            cell.ApplyDamage(damageEventData.EffectedDamageValue, damagePosition);
        }

        void SetPlayer(PlayerData playerData)
        {
            userInstanceId = playerData.InstanceId;
        }
    }
}
