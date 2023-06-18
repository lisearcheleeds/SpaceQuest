namespace AloneSpace
{
    public class BrokenActorGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData => ActorData;

        public ActorData ActorData { get; }

        public GraphicEffectSpecVO BrokenActorSmokeGraphicEffectSpecVO { get; }

        public BrokenActorGraphicEffectHandler(ActorData actorData, GraphicEffectSpecVO brokenActorSmokeGraphicEffectSpecVO)
        {
            ActorData = actorData;
            BrokenActorSmokeGraphicEffectSpecVO = brokenActorSmokeGraphicEffectSpecVO;
        }
    }
}
