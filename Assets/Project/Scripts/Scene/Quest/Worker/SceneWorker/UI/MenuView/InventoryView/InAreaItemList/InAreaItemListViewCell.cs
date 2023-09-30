using System;
using System.Collections;
using System.Collections.Generic;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class InAreaItemListViewCell : FancyScrollRectCell<InAreaItemListViewCell.CellData, InAreaItemListViewCell.CellContext>
    {
        [SerializeField] Image typeLabel;
        [SerializeField] RectTransform cellParent;
        [SerializeField] RectTransform cellElementPrefab;

        [SerializeField] Text text;
        [SerializeField] Text distanceText;
        [SerializeField] Button button;

        List<RectTransform> cells = new List<RectTransform>();

        CellData cellData;
        float lastUpdateTime;

        public class CellData
        {
            public IInteractData InteractData { get; }
            public bool IsSelected { get; }
            public string NameText { get; private set; }
            public Func<IInteractData, string> GetDistanceText { get; private set; }

            public CellData(IInteractData interactData, bool isSelected, Func<IInteractData, string> getDistanceText)
            {
                InteractData = interactData;
                NameText = interactData.Text;
                IsSelected = isSelected;
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
            button.onClick.AddListener(OnClick);
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;

            text.text = cellData.NameText;
            distanceText.text = cellData.GetDistanceText(cellData.InteractData);

            if (cellData.InteractData is ItemInteractData itemInteractData)
            {
                // TODO: 気になったらパフォーマンスチューニングする
                cellParent.gameObject.SetActive(true);

                foreach (var cell in cells)
                {
                    Destroy(cell.gameObject);
                }

                cells.Clear();

                var cellWidth = 15.0f;
                var cellHeight = 15.0f;
                var offsetWidth = (itemInteractData.ItemData.ItemVO.Width - 1) * -0.5f * cellWidth;
                var offsetHeight = (itemInteractData.ItemData.ItemVO.Height - 1) * -0.5f * cellHeight;
                for (var w = 0; w < itemInteractData.ItemData.ItemVO.Width; w++)
                {
                    for (var h = 0; h < itemInteractData.ItemData.ItemVO.Height; h++)
                    {
                        var cell = Instantiate(cellElementPrefab, cellParent, false);
                        cell.localPosition = new Vector3(offsetWidth + cellWidth * w, offsetHeight + cellHeight * h, 0);
                        cells.Add(cell);
                    }
                }
            }
            else
            {
                cellParent.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (Time.time - lastUpdateTime > 1.0f)
            {
                lastUpdateTime = Time.time;
                distanceText.text = cellData.GetDistanceText(cellData.InteractData);
            }
        }

        void OnClick()
        {
            Context.OnConfirm(cellData);
        }
    }
}
