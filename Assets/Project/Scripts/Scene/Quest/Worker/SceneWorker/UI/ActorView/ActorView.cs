using UnityEngine;

namespace AloneSpace
{
    public class ActorView : MonoBehaviour
    {
        [SerializeField] EnduranceView enduranceView;
        [SerializeField] WeaponDataListView weaponDataListView;

        public void Initialize()
        {
            enduranceView.Initialize();
            weaponDataListView.Initialize();
        }

        public void Finalize()
        {
            enduranceView.Finalize();
            weaponDataListView.Finalize();
        }

        public void OnUpdate()
        {
            enduranceView.OnUpdate();
            weaponDataListView.OnUpdate();
        }
    }
}
