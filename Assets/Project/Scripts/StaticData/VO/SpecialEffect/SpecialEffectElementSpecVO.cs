using System;

namespace AloneSpace
{
    /// <summary>
    /// SpecialEffectSpecVO
    /// </summary>
    public class SpecialEffectElementSpecVO
    {
        public int Id => row.Id;

        public SpecialEffectElementCategory Category { get; }

        ISpecialEffectElementSpecMasterRow row;

        public SpecialEffectElementSpecVO(SpecialEffectElementCategory category, int id)
        {
            Category = category;

            switch (category)
            {
                case SpecialEffectElementCategory.SpecialEffectTrigger:
                    row = SpecialEffectElementSpecialEffectTriggerSpecMaster.Instance.Get(id);
                    break;
                case SpecialEffectElementCategory.AddWeaponEffect:
                    row = SpecialEffectElementAddWeaponEffectSpecMaster.Instance.Get(id);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
