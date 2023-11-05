using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] ActorView actorView;
        [SerializeField] TargetView targetView;
        [SerializeField] MenuView menuView;

        public void Initialize(QuestData questData)
        {
            actorView.Initialize();
            targetView.Initialize();
            menuView.Initialize(questData);
        }

        public void Finalize()
        {
            actorView.Finalize();
            targetView.Finalize();
            menuView.Finalize();
        }

        public void OnUpdate()
        {
            actorView.OnUpdate();
            targetView.OnUpdate();
            menuView.OnUpdate();
        }
    }
}
