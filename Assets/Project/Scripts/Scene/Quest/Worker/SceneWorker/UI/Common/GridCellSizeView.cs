using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class GridCellSizeView : MonoBehaviour
    {
        [SerializeField] RectTransform gridCellParent;
        [SerializeField] RectTransform gridCellElementPrefab;
        [SerializeField] float gridCellElementSize;

        List<RectTransform> elements = new List<RectTransform>();

        public void Apply(int width, int height)
        {
            // TODO: 気になったらパフォーマンスチューニングする
            foreach (var cell in elements)
            {
                Destroy(cell.gameObject);
            }

            elements.Clear();

            var offsetWidth = (width - 1) * -0.5f * gridCellElementSize;
            var offsetHeight = (height - 1) * -0.5f * gridCellElementSize;

            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    var cell = Instantiate(gridCellElementPrefab, gridCellParent, false);
                    cell.localPosition = new Vector3(offsetWidth + gridCellElementSize * w, offsetHeight + gridCellElementSize * h, 0);
                    elements.Add(cell);
                }
            }
        }
    }
}
