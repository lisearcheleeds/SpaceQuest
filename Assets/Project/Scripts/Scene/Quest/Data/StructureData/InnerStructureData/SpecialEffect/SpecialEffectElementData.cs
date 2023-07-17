using System;

namespace AloneSpace
{
    public class SpecialEffectElementData : IReleasableData, IOrderModuleHolder
    {
        public bool IsReleased { get; private set; }

        public IOrderModule OrderModule { get; protected set; }

        public SpecialEffectElementSpecVO SpecialEffectElementSpecVO { get; }

        public SpecialEffectElementStateData StateData { get; } = new SpecialEffectElementStateData();

        public SpecialEffectElementData(SpecialEffectElementSpecVO specialEffectElementSpecVO)
        {
            SpecialEffectElementSpecVO = specialEffectElementSpecVO;

            StateData.RemainingTime = specialEffectElementSpecVO.EffectTime;
            StateData.RemainingExectureCount = specialEffectElementSpecVO.MaxExecuteCount;

            switch (specialEffectElementSpecVO.Category)
            {
                case SpecialEffectElementCategory.SpecialEffectTrigger:
                    OrderModule = new SpecialEffectElementTriggerOrderModule(this);
                    break;
                case SpecialEffectElementCategory.AddWeaponEffect:
                    OrderModule = new AddWeaponEffectElementOrderModule(this);
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
