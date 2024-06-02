using UnityEngine;

namespace AloneSpace.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("FLOAT VIEW")]
        [SerializeField] TargetView targetView;
        [SerializeField] RadarView radarView;
        [SerializeField] ReticleView reticleView;
        [SerializeField] DamageView damageView;
        
        [Header("FIXED VIEW")]
        [SerializeField] EnduranceView enduranceView;
        [SerializeField] WeaponDataListView weaponDataListView;
        [SerializeField] ActorOperationModeView actorOperationModeView;
        
        [Header("FULL SCREEN")]
        [SerializeField] MenuView menuView;
        [SerializeField] SpaceMapView spaceMapView;
        
        [Header("OVERLAY VIEW")]
        [SerializeField] InventoryLogView inventoryLogView;
        [SerializeField] ContentQuickView contentQuickView;

        [SerializeField] GameObject spaceViewObject;
        
        public void Initialize(QuestData questData)
        {
            targetView.Initialize();
            radarView.Initialize(questData);
            reticleView.Initialize(questData);
            damageView.Initialize();
            
            menuView.Initialize(questData);
            enduranceView.Initialize();
            weaponDataListView.Initialize();
            actorOperationModeView.Initialize();
            spaceMapView.Initialize(questData);
            
            inventoryLogView.Initialize();
            contentQuickView.Initialize();
            
            MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.AddListener(UserInputOpenSpaceMapView);
            MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.AddListener(UserInputCloseSpaceMapView);
            MessageBus.Instance.UserInput.UserInputOpenMenu.AddListener(UserInputOpenMenu);
            MessageBus.Instance.UserInput.UserInputCloseMenu.AddListener(UserInputCloseMenu);
        }

        public void Finalize()
        {
            targetView.Finalize();
            radarView.Finalize();
            reticleView.Finalize();
            damageView.Finalize();
            
            menuView.Finalize();
            enduranceView.Finalize();
            weaponDataListView.Finalize();
            actorOperationModeView.Finalize();
            spaceMapView.Finalize();
            
            inventoryLogView.Finalize();
            contentQuickView.Finalize();
            
            MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.RemoveListener(UserInputOpenSpaceMapView);
            MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.RemoveListener(UserInputCloseSpaceMapView);
            MessageBus.Instance.UserInput.UserInputOpenMenu.RemoveListener(UserInputOpenMenu);
            MessageBus.Instance.UserInput.UserInputCloseMenu.RemoveListener(UserInputCloseMenu);
        }

        public void OnUpdate()
        {
            targetView.OnUpdate();
            radarView.OnUpdate();
            reticleView.OnUpdate();
            damageView.OnUpdate();
            
            menuView.OnUpdate();
            enduranceView.OnUpdate();
            weaponDataListView.OnUpdate();
            actorOperationModeView.OnUpdate();
            spaceMapView.OnUpdate();
            
            inventoryLogView.OnUpdate();
            contentQuickView.OnUpdate();
        }
        
        void UserInputOpenSpaceMapView()
        {
            spaceViewObject.SetActive(false);
        }

        void UserInputCloseSpaceMapView()
        {
            spaceViewObject.SetActive(true);
        }

        void UserInputOpenMenu()
        {
            spaceViewObject.SetActive(false);
        }

        void UserInputCloseMenu()
        {
            spaceViewObject.SetActive(true);
        }
    }
}
