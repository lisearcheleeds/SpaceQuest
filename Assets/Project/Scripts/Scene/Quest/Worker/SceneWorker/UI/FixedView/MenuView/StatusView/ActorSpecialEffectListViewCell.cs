using System;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class ActorSpecialEffectListViewCell : FancyScrollRectCell<ActorSpecialEffectListViewCell.CellData, ActorSpecialEffectListViewCell.CellContext>
    {
        [SerializeField] Image SourceTypeImage;
        [SerializeField] Image icon;
        [SerializeField] Text nameText;
        [SerializeField] Text descriptionText;

        [SerializeField] Color selfActorSpecColor;
        [SerializeField] Color selfActorPresetColor;
        [SerializeField] Color selfWeaponColor;
        [SerializeField] Color otherColor;

        CellData cellData;

        public class CellData
        {
            public SpecialEffectData SpecialEffectData { get; }

            public CellData(SpecialEffectData specialEffectData)
            {
                SpecialEffectData = specialEffectData;
            }
        }

        public class CellContext : FancyScrollRectContext
        {
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;
            SourceTypeImage.color = GetSourceColor(cellData.SpecialEffectData.SpecialEffectSourceType);

            nameText.text = cellData.SpecialEffectData.SpecialEffectSpecVO.Name;
            descriptionText.text = cellData.SpecialEffectData.SpecialEffectSpecVO.Description;
        }

        Color GetSourceColor(SpecialEffectSourceType specialEffectSourceType)
        {
            switch (specialEffectSourceType)
            {
                case SpecialEffectSourceType.SelfActorSpec: return selfActorSpecColor;
                case SpecialEffectSourceType.SelfActorPreset: return selfActorPresetColor;
                case SpecialEffectSourceType.SelfWeapon: return selfWeaponColor;
                default: return otherColor;
            }
        }
    }
}
