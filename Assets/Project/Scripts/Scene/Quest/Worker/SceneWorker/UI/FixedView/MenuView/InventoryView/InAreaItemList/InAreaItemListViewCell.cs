using System;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace.UI
{
    public class InAreaItemListViewCell : FancyScrollRectCell<InAreaItemListViewCell.CellData, InAreaItemListViewCell.CellContext>
    {
        [SerializeField] Image typeLabel;
        [SerializeField] GameObject highLight;

        [SerializeField] ItemThumbnail itemThumbnail;
        [SerializeField] GridCellSizeView gridCellSizeView;

        [SerializeField] RectTransform progressIcon;
        [SerializeField] RectTransform progressGauge;

        [SerializeField] Text text;
        [SerializeField] Text distanceText;
        [SerializeField] StandardButton button;

        CellData cellData;
        float lastUpdateTime;
        bool prevStateExist;
        bool isDirty;

        public class CellData
        {
            public IInteractData InteractData { get; }
            public bool IsSelected { get; }
            public string NameText { get; }

            public Func<IInteractData, InteractOrderState> GetState { get; }
            public Func<IInteractData, string> GetDistanceText { get; }

            public CellData(
                IInteractData interactData,
                bool isSelected,
                Func<IInteractData, InteractOrderState> getState,
                Func<IInteractData, string> getDistanceText)
            {
                InteractData = interactData;
                NameText = interactData.Text;
                IsSelected = isSelected;

                GetState = getState;
                GetDistanceText = getDistanceText;
            }
        }

        public class CellContext : FancyScrollRectContext
        {
            public Action<CellData> OnSelect { get; set; }

            public Action<CellData> OnConfirm { get; set; }
        }

        public override void Initialize()
        {
            base.Initialize();
            button.SetCallback(OnClick, OnClickOption, OnEnter, OnExit);
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;

            text.text = cellData.NameText;
            distanceText.text = cellData.GetDistanceText(cellData.InteractData);

            // 初期化
            progressIcon.gameObject.SetActive(false);
            progressGauge.gameObject.SetActive(false);
            progressGauge.anchorMax = Vector2.one;
            itemThumbnail.SetVisualState(ItemThumbnail.VisualStateMode.Default);

            // コンテンツ更新
            if (cellData.InteractData is ItemInteractData itemInteractData)
            {
                UpdateContentItemInteractData(itemInteractData);
                gridCellSizeView.gameObject.SetActive(true);
            }
            else
            {
                gridCellSizeView.gameObject.SetActive(false);
            }

            isDirty = true;
        }

        void UpdateContentItemInteractData(ItemInteractData itemInteractData)
        {
            itemThumbnail.Apply(itemInteractData.ItemData, visualStateMode: ItemThumbnail.VisualStateMode.Manual);
            gridCellSizeView.Apply(itemInteractData.ItemData.WidthCount, itemInteractData.ItemData.HeightCount);
        }

        void Update()
        {
            var state = cellData.GetState(cellData.InteractData);
            if (state != null)
            {
                if (!prevStateExist || isDirty)
                {
                    progressIcon.gameObject.SetActive(true);
                    progressGauge.gameObject.SetActive(true);
                    itemThumbnail.SetVisualState(ItemThumbnail.VisualStateMode.GrayOut);
                }

                if (state.InProgress)
                {
                    progressIcon.Rotate(Vector3.forward, -3.0f);
                    progressGauge.anchorMax = new Vector2(state.ProgressRatio, 1.0f);
                }
            }
            else
            {
                if (prevStateExist || isDirty)
                {
                    progressIcon.gameObject.SetActive(false);
                    progressGauge.gameObject.SetActive(false);
                    progressGauge.anchorMax = Vector2.one;
                    itemThumbnail.SetVisualState(ItemThumbnail.VisualStateMode.Default);
                }
            }

            prevStateExist = state != null;
            isDirty = false;

            if (Time.time - lastUpdateTime > 0.25f)
            {
                lastUpdateTime = Time.time;
                distanceText.text = cellData.GetDistanceText(cellData.InteractData);
            }
        }

        void OnClick()
        {
            Context.OnConfirm(cellData);
        }

        void OnClickOption()
        {
        }

        void OnEnter()
        {
            highLight.SetActive(true);
        }

        void OnExit()
        {
            highLight.SetActive(false);
        }
    }
}
