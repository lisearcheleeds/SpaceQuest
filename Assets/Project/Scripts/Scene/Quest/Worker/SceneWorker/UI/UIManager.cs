using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] ActorView actorView;
        [SerializeField] FloatingView floatingView;
        [SerializeField] MenuView menuView;
        [SerializeField] ContentQuickView contentQuickView;

        public void Initialize(QuestData questData)
        {
            actorView.Initialize();
            floatingView.Initialize(questData);
            menuView.Initialize(questData);
            contentQuickView.Initialize();
        }

        public void Finalize()
        {
            actorView.Finalize();
            floatingView.Finalize();
            menuView.Finalize();
            contentQuickView.Finalize();
        }

        public void OnUpdate()
        {
            actorView.OnUpdate();
            floatingView.OnUpdate();
            menuView.OnUpdate();
            contentQuickView.OnUpdate();
        }
    }
}
