using System.Collections;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class AreaUpdater : MonoBehaviour
    {
        [SerializeField] Transform variableParent;

        ActorObjectUpdater actorObjectUpdater = new ActorObjectUpdater();
        InteractObjectUpdater interactObjectUpdater = new InteractObjectUpdater();
        
        public void Initialize(QuestData questData)
        {
            actorObjectUpdater.Initialize(questData, variableParent, this);
            interactObjectUpdater.Initialize(questData, variableParent, this);
        }

        public void Finalize()
        {
            actorObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
        }

        public void OnLateUpdate()
        {
            actorObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
        }

        public void SetObservePlayerQuestData(PlayerQuestData playerQuestData)
        {
            actorObjectUpdater.SetObservePlayerQuestData(playerQuestData);
        }

        public void SetObserveAreaData(AreaData areaData)
        {
            actorObjectUpdater.SetObserveAreaData(areaData);
            interactObjectUpdater.SetObserveAreaData(areaData);
        }
    }
}
