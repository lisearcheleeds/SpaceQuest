using AloneSpace.UI;
using UnityEngine;

namespace AloneSpace
{
    public class SceneUpdater : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] GameObjectUpdater gameObjectUpdater;
        [SerializeField] AreaAmbientController areaAmbientController;
        
        [SerializeField] CameraController cameraController;
        [SerializeField] SpaceMapCameraController spaceMapCameraController;

        SceneInputLayer sceneInputLayer;

        UserController userController = new UserController();

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            sceneInputLayer = new SceneInputLayer(questData.UserData);
            InputLayerController.Instance.PushLayer(sceneInputLayer);

            uiManager.Initialize(questData);
            gameObjectUpdater.Initialize(questData);
            areaAmbientController.Initialize(questData);
            cameraController.Initialize(questData);
            spaceMapCameraController.Initialize(questData);

            userController.Initialize(questData.UserData);
        }

        public void Finalize()
        {
            InputLayerController.Instance.PopLayer(sceneInputLayer);

            uiManager.Finalize();
            gameObjectUpdater.Finalize();
            areaAmbientController.Finalize();
            cameraController.Finalize();
            spaceMapCameraController.Finalize();

            userController.Finalize();
        }

        public void OnUpdate(float deltaTime)
        {
            if (questData.UserData?.PlayerData == null)
            {
                return;
            }

            userController.OnUpdate();

            uiManager.OnUpdate();
            gameObjectUpdater.OnUpdate(deltaTime);
            areaAmbientController.OnUpdate();
            cameraController.OnUpdate();
            spaceMapCameraController.OnUpdate();
        }

        public void OnLateUpdate()
        {
        }
    }
}
