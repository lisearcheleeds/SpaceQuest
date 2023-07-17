using System;

namespace AloneSpace
{
    public class SpecialEffectElementData : IReleasableData, IOrderModuleHolder
    {
        public bool IsReleased { get; private set; }

        public IOrderModule OrderModule { get; protected set; }

        public SpecialEffectElementSpecVO SpecialEffectElementSpecVO { get; }

        public int Count { get; }
        public float RemainingTime { get; }
        public float IntervalTime { get; }
        public int RemainingCount { get; }

        public SpecialEffectElementData(SpecialEffectElementSpecVO specialEffectElementSpecVO)
        {
            SpecialEffectElementSpecVO = specialEffectElementSpecVO;

            switch (specialEffectElementSpecVO.Category)
            {
                case SpecialEffectElementCategory.SpecialEffectTrigger:
                    OrderModule = new SpecialEffectTriggerOrderModule(this);
                    break;
                case SpecialEffectElementCategory.AddWeaponEffect:
                    OrderModule = new AddWeaponEffectOrderModule(this);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void ActivateModules()
        {
            OrderModule.ActivateModule();
        }

        public void DeactivateModules()
        {
            OrderModule.DeactivateModule();

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
            OrderModule = null;
        }

        public void Release()
        {
            IsReleased = true;
        }
    }
}
