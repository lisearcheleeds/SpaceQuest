using System;
using UnityEngine;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace.UI
{
    public class ItemThumbnail : CacheableGameObject
    {
        [SerializeField] RawImage iconImage;
        [SerializeField] Text text;
        [SerializeField] Text optionText;
        [SerializeField] GameObject background;

        [SerializeField] StandardButton button;

        [SerializeField] float iconBaseSize = 100.0f;

        ItemData itemData;

        Action onClick;
        Action onClickOption;
        Action onEnter;
        Action onExit;

        bool isInitialized;

        bool showText;
        bool showOptionText;
        bool showBackground;

        bool grayOut;

        bool showQuickViewOnEnter;

        public enum LayoutMode
        {
            Default,
            Manual,
            NoText,
        }

        public enum PointerActionMode
        {
            Default,
            Manual,
        }

        public enum VisualStateMode
        {
            Default,
            Manual,
            GrayOut,
        }

        public void Apply(
            ItemVO itemVO,
            LayoutMode layoutMode = LayoutMode.Default,
            PointerActionMode pointerActionMode = PointerActionMode.Default,
            VisualStateMode visualStateMode = VisualStateMode.Default)
        {
        }

        public void Apply(
            ItemData itemData,
            LayoutMode layoutMode = LayoutMode.Default,
            PointerActionMode pointerActionMode = PointerActionMode.Default,
            VisualStateMode visualStateMode = VisualStateMode.Default)
        {
            InitializeIfNeed();

            this.itemData = itemData;

            iconImage.gameObject.SetActive(false);
            if (itemData == null)
            {
                text.text = "";
                optionText.text = "";
                iconImage.texture = null;
                ReSize();

                SetLayout(layoutMode);
                SetPointerAction(pointerActionMode);
                SetVisualState(visualStateMode);
            }
            else
            {
                text.text = itemData.ItemVO.Name;
                optionText.text = $"x{itemData.Amount}";
                SetLayout(layoutMode);
                SetPointerAction(pointerActionMode);
                SetVisualState(visualStateMode);

                AssetLoader.Instance.StartLoadAsyncTextureCache(itemData.ItemVO.ImageAsset, tex =>
                {
                    iconImage.gameObject.SetActive(true);
                    iconImage.texture = tex;
                    ReSize();
                });
            }
        }

        public void ReSize()
        {
            var tex = iconImage.texture;
            if (tex == null)
            {
                iconImage.rectTransform.sizeDelta = new Vector2(iconBaseSize, iconBaseSize);
            }
            else
            {
                iconImage.rectTransform.sizeDelta =
                    tex.texelSize.x < tex.texelSize.y
                        ? new Vector2(iconBaseSize * (tex.texelSize.x / tex.texelSize.y), iconBaseSize)
                        : new Vector2(iconBaseSize, iconBaseSize * (tex.texelSize.y / tex.texelSize.x));
            }
        }

        public void SetLayout(LayoutMode layoutMode)
        {
            switch (layoutMode)
            {
                case LayoutMode.Default:
                    SetLayout(true, true, true);
                    break;
                case LayoutMode.Manual:
                    // None
                    break;
                case LayoutMode.NoText:
                    SetLayout(false, false, true);
                    break;
            }
        }

        public void SetLayout(bool showText, bool showOptionText, bool showBackground)
        {
            this.showText = showText;
            this.showOptionText = showOptionText;
            this.showBackground = showBackground;
            ReLayout();
        }

        public void ReLayout()
        {
            text.gameObject.SetActive(showText);
            optionText.gameObject.SetActive(showOptionText);
            background.gameObject.SetActive(showBackground);
        }

        public void SetPointerAction(bool showQuickViewOnEnter)
        {
            this.showQuickViewOnEnter = showQuickViewOnEnter;
        }

        public void SetPointerAction(PointerActionMode pointerActionMode)
        {
            switch (pointerActionMode)
            {
                case PointerActionMode.Default:
                    SetPointerAction(true);
                    break;
                case PointerActionMode.Manual:
                    // None
                    break;
            }
        }

        public void SetVisualState(VisualStateMode visualStateMode)
        {
            switch (visualStateMode)
            {
                case VisualStateMode.Default:
                    iconImage.color = Color.white;
                    break;
                case VisualStateMode.Manual:
                    // None
                    break;
                case VisualStateMode.GrayOut:
                    iconImage.color = Color.gray;
                    break;
            }
        }

        public void SetCallBack(Action onClick, Action onClickOption, Action onEnter, Action onExit)
        {
            this.onClick = onClick;
            this.onClickOption = onClickOption;
            this.onEnter = onEnter;
            this.onExit = onExit;
        }

        protected override void OnRelease()
        {
            SetCallBack(null, null, null, null);
            SetLayout(LayoutMode.Default);
            SetPointerAction(PointerActionMode.Default);
        }

        void InitializeIfNeed()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;
            button.SetCallback(OnClick, OnClickOption, OnEnter, OnExit);
        }

        void OnClick()
        {
            onClick?.Invoke();
        }

        void OnClickOption()
        {
            onClickOption?.Invoke();
        }

        void OnEnter()
        {
            if (showQuickViewOnEnter)
            {
                MessageBus.Instance.UserInput.UserInputOpenContentQuickView.Broadcast(itemData, () => gameObject != null && gameObject.activeInHierarchy, true);
            }

            onEnter?.Invoke();
        }

        void OnExit()
        {
            if (showQuickViewOnEnter)
            {
                MessageBus.Instance.UserInput.UserInputCloseContentQuickView.Broadcast();
            }

            onExit?.Invoke();
        }
    }
}
