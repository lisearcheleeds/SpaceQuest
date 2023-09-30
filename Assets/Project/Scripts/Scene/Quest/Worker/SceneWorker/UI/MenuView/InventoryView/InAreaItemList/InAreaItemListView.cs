using System;
using FancyScrollView;
using UnityEngine;

namespace AloneSpace
{
    public class InAreaItemListView : FancyScrollRect<InAreaItemListViewCell.CellData, InAreaItemListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(
            InAreaItemListViewCell.CellData[] cellData,
            Action<InAreaItemListViewCell.CellData> onSelect,
            Action<InAreaItemListViewCell.CellData> onConfirm)
        {
            Context.OnSelect = onSelect;
            Context.OnConfirm = onConfirm;
            UpdateContents(cellData);
        }
    }
}
