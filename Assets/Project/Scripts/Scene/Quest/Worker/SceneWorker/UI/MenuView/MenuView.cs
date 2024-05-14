using AloneSpace.Common;
using UnityEngine;

namespace AloneSpace.UI
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] TabController tabController;

        [SerializeField] StatusView statusView;
        [SerializeField] InventoryView inventoryView;
        [SerializeField] PlayerView playerView;

        MenuElement currentMenuElement = MenuElement.StatusView;

        MenuInputLayer menuInputLayer;

        public enum MenuElement
        {
            StatusView,
            InventoryView,
            PlayerView,
        }

        public void Initialize(QuestData questData)
        {
            menuInputLayer = new MenuInputLayer(() => currentMenuElement);

            statusView.Initialize(questData);
            inventoryView.Initialize(questData);
            playerView.Initialize(questData);

            tabController.SetOnChangeIndexFromButton(OnChangeIndexFromButton);

            MessageBus.Instance.UserInput.UserInputOpenMenu.AddListener(UserInputOpenMenu);
            MessageBus.Instance.UserInput.UserInputCloseMenu.AddListener(UserInputCloseMenu);
            MessageBus.Instance.UserInput.UserInputSwitchMenuStatusView.AddListener(UserInputSwitchMenuStatusView);
            MessageBus.Instance.UserInput.UserInputSwitchMenuInventoryView.AddListener(UserInputSwitchMenuInventoryView);
            MessageBus.Instance.UserInput.UserInputSwitchMenuPlayerView.AddListener(UserInputSwitchMenuPlayerView);
        }

        public void Finalize()
        {
            statusView.Finalize();
            inventoryView.Finalize();
            playerView.Finalize();

            tabController.SetOnChangeIndexFromButton(null);

            MessageBus.Instance.UserInput.UserInputOpenMenu.RemoveListener(UserInputOpenMenu);
            MessageBus.Instance.UserInput.UserInputCloseMenu.RemoveListener(UserInputCloseMenu);
            MessageBus.Instance.UserInput.UserInputSwitchMenuStatusView.RemoveListener(UserInputSwitchMenuStatusView);
            MessageBus.Instance.UserInput.UserInputSwitchMenuInventoryView.RemoveListener(UserInputSwitchMenuInventoryView);
            MessageBus.Instance.UserInput.UserInputSwitchMenuPlayerView.RemoveListener(UserInputSwitchMenuPlayerView);
        }

        public void OnUpdate()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            switch (currentMenuElement)
            {
                case MenuElement.StatusView:
                    statusView.OnUpdate();
                    break;
                case MenuElement.InventoryView:
                    inventoryView.OnUpdate();
                    break;
                case MenuElement.PlayerView:
                    playerView.OnUpdate();
                    break;
            }
        }

        void OnChangeIndexFromButton(int index)
        {
            switch (GetMenuElementFromIndex(index))
            {
                case MenuElement.StatusView:
                    MessageBus.Instance.UserInput.UserInputSwitchMenuStatusView.Broadcast();
                    break;
                case MenuElement.InventoryView:
                    MessageBus.Instance.UserInput.UserInputSwitchMenuInventoryView.Broadcast();
                    break;
                case MenuElement.PlayerView:
                    MessageBus.Instance.UserInput.UserInputSwitchMenuPlayerView.Broadcast();
                    break;
            }
        }

        void UpdateView()
        {
            tabController.SetIndex(GetIndexFromMenuElement(currentMenuElement));

            switch (currentMenuElement)
            {
                case MenuElement.StatusView:
                    statusView.SetDirty();
                    break;
                case MenuElement.InventoryView:
                    inventoryView.SetDirty();
                    break;
                case MenuElement.PlayerView:
                    playerView.SetDirty();
                    break;
            }

            OnUpdate();
        }

        void UserInputOpenMenu()
        {
            gameObject.SetActive(true);
            UpdateView();

            InputLayerController.Instance.PushLayer(menuInputLayer);
        }

        void UserInputCloseMenu()
        {
            gameObject.SetActive(false);

            InputLayerController.Instance.PopLayer(menuInputLayer);
        }

        void UserInputSwitchMenuStatusView()
        {
            currentMenuElement = MenuElement.StatusView;
            UpdateView();
        }

        void UserInputSwitchMenuInventoryView()
        {
            currentMenuElement = MenuElement.InventoryView;
            UpdateView();
        }

        void UserInputSwitchMenuPlayerView()
        {
            currentMenuElement = MenuElement.PlayerView;
            UpdateView();
        }

        int GetIndexFromMenuElement(MenuElement menuElement)
        {
            return (int)menuElement;
        }

        MenuElement GetMenuElementFromIndex(int index)
        {
            return (MenuElement) index;
        }
    }
}
