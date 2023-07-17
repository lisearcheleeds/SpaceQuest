using System;
using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectData : IReleasableData, IModuleHolder
    {
        public bool IsReleased => SpecialEffectElementDataList.All(x => x.IsReleased);

        public SpecialEffectSpecVO SpecialEffectSpecVO { get; }
        public Guid? SourceActorInstanceId { get; }

        public SpecialEffectElementData[] SpecialEffectElementDataList { get; }

        public SpecialEffectData(SpecialEffectSpecVO specialEffectSpecVO, Guid? sourceActorInstanceId)
        {
            SpecialEffectSpecVO = specialEffectSpecVO;
            SourceActorInstanceId = sourceActorInstanceId;
            SpecialEffectElementDataList = specialEffectSpecVO.SpecialEffectElementSpecVOs.Select(x => new SpecialEffectElementData(x)).ToArray();
        }

        public void ActivateModules()
        {
            foreach (var specialEffectElementData in SpecialEffectElementDataList)
            {
                specialEffectElementData.ActivateModules();
            }
        }

        public void DeactivateModules()
        {
            foreach (var specialEffectElementData in SpecialEffectElementDataList)
            {
                specialEffectElementData.DeactivateModules();
            }
        }

        public void Release()
        {
            foreach (var specialEffectElementData in SpecialEffectElementDataList)
            {
                specialEffectElementData.Release();
            }
        }
    }
}
