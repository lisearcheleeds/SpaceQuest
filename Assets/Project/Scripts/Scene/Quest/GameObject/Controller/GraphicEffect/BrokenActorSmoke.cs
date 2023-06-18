using UnityEngine;

namespace AloneSpace
{
    public class BrokenActorSmoke : GraphicEffect
    {
        [SerializeField] TrailRenderer trailRenderer;
        BrokenActorSmokeGraphicEffectHandler brokenActorSmokeGraphicEffectHandler;

        protected override void OnInit()
        {
            brokenActorSmokeGraphicEffectHandler = (BrokenActorSmokeGraphicEffectHandler)GraphicEffectHandler;

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;

            trailRenderer.Clear();
        }

        public override void OnLateUpdate(float deltaTime)
        {
            if (brokenActorSmokeGraphicEffectHandler.IsAbandoned)
            {
                if (trailRenderer.positionCount == 0)
                {
                    IsCompleted = true;
                }

                return;
            }

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;
        }
    }
}
