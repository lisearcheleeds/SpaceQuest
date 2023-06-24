using UnityEngine;

namespace AloneSpace
{
    public class BrokenActorGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData => ActorData;

        public ActorData ActorData { get; }

        public GraphicEffectSpecVO BrokenActorSmokeGraphicEffectSpecVO { get; }

        public Vector3 MovementVelocity { get; }

        public BrokenActorGraphicEffectHandler(
            ActorData actorData,
            GraphicEffectSpecVO brokenActorSmokeGraphicEffectSpecVO,
            Vector3 movementVelocity)
        {
            ActorData = actorData;
            BrokenActorSmokeGraphicEffectSpecVO = brokenActorSmokeGraphicEffectSpecVO;
            MovementVelocity = movementVelocity;
        }
    }
}
