namespace AloneSpace
{
    public class CollisionEffectReceiverModule : IModule
    {
        public void AddHit(ICollisionDataHolder otherCollisionDataHolder)
        {
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterCollisionEffectReceiverModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterCollisionEffectReceiverModule.Broadcast(this);
        }
        
        public void OnUpdateModule(float deltaTime)
        {
            // MessageBus.Instance.NoticeCollisionEffectData.Broadcast(new CollisionEffectData(sender, receiver));
        }
    }
}