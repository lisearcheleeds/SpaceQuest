using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class ContentQuickViewWindow : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] GameObject textObject;
        [SerializeField] Text text;

        [SerializeField] GameObject optionTextObject;
        [SerializeField] Text optionText;

        [SerializeField] ItemThumbnail itemThumbnail;

        [SerializeField] GameObject gridCellObject;
        [SerializeField] RectTransform gridCellParent;
        [SerializeField] RectTransform gridCellElementPrefab;
        [SerializeField] float gridCellSize;
        List<RectTransform> gridCells = new List<RectTransform>();

        RectTransform rectTransform;
        bool followPointerPosition;

        public void Initialize()
        {
            rectTransform = transform as RectTransform;

            Apply(null, false);
        }

        public void Finalize()
        {
        }

        public void Apply(IContentQuickViewData contentQuickViewData, bool followPointerPosition)
        {
            this.followPointerPosition = followPointerPosition;

            switch (contentQuickViewData)
            {
                case ItemInteractData itemInteractData:
                    gameObject.SetActive(true);
                    ApplyTextObject(itemInteractData.Text);
                    ApplyOptionTextObject(null);
                    ApplyThumbnailObject(itemInteractData.ItemData);
                    ApplyGridCellObject(itemInteractData.ItemData.WidthCount, itemInteractData.ItemData.HeightCount);
                    break;
                case ItemData itemData:
                    gameObject.SetActive(true);
                    ApplyTextObject(itemData.ItemVO.Text);
                    ApplyOptionTextObject(null);
                    ApplyThumbnailObject(itemData);
                    ApplyGridCellObject(itemData.WidthCount, itemData.HeightCount);
                    break;
                default:
                    gameObject.SetActive(false);
                    ApplyTextObject(null);
                    ApplyOptionTextObject(null);
                    ApplyThumbnailObject(null);
                    ApplyGridCellObject(0, 0);
                    MessageBus.Instance.UserInputCloseContentQuickView.Broadcast();
                    break;
            }

            if (!followPointerPosition)
            {
                rectTransform.localPosition = (new Vector2(Screen.width, Screen.height) * 0.5f);
            }
        }

        public void OnUpdate()
        {
            if (followPointerPosition)
            {
                var targetPosition = Mouse.current.position.ReadValue();
                var offsetX = rectTransform.sizeDelta.x * 0.5f;
                var offsetY = rectTransform.sizeDelta.y * 0.5f;
                targetPosition.x = Mathf.Min(targetPosition.x + offsetX, Screen.width - offsetX);
                targetPosition.y = Mathf.Min(targetPosition.y + offsetY, Screen.height - offsetY);
                rectTransform.localPosition = targetPosition - (new Vector2(Screen.width, Screen.height) * 0.5f);
            }
        }

        void ApplyTextObject(string textData)
        {
            textObject.SetActive(!string.IsNullOrEmpty(textData));
            text.text = textData;
        }

        void ApplyOptionTextObject(string optionTextData)
        {
            optionTextObject.SetActive(!string.IsNullOrEmpty(optionTextData));
            optionText.text = optionTextData;
        }

        void ApplyThumbnailObject(ItemData itemData)
        {
            itemThumbnail.Apply(itemData, ItemThumbnail.LayoutMode.NoText);
        }

        void ApplyGridCellObject(int width, int height)
        {
            gridCellObject.SetActive(width != 0 && height != 0);

            if (width == 0 || height == 0)
            {
                return;
            }

            foreach (var cell in gridCells)
            {
                Destroy(cell.gameObject);
            }

            gridCells.Clear();

            var offsetWidth = (width - 1) * -0.5f * gridCellSize;
            var offsetHeight = (height - 1) * -0.5f * gridCellSize;
            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    var cell = Instantiate(gridCellElementPrefab, gridCellParent, false);
                    cell.localPosition = new Vector3(offsetWidth + gridCellSize * w, offsetHeight + gridCellSize * h, 0);
                    gridCells.Add(cell);
                }
            }
        }
    }
}
