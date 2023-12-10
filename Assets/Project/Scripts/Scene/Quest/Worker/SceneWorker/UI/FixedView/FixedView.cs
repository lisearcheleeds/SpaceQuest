using UnityEngine;

namespace AloneSpace.UI
{
    public class FixedView : MonoBehaviour
    {
        [SerializeField] MenuView menuView;
        
        [SerializeField] EnduranceView enduranceView;
        [SerializeField] WeaponDataListView weaponDataListView;
        [SerializeField] ActorOperationModeView actorOperationModeView;

        public void Initialize(QuestData questData)
        {
            menuView.Initialize(questData);
            
            enduranceView.Initialize();
            weaponDataListView.Initialize();
            actorOperationModeView.Initialize();
        }

        public void Finalize()
        {
            menuView.Finalize();
            
            enduranceView.Finalize();
            weaponDataListView.Finalize();
            actorOperationModeView.Finalize();
        }

        public void OnUpdate()
        {
            menuView.OnUpdate();
            
            enduranceView.OnUpdate();
            weaponDataListView.OnUpdate();
            actorOperationModeView.OnUpdate();
        }
    }
}