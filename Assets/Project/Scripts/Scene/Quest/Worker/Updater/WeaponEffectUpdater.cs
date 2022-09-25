using UnityEngine;

namespace AloneSpace
{
    public class WeaponEffectUpdater : IUpdater
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.AddWeaponEffectData.AddListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.AddListener(RemoveWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.AddWeaponEffectData.RemoveListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.RemoveListener(RemoveWeaponEffectData);
        }

        public void OnLateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            foreach (var weaponEffectData in questData.WeaponEffectData)
            {
                weaponEffectData.OnLateUpdate(deltaTime);
            }
        }

        void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            MessageBus.Instance.SendThreat.Broadcast(weaponEffectData, true);
            MessageBus.Instance.SendIntuition.Broadcast(weaponEffectData, true);
            MessageBus.Instance.SendCollision.Broadcast(weaponEffectData, true);
        }

        void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            MessageBus.Instance.SendThreat.Broadcast(weaponEffectData, false);
            MessageBus.Instance.SendIntuition.Broadcast(weaponEffectData, false);
            MessageBus.Instance.SendCollision.Broadcast(weaponEffectData, false);
        }
    }
}