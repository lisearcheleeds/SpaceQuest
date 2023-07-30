using System;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class WeaponListViewCell : FancyScrollRectCell<WeaponListViewCell.CellData, WeaponListViewCell.CellContext>
    {
        [SerializeField] Image icon;
        [SerializeField] Text text;

        CellData cellData;
        float lastUpdateTime;

        public class CellData
        {
            public IWeaponSpecVO WeaponSpecVO { get; }

            public CellData(IWeaponSpecVO weaponSpecVO)
            {
                WeaponSpecVO = weaponSpecVO;
            }
        }

        public class CellContext : FancyScrollRectContext
        {
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;
            text.text = cellData.WeaponSpecVO.Name;
        }
    }
}
