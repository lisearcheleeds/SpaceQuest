using UnityEngine;

namespace AloneSpace
{
    public class MenuView : MonoBehaviour
    {
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

        void UpdateView()
        {
            statusView.gameObject.SetActive(currentMenuElement == MenuElement.StatusView);
            inventoryView.gameObject.SetActive(currentMenuElement == MenuElement.InventoryView);
            playerView.gameObject.SetActive(currentMenuElement == MenuElement.PlayerView);
            areaView.gameObject.SetActive(currentMenuElement == MenuElement.AreaView);
            mapView.gameObject.SetActive(currentMenuElement == MenuElement.MapView);

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

    }
}
