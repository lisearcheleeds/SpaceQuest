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

        GameObjectCache gameObjectCache = new GameObjectCache();

        public void Initialize(QuestData questData)
        {
            actorObjectUpdater.Initialize(questData, variableParent, this);
            weaponEffectObjectUpdater.Initialize(questData);
            interactObjectUpdater.Initialize(this);
            graphicEffectObjectUpdater.Initialize();

            gameObjectCache.Initialize(variableParent);
        }

        public void Finalize()
        {
            actorObjectUpdater.Finalize();
            weaponEffectObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
            graphicEffectObjectUpdater.Finalize();

            gameObjectCache.Finalize();
        }

        public void OnUpdate(float deltaTime)
        {
            actorObjectUpdater.OnUpdate();
            weaponEffectObjectUpdater.OnUpdate();
            interactObjectUpdater.OnUpdate();
            graphicEffectObjectUpdater.OnUpdate(deltaTime);
        }
    }
}
