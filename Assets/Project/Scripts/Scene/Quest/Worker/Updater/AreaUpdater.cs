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

            areaAmbientController.Initialize();
            cameraController.Initialize(questData);
            
            actorObjectUpdater.Initialize(variableParent, this);
            interactObjectUpdater.Initialize();
        }

        public void Finalize()
        {
            areaAmbientController.Finalize();
            cameraController.Finalize();
            
            actorObjectUpdater.Finalize();
            interactObjectUpdater.Finalize();
        }

        public IEnumerator LoadArea(int nextAreaIndex)
        {
            yield return areaAmbientController.LoadArea(questData);
            yield return actorObjectUpdater.LoadArea(questData);
            yield return interactObjectUpdater.LoadArea(questData, nextAreaIndex);
        }

        public void OnLoadedArea()
        {
            actorObjectUpdater.OnLoadedArea();
            interactObjectUpdater.OnLoadedArea();
        }

        public void OnLateUpdate()
        {
            actorObjectUpdater.OnLateUpdate();
            interactObjectUpdater.OnLateUpdate();
        }
    }
}
