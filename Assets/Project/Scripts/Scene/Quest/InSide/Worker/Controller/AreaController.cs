using System.Collections;
using AloneSpace.InSide;
using UnityEngine;

namespace AloneSpace
{
    public class AreaController : MonoBehaviour
    {
        [SerializeField] AreaAmbientController areaAmbientController;
        [SerializeField] CameraController cameraController;

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
            
            actorList.Initialize();
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

        public void ResetArea()
        {
            areaAmbientController.ResetArea();
            actorList.ResetArea();
            interactList.ResetArea();
            
            GameObjectCache.Instance.AllRelease();
        }

        public IEnumerator LoadArea(int nextAreaIndex)
        {
            yield return areaAmbientController.LoadArea(questData, nextAreaIndex);
            yield return actorList.LoadArea(questData, nextAreaIndex, this);
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
