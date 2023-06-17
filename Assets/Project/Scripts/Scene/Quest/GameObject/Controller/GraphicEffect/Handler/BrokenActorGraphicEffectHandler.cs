namespace AloneSpace
{
    public class BrokenActorGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData => ActorData;

        public ActorData ActorData { get; }

        public BrokenActorGraphicEffectHandler(ActorData actorData)
        {
            ActorData = actorData;
        }
    }
}
