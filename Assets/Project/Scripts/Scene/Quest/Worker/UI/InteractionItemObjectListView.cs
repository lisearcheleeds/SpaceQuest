using System;
using FancyScrollView;
using UnityEngine;

namespace AloneSpace
{
    public class InteractionItemObjectListView : FancyScrollRect<InteractionItemObjectListViewCell.CellData, InteractionItemObjectListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(
            InteractionItemObjectListViewCell.CellData[] cellData,
            ItemInteractData[][] takeOrderItems,
            InteractionItemObjectListViewCell.CellData selectCellData,
            Action<InteractionItemObjectListViewCell.CellData> onClick)
        {
            Context.TakeOrderItems = takeOrderItems;
            Context.SelectCellData = selectCellData;
            Context.OnClick = onClick;
            UpdateContents(cellData);
        }
    }
}
