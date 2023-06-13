using UnityEngine;

namespace AloneSpace
{
    public class GameObjectUpdater : MonoBehaviour
    {
        [SerializeField] Transform variableParent;
        [SerializeField] Transform cacheParent;

        ActorObjectUpdater actorObjectUpdater = new ActorObjectUpdater();
        WeaponEffectObjectUpdater weaponEffectObjectUpdater = new WeaponEffectObjectUpdater();
        InteractObjectUpdater interactObjectUpdater = new InteractObjectUpdater();
        GraphicEffectObjectUpdater graphicEffectObjectUpdater = new GraphicEffectObjectUpdater();

        GameObjectCache gameObjectCache = new GameObjectCache();

        public void Initialize(QuestData questData)
        {
            actorObjectUpdater.Initialize(questData, variableParent, this);
            weaponEffectObjectUpdater.Initialize(questData);
            interactObjectUpdater.Initialize(questData, this);
            graphicEffectObjectUpdater.Initialize(questData);

            gameObjectCache.Initialize(variableParent, cacheParent);
        }

        public void Finalize()
        {
            actorObjectUpdater.Finalize();
            weaponEffectObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
            graphicEffectObjectUpdater.Finalize();

            gameObjectCache.Finalize();
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
