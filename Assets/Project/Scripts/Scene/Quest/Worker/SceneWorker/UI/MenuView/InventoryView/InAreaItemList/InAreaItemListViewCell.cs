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
        [SerializeField] RawImage icon;
        [SerializeField] float baseIconSize;

        [SerializeField] RectTransform cellParent;
        [SerializeField] RectTransform cellElementPrefab;
        [SerializeField] float cellSize;

        [SerializeField] RectTransform progressIcon;
        [SerializeField] RectTransform progressGauge;

        [SerializeField] Text text;
        [SerializeField] Text distanceText;
        [SerializeField] Button button;

        List<RectTransform> cells = new List<RectTransform>();

        CellData cellData;
        float lastUpdateTime;
        bool prevStateExist;

        public class CellData
        {
            public IInteractData InteractData { get; }
            public bool IsSelected { get; }
            public string NameText { get; }

            public Func<IInteractData, ActorStateData.InteractOrderState> GetState { get; }
            public Func<IInteractData, string> GetDistanceText { get; }

            public CellData(
                IInteractData interactData,
                bool isSelected,
                Func<IInteractData, ActorStateData.InteractOrderState> getState,
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
            button.onClick.AddListener(OnClick);
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
            icon.color = Color.white;

            // コンテンツ更新
            if (cellData.InteractData is ItemInteractData itemInteractData)
            {
                UpdateContentItemInteractData(itemInteractData);
            }
            else
            {
                cellParent.gameObject.SetActive(false);
            }
        }

        void UpdateContentItemInteractData(ItemInteractData itemInteractData)
        {
            // TODO: 気になったらパフォーマンスチューニングする
            cellParent.gameObject.SetActive(true);

            foreach (var cell in cells)
            {
                Destroy(cell.gameObject);
            }

            cells.Clear();

            icon.gameObject.SetActive(false);
            StartCoroutine(LoadAsync(itemInteractData.ItemData.ImagePath, tex =>
            {
                icon.texture = tex;
                icon.rectTransform.sizeDelta =
                    tex.texelSize.x < tex.texelSize.y
                        ? new Vector2(baseIconSize * (tex.texelSize.x / tex.texelSize.y), baseIconSize)
                        : new Vector2(baseIconSize, baseIconSize * (tex.texelSize.y / tex.texelSize.x));
                icon.gameObject.SetActive(true);
            }));

            IEnumerator LoadAsync(string path, Action<Texture2D> onLoad)
            {
                var loader = Resources.LoadAsync<Texture2D>(path);
                yield return loader;
                onLoad(loader.asset as Texture2D);
            }

            var offsetWidth = (itemInteractData.ItemData.ItemVO.Width - 1) * -0.5f * cellSize;
            var offsetHeight = (itemInteractData.ItemData.ItemVO.Height - 1) * -0.5f * cellSize;
            for (var w = 0; w < itemInteractData.ItemData.ItemVO.Width; w++)
            {
                for (var h = 0; h < itemInteractData.ItemData.ItemVO.Height; h++)
                {
                    var cell = Instantiate(cellElementPrefab, cellParent, false);
                    cell.localPosition = new Vector3(offsetWidth + cellSize * w, offsetHeight + cellSize * h, 0);
                    cells.Add(cell);
                }
            }
        }

        void Update()
        {
            var state = cellData.GetState(cellData.InteractData);
            if (state != null)
            {
                if (!prevStateExist)
                {
                    progressIcon.gameObject.SetActive(true);
                    progressGauge.gameObject.SetActive(true);
                    icon.color = Color.gray;
                }

                if (state.InProgress)
                {
                    progressIcon.Rotate(Vector3.forward, -3.0f);
                    progressGauge.anchorMax = new Vector2(state.ProgressRatio, 1.0f);
                }
            }
            else
            {
                if (prevStateExist)
                {
                    progressIcon.gameObject.SetActive(false);
                    progressGauge.gameObject.SetActive(false);
                    progressGauge.anchorMax = Vector2.one;
                    icon.color = Color.white;
                }
            }

            prevStateExist = state != null;

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
    }
}
