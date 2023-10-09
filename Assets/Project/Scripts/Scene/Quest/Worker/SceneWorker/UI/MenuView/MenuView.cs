using AloneSpace.Common;
using UnityEngine;

namespace AloneSpace
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] TabController tabController;

        [SerializeField] StatusView statusView;
        [SerializeField] InventoryView inventoryView;
        [SerializeField] PlayerView playerView;
        [SerializeField] AreaView areaView;
        [SerializeField] MapView mapView;

        MenuElement currentMenuElement = MenuElement.StatusView;

        enum MenuElement
        {
            StatusView,
            InventoryView,
            PlayerView,
            AreaView,
            MapView,
        }

        public void Initialize(QuestData questData)
        {
            statusView.Initialize(questData);
            inventoryView.Initialize(questData);
            playerView.Initialize(questData);
            areaView.Initialize(questData);
            mapView.Initialize(questData);

            tabController.SetOnChangeIndexFromButton(OnChangeIndexFromButton);

            MessageBus.Instance.UserInputOpenMenu.AddListener(UserInputOpenMenu);
            MessageBus.Instance.UserInputCloseMenu.AddListener(UserInputCloseMenu);
            MessageBus.Instance.UserInputSwitchMenuStatusView.AddListener(UserInputSwitchMenuStatusView);
            MessageBus.Instance.UserInputSwitchMenuInventoryView.AddListener(UserInputSwitchMenuInventoryView);
            MessageBus.Instance.UserInputSwitchMenuPlayerView.AddListener(UserInputSwitchMenuPlayerView);
            MessageBus.Instance.UserInputSwitchMenuAreaView.AddListener(UserInputSwitchMenuAreaView);
            MessageBus.Instance.UserInputSwitchMenuMapView.AddListener(UserInputSwitchMenuMapView);
        }

        public void Finalize()
        {
            statusView.Finalize();
            inventoryView.Finalize();
            playerView.Finalize();
            areaView.Finalize();
            mapView.Finalize();

            tabController.SetOnChangeIndexFromButton(null);

            MessageBus.Instance.UserInputOpenMenu.RemoveListener(UserInputOpenMenu);
            MessageBus.Instance.UserInputCloseMenu.RemoveListener(UserInputCloseMenu);
            MessageBus.Instance.UserInputSwitchMenuStatusView.RemoveListener(UserInputSwitchMenuStatusView);
            MessageBus.Instance.UserInputSwitchMenuInventoryView.RemoveListener(UserInputSwitchMenuInventoryView);
            MessageBus.Instance.UserInputSwitchMenuPlayerView.RemoveListener(UserInputSwitchMenuPlayerView);
            MessageBus.Instance.UserInputSwitchMenuAreaView.RemoveListener(UserInputSwitchMenuAreaView);
            MessageBus.Instance.UserInputSwitchMenuMapView.RemoveListener(UserInputSwitchMenuMapView);
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
                case MenuElement.AreaView:
                    areaView.OnUpdate();
                    break;
                case MenuElement.MapView:
                    mapView.OnUpdate();
                    break;
            }
        }

        void OnChangeIndexFromButton(int index)
        {
            switch (GetMenuElementFromIndex(index))
            {
                case MenuElement.StatusView:
                    MessageBus.Instance.UserInputSwitchMenuStatusView.Broadcast();
                    break;
                case MenuElement.InventoryView:
                    MessageBus.Instance.UserInputSwitchMenuInventoryView.Broadcast();
                    break;
                case MenuElement.PlayerView:
                    MessageBus.Instance.UserInputSwitchMenuPlayerView.Broadcast();
                    break;
                case MenuElement.AreaView:
                    MessageBus.Instance.UserInputSwitchMenuAreaView.Broadcast();
                    break;
                case MenuElement.MapView:
                    MessageBus.Instance.UserInputSwitchMenuMapView.Broadcast();
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
                case MenuElement.AreaView:
                    areaView.SetDirty();
                    break;
                case MenuElement.MapView:
                    mapView.SetDirty();
                    break;
            }

            OnUpdate();
        }

        void UserInputOpenMenu()
        {
            gameObject.SetActive(true);
            UpdateView();
        }

        void UserInputCloseMenu()
        {
            gameObject.SetActive(false);
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

        void UserInputSwitchMenuAreaView()
        {
            currentMenuElement = MenuElement.AreaView;
            UpdateView();
        }

        void UserInputSwitchMenuMapView()
        {
            currentMenuElement = MenuElement.MapView;
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
