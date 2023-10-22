using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardGridCell : GridCell
    {
        protected override ICellActions CellActions => button;

        [SerializeField] Graphic background;
        [SerializeField] RawImage cellImage;
        [SerializeField] GameObject highlight;
        [SerializeField] GameObject raycastTarget;

        [SerializeField] StandardButton button;

        public virtual void SetHighLight(bool value)
        {
            highlight.SetActive(value);
        }

        protected override void OnApply()
        {
            base.OnApply();
            SetHighLight(false);

            if (GridCellData == null)
            {
                cellImage.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
                return;
            }

            // update cell image
            cellImage.gameObject.SetActive(false);
            StartCoroutine(LoadAsync(((StandardGridCellData)GridCellData).ImagePath, tex =>
            {
                cellImage.texture = tex;
                cellImage.gameObject.SetActive(true);
            }));

            IEnumerator LoadAsync(string path, Action<Texture2D> onLoad)
            {
                var loader = Resources.LoadAsync<Texture2D>(path);
                yield return loader;
                onLoad(loader.asset as Texture2D);
            }

            background.gameObject.SetActive(CellActions.IsActive);
        }

        public override void SetClickable(bool clickable)
        {
            button.interactable = clickable;
            raycastTarget.SetActive(clickable);
        }
    }
}
