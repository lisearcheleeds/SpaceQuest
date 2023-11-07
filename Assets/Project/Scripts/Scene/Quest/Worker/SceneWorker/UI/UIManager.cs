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
        [SerializeField] ContentQuickView contentQuickView;

        public void Initialize(QuestData questData)
        {
            actorView.Initialize();
            targetView.Initialize();
            menuView.Initialize(questData);
            contentQuickView.Initialize();
        }

        public void Finalize()
        {
            actorView.Finalize();
            targetView.Finalize();
            menuView.Finalize();
            contentQuickView.Finalize();
        }

        public void OnUpdate()
        {
            actorView.OnUpdate();
            targetView.OnUpdate();
            menuView.OnUpdate();
            contentQuickView.OnUpdate();
        }
    }
}
