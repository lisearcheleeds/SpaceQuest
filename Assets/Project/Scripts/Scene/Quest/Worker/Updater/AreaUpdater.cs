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
        
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            areaAmbientController.Initialize(questData);
            cameraController.Initialize(questData);
            
            actorObjectUpdater.Initialize(questData, variableParent, this);
            interactObjectUpdater.Initialize(questData);
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
            cameraController.OnLateUpdate();
            
            actorObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
        }

        public IEnumerator LoadArea(AreaData areaData)
        {
            yield return areaAmbientController.LoadArea(areaData);
            
            yield return actorObjectUpdater.LoadArea(areaData);
            yield return interactObjectUpdater.LoadArea(areaData);
        }
    }
}
