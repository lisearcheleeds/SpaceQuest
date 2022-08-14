using UnityEngine;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardCell : VariableInventoryCell
    {
        public override Vector2 CellSize => cellSize;

        protected override IVariableInventoryCellActions ButtonActions => button;
        protected virtual StandardAssetLoader Loader { get; set; }

        [SerializeField] Vector2 cellSize;

        [SerializeField] RectTransform sizeRoot;
        [SerializeField] RectTransform target;
        [SerializeField] Graphic background;
        [SerializeField] RawImage cellImage;
        [SerializeField] GameObject highlight;

        [SerializeField] StandardButton button;
        
        IVariableInventoryAsset currentImageAsset;

        public override void SetSelectable(bool value)
        {
            ButtonActions.IsActive = value;
        }

        public virtual void SetHighLight(bool value)
        {
            highlight.SetActive(value);
        }

        protected override void OnApply()
        {
            SetHighLight(false);
            target.gameObject.SetActive(CellData != null);
            ApplySize();

            if (CellData == null)
            {
                cellImage.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
            }
            else
            {
                // update cell image
                if (currentImageAsset != CellData.ImageAsset)
                {
                    currentImageAsset = CellData.ImageAsset;

                    cellImage.gameObject.SetActive(false);
                    if (Loader == null)
                    {
                        Loader = new StandardAssetLoader();
                    }

                    StartCoroutine(Loader.LoadAsync(CellData.ImageAsset, tex =>
                    {
                        cellImage.texture = tex;
                        cellImage.gameObject.SetActive(true);
                    }));
                }

                background.gameObject.SetActive(ButtonActions.IsActive);
            }
        }

        protected virtual void ApplySize()
        {
            sizeRoot.sizeDelta = RotateCellDataSize;
            target.sizeDelta = CellDataSize;
            target.localEulerAngles = Vector3.forward * (CellData?.IsRotate ?? false ? 90 : 0);
        }
    }
}
