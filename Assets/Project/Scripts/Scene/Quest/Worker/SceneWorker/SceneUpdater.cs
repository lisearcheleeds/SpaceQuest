using UnityEngine;

namespace AloneSpace
{
    public class SceneUpdater : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] GameObjectUpdater gameObjectUpdater;
        [SerializeField] AreaAmbientController areaAmbientController;
        [SerializeField] CameraController cameraController;

        KeyBindController keyBindController = new KeyBindController();
        MouseBindController mouseBindController = new MouseBindController();
        UserController userController = new UserController();

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            uiManager.Initialize(questData);
            gameObjectUpdater.Initialize(questData);
            areaAmbientController.Initialize(questData);
            cameraController.Initialize(questData);

            keyBindController.Initialize(questData.UserData);
            mouseBindController.Initialize(questData.UserData);
            userController.Initialize(questData.UserData);
        }

        public void Finalize()
        {
            uiManager.Finalize();
            gameObjectUpdater.Finalize();
            areaAmbientController.Finalize();
            cameraController.Finalize();

            keyBindController.Finalize();
            mouseBindController.Finalize();
            userController.Finalize();
        }

        public void OnUpdate(float deltaTime)
        {
            if (questData.UserData?.PlayerData == null)
            {
                return;
            }

            keyBindController.OnUpdate();
            mouseBindController.OnUpdate();
            userController.OnUpdate();

            uiManager.OnUpdate();
            gameObjectUpdater.OnUpdate(deltaTime);
            areaAmbientController.OnUpdate();
            cameraController.OnUpdate();
        }

        public void OnLateUpdate()
        {
        }
    }
}
