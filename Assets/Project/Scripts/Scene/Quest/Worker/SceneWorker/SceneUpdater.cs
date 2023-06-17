using UnityEngine;

namespace AloneSpace
{
    public class SceneUpdater : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] GameObjectUpdater gameObjectUpdater;
        [SerializeField] AreaAmbientController areaAmbientController;
        [SerializeField] CameraController cameraController;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            uiManager.Initialize(questData);
            gameObjectUpdater.Initialize(questData);
            areaAmbientController.Initialize(questData);
            cameraController.Initialize(questData);
        }

        public void Finalize()
        {
            uiManager.Finalize();
            gameObjectUpdater.Finalize();
            areaAmbientController.Finalize();
            cameraController.Finalize();
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (questData.UserData?.PlayerData == null)
            {
                return;
            }

            uiManager.OnLateUpdate();
            gameObjectUpdater.OnLateUpdate(deltaTime);
            areaAmbientController.OnLateUpdate();
            cameraController.OnLateUpdate();
        }
    }
}
