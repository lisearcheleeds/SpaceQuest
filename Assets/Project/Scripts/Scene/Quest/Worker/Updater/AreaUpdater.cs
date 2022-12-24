using System.Collections;
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

        public IEnumerator LoadArea()
        {
            yield return areaAmbientController.LoadArea();
            yield return actorObjectUpdater.LoadArea();
            yield return interactObjectUpdater.LoadArea();
        }

        public void OnLateUpdate()
        {
            cameraController.OnLateUpdate();
            actorObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
        }
    }
}
