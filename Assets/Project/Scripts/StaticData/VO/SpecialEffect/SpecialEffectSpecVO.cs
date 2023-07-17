using System.Linq;

namespace AloneSpace
{
    /// <summary>
    /// SpecialEffectSpecVO
    /// </summary>
    public class SpecialEffectSpecVO
    {
        public int Id => row.Id;
        public string Name => row.Name;

        public SpecialEffectElementSpecVO[] SpecialEffectElementSpecVOs { get; }

        SpecialEffectSpecMaster.Row row;

        public SpecialEffectSpecVO(int id)
        {
            row = SpecialEffectSpecMaster.Instance.Get(id);

            var specialEffectRelationMaster = SpecialEffectRelationMaster.Instance.GetRange(id);
            SpecialEffectElementSpecVOs = specialEffectRelationMaster.Select(x => new SpecialEffectElementSpecVO(x.Category, x.SpecialEffectElementSpecId)).ToArray();
        }
    }
}
