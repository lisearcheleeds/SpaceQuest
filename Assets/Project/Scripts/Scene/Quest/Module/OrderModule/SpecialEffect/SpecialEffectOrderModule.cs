namespace AloneSpace
{
    public abstract class SpecialEffectOrderModule : IOrderModule
    {
        public SpecialEffectElementData SpecialEffectElementData { get; }

        protected SpecialEffectOrderModule(SpecialEffectElementData specialEffectElementData)
        {
            SpecialEffectElementData = specialEffectElementData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterOrderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterOrderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
        }
    }
}
