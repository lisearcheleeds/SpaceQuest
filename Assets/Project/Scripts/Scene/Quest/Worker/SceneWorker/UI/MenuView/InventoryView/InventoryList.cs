using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryList : MonoBehaviour
    {
        public StandardStashView StandardStashView => stashView;

        [SerializeField] StandardStashView stashView;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            var data = new VariableInventoryViewData(6, 6);
            stashView.Apply(data);
        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
        }
    }
}
