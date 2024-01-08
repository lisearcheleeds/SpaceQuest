using System;
using UnityEngine;

namespace AloneSpace
{
    public class PullItem : GraphicEffect
    {
        [SerializeField] LineRenderer lineRenderer;
        PullItemGraphicEffectHandler pullItemGraphicEffectHandler;

        protected override void OnInit()
        {
            pullItemGraphicEffectHandler = (PullItemGraphicEffectHandler)GraphicEffectHandler;

            transform.position = pullItemGraphicEffectHandler.PositionData.Position;

            lineRenderer.SetPositions(new[] { 
                pullItemGraphicEffectHandler.PositionData.Position, 
                pullItemGraphicEffectHandler.PositionData.Position });
        }

        public override void OnLateUpdate(float deltaTime)
        {
            if (pullItemGraphicEffectHandler.IsAbandoned)
            {
                IsCompleted = true;

                return;
            }

            lineRenderer.SetPosition(0, pullItemGraphicEffectHandler.PositionData.Position);
            lineRenderer.SetPosition(1, pullItemGraphicEffectHandler.ItemPositionData.Position);
        }
    }
}