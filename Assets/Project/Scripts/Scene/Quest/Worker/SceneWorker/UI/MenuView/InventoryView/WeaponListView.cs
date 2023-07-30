using System;
using UnityEngine;
using FancyScrollView;

namespace AloneSpace
{
    public class WeaponListView : FancyScrollRect<WeaponListViewCell.CellData, WeaponListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(WeaponListViewCell.CellData[] cellData)
        {
            UpdateContents(cellData);
        }
    }
}
