using UnityEngine;

namespace AloneSpace
{
    public class ActorView : MonoBehaviour
    {
        [SerializeField] EnduranceView enduranceView;
        [SerializeField] WeaponDataListView weaponDataListView;
        [SerializeField] UserDataView userDataView;

        public void Initialize()
        {
            enduranceView.Initialize();
            weaponDataListView.Initialize();
            userDataView.Initialize();
        }

        public void Finalize()
        {
            enduranceView.Finalize();
            weaponDataListView.Finalize();
            userDataView.Finalize();
        }

        public void OnUpdate()
        {
            enduranceView.OnUpdate();
            weaponDataListView.OnUpdate();
            userDataView.OnUpdate();
        }
    }
}
