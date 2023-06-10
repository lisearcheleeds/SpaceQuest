using UnityEngine;

namespace AloneSpace
{
    public class GameObjectUpdater : MonoBehaviour
    {
        [SerializeField] Transform variableParent;

        ActorObjectUpdater actorObjectUpdater = new ActorObjectUpdater();
        WeaponEffectObjectUpdater weaponEffectObjectUpdater = new WeaponEffectObjectUpdater();
        InteractObjectUpdater interactObjectUpdater = new InteractObjectUpdater();
        GraphicEffectObjectUpdater graphicEffectObjectUpdater = new GraphicEffectObjectUpdater();

        public void Initialize(QuestData questData)
        {
            actorObjectUpdater.Initialize(questData, variableParent, this);
            weaponEffectObjectUpdater.Initialize(questData, variableParent);
            interactObjectUpdater.Initialize(questData, variableParent, this);
            graphicEffectObjectUpdater.Initialize(questData, variableParent);
        }

        public void Finalize()
        {
            actorObjectUpdater.Finalize();
            weaponEffectObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
            graphicEffectObjectUpdater.Finalize();
        }

        public void OnLateUpdate(float deltaTime)
        {
            actorObjectUpdater.OnLateUpdate();
            weaponEffectObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
            graphicEffectObjectUpdater.OnLateUpdate(deltaTime);
        }
    }
}
