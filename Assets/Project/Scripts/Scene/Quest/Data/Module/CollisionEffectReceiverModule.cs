namespace AloneSpace
{
    public class CollisionEffectReceiverModule : IModule
    {
        public void AddHit(ICollisionDataHolder otherCollisionDataHolder)
        {
        }
        
        public void OnUpdateModule(float deltaTime)
        {
            // MessageBus.Instance.NoticeCollisionEffectData.Broadcast(new CollisionEffectData(sender, receiver));
        }
    }
}