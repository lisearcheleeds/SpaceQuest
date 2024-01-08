namespace AloneSpace
{
    public abstract class SpecialEffectElementOrderModule : IOrderModule
    {
        public SpecialEffectElementData SpecialEffectElementData { get; }

        protected SpecialEffectElementOrderModule(SpecialEffectElementData specialEffectElementData)
        {
            SpecialEffectElementData = specialEffectElementData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.Module.RegisterOrderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.Module.UnRegisterOrderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
        }
    }
}
