using UnityEngine;

namespace AloneSpace
{
    public class WeaponEffectUpdater : IUpdater
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
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
    }
}