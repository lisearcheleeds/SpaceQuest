using System.Collections;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class GameObjectUpdater : MonoBehaviour
    {
        [SerializeField] Transform variableParent;

        ActorObjectUpdater actorObjectUpdater = new ActorObjectUpdater();
        WeaponEffectObjectUpdater weaponEffectObjectUpdater = new WeaponEffectObjectUpdater();
        InteractObjectUpdater interactObjectUpdater = new InteractObjectUpdater();
        
        public void Initialize(QuestData questData)
        {
            actorObjectUpdater.Initialize(questData, variableParent, this);
            weaponEffectObjectUpdater.Initialize(questData, variableParent);
            interactObjectUpdater.Initialize(questData, variableParent, this);
        }

        public void Finalize()
        {
            actorObjectUpdater.Finalize();
            weaponEffectObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
        }

        public void OnLateUpdate()
        {
            actorObjectUpdater.OnLateUpdate();
            weaponEffectObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
        }

        public void SetObservePlayerQuestData(PlayerQuestData playerQuestData)
        {
            actorObjectUpdater.SetObservePlayerQuestData(playerQuestData);
        }

        public void SetObserveAreaData(AreaData areaData)
        {
            actorObjectUpdater.SetObserveAreaData(areaData);
            weaponEffectObjectUpdater.SetObserveAreaData(areaData);
            interactObjectUpdater.SetObserveAreaData(areaData);
        }
    }
}
