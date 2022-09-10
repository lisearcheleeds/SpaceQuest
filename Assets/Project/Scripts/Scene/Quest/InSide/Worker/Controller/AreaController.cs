using System.Collections;
using AloneSpace.InSide;
using UnityEngine;

namespace AloneSpace
{
    public class AreaController : MonoBehaviour
    {
        [SerializeField] AreaAmbientController areaAmbientController;
        [SerializeField] CameraController cameraController;
        [SerializeField] Transform variableParent;

        ActorList actorList = new ActorList();
        CollisionChecker collisionChecker = new CollisionChecker();
        ThreatChecker threatChecker = new ThreatChecker();
        TargetList targetList = new TargetList();
        InteractList interactList = new InteractList();
        WeaponController weaponController = new WeaponController();
        
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            areaAmbientController.Initialize();
            cameraController.Initialize(questData);
            
            actorList.Initialize(variableParent, this);
            collisionChecker.Initialize();
            threatChecker.Initialize();
            targetList.Initialize();
            interactList.Initialize();
            weaponController.Initialize();
        }

        public void Finalize()
        {
            areaAmbientController.Finalize();
            cameraController.Finalize();
            
            actorList.Finalize();
            collisionChecker.Finalize();
            threatChecker.Finalize();
            targetList.Finalize();
            interactList.Finalize();
            weaponController.Finalize();
        }

        public IEnumerator LoadArea(int nextAreaIndex)
        {
            yield return areaAmbientController.LoadArea(questData);
            yield return actorList.LoadArea(questData);
            yield return interactList.LoadArea(questData, nextAreaIndex);
        }

        public void OnLoadedArea()
        {
            actorList.OnLoadedArea();
            targetList.OnLoadedArea();
            interactList.OnLoadedArea();
        }

        void LateUpdate()
        {
            actorList.LateUpdate();
            interactList.LateUpdate();
            collisionChecker.LateUpdate();
            threatChecker.LateUpdate();
            targetList.LateUpdate();
        }
    }
}
