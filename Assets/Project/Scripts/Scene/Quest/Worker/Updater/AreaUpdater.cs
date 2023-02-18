using System.Collections;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class AreaUpdater : MonoBehaviour, IUpdater
    {
        [SerializeField] AreaAmbientController areaAmbientController;
        [SerializeField] CameraController cameraController;

        [SerializeField] Transform variableParent;

        ActorObjectUpdater actorObjectUpdater = new ActorObjectUpdater();
        InteractObjectUpdater interactObjectUpdater = new InteractObjectUpdater();
        
        public void Initialize(QuestData questData)
        {
            areaAmbientController.Initialize(questData);
            cameraController.Initialize();
            
            actorObjectUpdater.Initialize(questData, variableParent, this);
            interactObjectUpdater.Initialize(questData, variableParent, this);
        }

        public void Finalize()
        {
            areaAmbientController.Finalize();
            cameraController.Finalize();
            
            actorObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
        }

        public void OnLateUpdate()
        {
            areaAmbientController.OnLateUpdate();
            cameraController.OnLateUpdate();
            
            actorObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
        }

        public void SetObservePlayerQuestData(PlayerQuestData playerQuestData)
        {
            actorObjectUpdater.SetObservePlayerQuestData(playerQuestData);
        }

        public void SetObserveAreaData(AreaData areaData)
        {
            areaAmbientController.SetObserveAreaData(areaData);
            
            actorObjectUpdater.SetObserveAreaData(areaData);
            interactObjectUpdater.SetObserveAreaData(areaData);
        }
    }
}
