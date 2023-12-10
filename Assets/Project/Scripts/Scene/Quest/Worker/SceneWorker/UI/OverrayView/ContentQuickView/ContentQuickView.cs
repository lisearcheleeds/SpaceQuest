using System;
using UnityEngine;

namespace AloneSpace.UI
{
    public class ContentQuickView : MonoBehaviour
    {
        [SerializeField] ContentQuickViewWindow contentQuickViewWindow;

        IContentQuickViewData contentQuickViewData;
        Func<bool> lifeCheck;
        bool followPointerPosition;

        bool isDirty;

        public void Initialize()
        {
            MessageBus.Instance.UserInputOpenContentQuickView.AddListener(UserInputOpenContentQuickView);
            MessageBus.Instance.UserInputCloseContentQuickView.AddListener(UserInputCloseContentQuickView);

            contentQuickViewWindow.Initialize();
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInputOpenContentQuickView.RemoveListener(UserInputOpenContentQuickView);
            MessageBus.Instance.UserInputCloseContentQuickView.RemoveListener(UserInputCloseContentQuickView);

            contentQuickViewWindow.Finalize();
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                contentQuickViewWindow.Apply(contentQuickViewData, followPointerPosition);
                isDirty = false;
            }

            if (contentQuickViewWindow.gameObject.activeInHierarchy && (!lifeCheck?.Invoke() ?? false))
            {
                MessageBus.Instance.UserInputCloseContentQuickView.Broadcast();
                return;
            }

            contentQuickViewWindow.OnUpdate();
        }

        void UserInputOpenContentQuickView(IContentQuickViewData contentQuickViewData, Func<bool> lifeCheck, bool followPointerPosition)
        {
            this.contentQuickViewData = contentQuickViewData;
            this.lifeCheck = lifeCheck;
            this.followPointerPosition = followPointerPosition;
            isDirty = true;
        }

        void UserInputCloseContentQuickView()
        {
            contentQuickViewData = null;
            lifeCheck = null;
            isDirty = true;
        }
    }
}
