namespace AloneSpace
{
    public class CollisionEffectSenderModule : IModule
    {
        public void AddHit(ICollisionDataHolder otherCollisionDataHolder)
        {
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterCollisionEffectSenderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterCollisionEffectSenderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
        }
    }
}