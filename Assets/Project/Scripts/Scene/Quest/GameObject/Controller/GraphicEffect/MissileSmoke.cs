using UnityEngine;

namespace AloneSpace
{
    public class MissileSmoke : GraphicEffect
    {
        [SerializeField] TrailRenderer trailRenderer;
        MissileGraphicEffectHandler missileGraphicEffectHandler;

        protected override void OnInit()
        {
            missileGraphicEffectHandler = (MissileGraphicEffectHandler)GraphicEffectHandler;

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;

            trailRenderer.Clear();
        }

        public override void OnLateUpdate(float deltaTime)
        {
            if (missileGraphicEffectHandler.IsAbandoned)
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