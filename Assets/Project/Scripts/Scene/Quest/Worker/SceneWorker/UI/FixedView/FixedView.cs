using UnityEngine;
using UnityEngine.Serialization;

namespace AloneSpace.UI
{
    public class FixedView : MonoBehaviour
    {
        [SerializeField] MenuView menuView;
        
        [SerializeField] EnduranceView enduranceView;
        [SerializeField] WeaponDataListView weaponDataListView;
        [SerializeField] ActorOperationModeView actorOperationModeView;
        [FormerlySerializedAs("areaView")] [SerializeField] SpaceMapView spaceMapView;

        public void Initialize(QuestData questData)
        {
            menuView.Initialize(questData);
            
            enduranceView.Initialize();
            weaponDataListView.Initialize();
            actorOperationModeView.Initialize();
            spaceMapView.Initialize(questData);
        }

        public void Finalize()
        {
            menuView.Finalize();
            
            enduranceView.Finalize();
            weaponDataListView.Finalize();
            actorOperationModeView.Finalize();
            spaceMapView.Finalize();
        }

        public void OnUpdate()
        {
            menuView.OnUpdate();
            
            enduranceView.OnUpdate();
            weaponDataListView.OnUpdate();
            actorOperationModeView.OnUpdate();
            spaceMapView.OnUpdate();
        }
    }
}